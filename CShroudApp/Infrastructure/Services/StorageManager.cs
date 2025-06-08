using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using CShroudApp.Core.Interfaces;
using MessagePack;


namespace CShroudApp.Infrastructure.Services;

public class StorageManager : IStorageManager
{
    private static readonly string AppFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CrimsonShroud");
    private static readonly string DataFilePath = Path.Combine(AppFolderPath, "data.cache");

    private Dictionary<string, object> _storage;

    public StorageManager()
    {
        _storage = Load();
    }
    
    public Dictionary<string, object> Load()
    {
        Console.WriteLine(DataFilePath);
        if (!File.Exists(DataFilePath))
            return new Dictionary<string, object>();

        byte[] encrypted = File.ReadAllBytes(DataFilePath);

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
        if (File.Exists(DataFilePath))
                File.Delete(DataFilePath);
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

    public async Task SaveChanges()
    {
        var data = MessagePackSerializer.Typeless.Serialize(_storage);
        var directory = Path.GetDirectoryName(DataFilePath);
        if (directory is not null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
            
        await File.WriteAllBytesAsync(DataFilePath, Encrypt(data, GetEncryptionKey()));
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