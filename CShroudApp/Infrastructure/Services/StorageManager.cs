using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using CShroudApp.Application.Serialization;
using CShroudApp.Core.Interfaces;
using CShroudApp.Infrastructure.Data.Config;
using MessagePack;
using Microsoft.Extensions.Options;


namespace CShroudApp.Infrastructure.Services;

public class StorageManager : IStorageManager
{
    private Dictionary<string, object> _storage;
    private SettingsConfig _settingsConfig;
    
    public StorageManager(IOptions<SettingsConfig> settingsConfig)
    {
        _storage = Load();
        _settingsConfig = settingsConfig.Value;
    }
    
    public Dictionary<string, object> Load()
    {
        Console.WriteLine(GlobalConstants.DataFilePath);
        if (!File.Exists(GlobalConstants.DataFilePath))
            return new Dictionary<string, object>();

        byte[] encrypted = File.ReadAllBytes(GlobalConstants.DataFilePath);

        var deserialized = MessagePackSerializer.Typeless.Deserialize(Decrypt(encrypted, GetEncryptionKey()))
            as Dictionary<string, object>;

        return deserialized ?? new Dictionary<string, object>();
    }
    
    private static byte[] GetEncryptionKey()
    {
        string deviceInfo = Environment.MachineName + Environment.UserName + Environment.OSVersion;
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(deviceInfo));
    }
    
    public static void ClearToken()
    {
        if (File.Exists(GlobalConstants.DataFilePath))
                File.Delete(GlobalConstants.DataFilePath);
    }

        // AES-шифрование
    private static byte[] Encrypt(byte[] data, byte[] key)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.GenerateIV();

        using var encryptor = aesAlg.CreateEncryptor();
        byte[] encrypted = encryptor.TransformFinalBlock(data, 0, data.Length);

            // Склеиваем IV + данные
        byte[] result = new byte[aesAlg.IV.Length + encrypted.Length];
        Buffer.BlockCopy(aesAlg.IV, 0, result, 0, aesAlg.IV.Length);
        Buffer.BlockCopy(encrypted, 0, result, aesAlg.IV.Length, encrypted.Length);

        return result;
    }

        // AES-дешифрование
    private static byte[] Decrypt(byte[] cipherTextCombined, byte[] key)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = key;

            // Выделяем IV
        byte[] iv = new byte[aesAlg.BlockSize / 8];
        Buffer.BlockCopy(cipherTextCombined, 0, iv, 0, iv.Length);
        aesAlg.IV = iv;

        using var decryptor = aesAlg.CreateDecryptor();
        byte[] cipherText = new byte[cipherTextCombined.Length - iv.Length];
        Buffer.BlockCopy(cipherTextCombined, iv.Length, cipherText, 0, cipherText.Length);

        byte[] decrypted = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

        return decrypted;
    }

    public TEntity? GetValue<TEntity>(string key) where TEntity : class
    {
        return _storage.GetValueOrDefault(key) as TEntity;
    }
        
    public async Task SetValue(string key, object data, bool saveChanges = true)
    {
        _storage[key] = data;
        if (saveChanges)
            await SaveChanges();
    }

    public async Task SaveConfigAsync()
    {
        var directory = Path.GetDirectoryName(GlobalConstants.DataFilePath);
        if (directory is not null && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        
        var json = JsonSerializer.Serialize(_settingsConfig, AppJsonContext.Default.SettingsConfig);
        
        await File.WriteAllTextAsync(GlobalConstants.ApplicationConfigPath, json);
    }

    public async Task SaveChanges()
    {
        var data = MessagePackSerializer.Typeless.Serialize(_storage);
        var directory = Path.GetDirectoryName(GlobalConstants.DataFilePath);
        if (directory is not null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
            
        await File.WriteAllBytesAsync(GlobalConstants.DataFilePath, Encrypt(data, GetEncryptionKey()));
    }

    public string? RefreshToken
    {
        get => GetValue<string>("refreshToken");
        set
        {
            if (value is not null)
                Task.Run(() => SetValue("refreshToken", value));
        }
    }
}