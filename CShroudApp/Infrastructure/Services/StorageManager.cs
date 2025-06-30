using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CShroudApp.Core.Configs;
using CShroudApp.Core.Interfaces;
using MessagePack;
using Microsoft.Extensions.Options;

namespace CShroudApp.Infrastructure.Services;

public class StorageManager : IStorageManager
{
    public struct ContainerStruct
    {
        public object Value;
        public DateTime? AliveUntil;
    }
    
    private Dictionary<string, ContainerStruct> _storage;

    public StorageManager()
    {
        _storage = Load();
    }
    
    public Dictionary<string, ContainerStruct> Load()
    {
        if (!File.Exists(AppConstants.CacheFilePath))
            return new Dictionary<string, ContainerStruct>();

        byte[] encrypted = File.ReadAllBytes(AppConstants.CacheFilePath);

        var deserialized = MessagePackSerializer.Typeless.Deserialize(Decrypt(encrypted, GetEncryptionKey()))
            as Dictionary<string, ContainerStruct>;

        return deserialized ?? new Dictionary<string, ContainerStruct>();
    }
    
    private static byte[] GetEncryptionKey()
    {
        string deviceInfo = Environment.MachineName + Environment.UserName + Environment.OSVersion;
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(deviceInfo));
    }
    
    private static byte[] Encrypt(byte[] data, byte[] key)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.GenerateIV();

        using var encryptor = aesAlg.CreateEncryptor();
        byte[] encrypted = encryptor.TransformFinalBlock(data, 0, data.Length);
        
        byte[] result = new byte[aesAlg.IV.Length + encrypted.Length];
        Buffer.BlockCopy(aesAlg.IV, 0, result, 0, aesAlg.IV.Length);
        Buffer.BlockCopy(encrypted, 0, result, aesAlg.IV.Length, encrypted.Length);

        return result;
    }
    
    private static byte[] Decrypt(byte[] cipherTextCombined, byte[] key)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = key;
        
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
        var value = _storage.GetValueOrDefault(key);
        if (!(value.AliveUntil is null || DateTime.UtcNow < value.AliveUntil)) return null;
        return value.Value as TEntity;
    }
        
    public async Task SetValue(string key, object data, TimeSpan? aliveTime = null, bool saveChanges = true)
    {
        _storage[key] = new ContainerStruct { Value = data, AliveUntil = aliveTime is not null ? DateTime.UtcNow + aliveTime : null };
        if (saveChanges)
            await SaveChanges();
    } 
    
    public async Task SetValueIfNot(string key, object data, TimeSpan? aliveTime = null, bool saveChanges = true)
    {
        if (_storage.TryGetValue(key, out var value) && value.Value == data && (value.AliveUntil is null || DateTime.UtcNow < value.AliveUntil)) return;
        _storage[key] = new ContainerStruct { Value = data, AliveUntil = aliveTime is not null ? DateTime.UtcNow + aliveTime : null };
        if (saveChanges)
            await SaveChanges();
    }

    public async Task DelValue(string key, bool saveChanges = true)
    {
        _storage.Remove(key);
        if (saveChanges)
            await SaveChanges();
    }

    public async Task SaveChanges()
    {
        var data = MessagePackSerializer.Typeless.Serialize(_storage);
        var directory = Path.GetDirectoryName(AppConstants.CacheFilePath);
        if (directory is not null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
            
        await File.WriteAllBytesAsync(AppConstants.CacheFilePath, Encrypt(data, GetEncryptionKey()));
    }

    public string? RefreshToken
    {
        get => GetValue<string>("refreshToken");
        set
        {
            if (value is not null)
                Task.Run(() => SetValueIfNot("refreshToken", value));
        }
    }
}