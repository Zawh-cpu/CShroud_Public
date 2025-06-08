namespace CShroudApp.Infrastructure.Services;

public class CacheManager
{
    private static readonly string AppFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CrimsonShroud");
    private static readonly string TokenFilePath = Path.Combine(AppFolderPath, "token.cache");
    
    private static byte[] GetEncryptionKey()
        {
            string deviceInfo = Environment.MachineName + Environment.UserName + Environment.OSVersion;
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(deviceInfo));
        }

        public static void SaveToken(string token)
        {
            Directory.CreateDirectory(AppFolder);

            byte[] key = GetEncryptionKey();
            byte[] encrypted = EncryptStringToBytes(token, key);

            File.WriteAllBytes(TokenFile, encrypted);
        }

        public static string? LoadToken()
        {
            if (!File.Exists(TokenFile))
                return null;

            byte[] key = GetEncryptionKey();
            byte[] encrypted = File.ReadAllBytes(TokenFile);

            return DecryptStringFromBytes(encrypted, key);
        }

        public static void ClearToken()
        {
            if (File.Exists(TokenFile))
                File.Delete(TokenFile);
        }

        // AES-шифрование
        private static byte[] EncryptStringToBytes(string plainText, byte[] key)
        {
            using var aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.GenerateIV();

            using var encryptor = aesAlg.CreateEncryptor();
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // Склеиваем IV + данные
            byte[] result = new byte[aesAlg.IV.Length + encrypted.Length];
            Buffer.BlockCopy(aesAlg.IV, 0, result, 0, aesAlg.IV.Length);
            Buffer.BlockCopy(encrypted, 0, result, aesAlg.IV.Length, encrypted.Length);

            return result;
        }

        // AES-дешифрование
        private static string DecryptStringFromBytes(byte[] cipherTextCombined, byte[] key)
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

            return Encoding.UTF8.GetString(decrypted);
        }
    }
}