using System.Security.Cryptography;

namespace CShroudGateway.Core.Interfaces;

public interface IRsaEncrypter
{
    public static RSA GeneratePrivateKey(int keySize = 1024)
    {
        var rsa = RSA.Create();
        rsa.KeySize = keySize;
        return rsa;
    }
    
    public static byte[] GenerateBytesPrivateKey(int keySize = 1024)
    {
        var rsa = RSA.Create();
        rsa.KeySize = keySize;
        return rsa.ExportPkcs8PrivateKey();
    }

    public static byte[] ExportPublicKey(RSA privateKey)
    {
        // Export as X.509 SubjectPublicKeyInfo
        return privateKey.ExportSubjectPublicKeyInfo();
    }

    public static byte[] ExportPrivateKey(RSA privateKey)
    {
        // Export as PKCS#8 private key bytes
        return privateKey.ExportPkcs8PrivateKey();
    }

    public static RSA ImportPublicKey(byte[] publicKeyBytes)
    {
        var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
        return rsa;
    }

    public static RSA ImportPrivateKey(byte[] privateKeyBytes)
    {
        var rsa = RSA.Create();
        rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
        return rsa;
    }

    public static byte[] Encrypt(byte[] data, byte[] publicKeyBytes)
    {
        using var rsa = ImportPublicKey(publicKeyBytes);
        return rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
    }

    public static byte[] Decrypt(byte[] encryptedData, byte[] privateKeyBytes)
    {
        using var rsa = ImportPrivateKey(privateKeyBytes);
        return rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);
    }
}