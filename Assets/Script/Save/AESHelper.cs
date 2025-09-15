using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class AESHelper
{
    private static readonly string Key = "vX3p$8Lq@M1z#4rD";
    private static readonly string IV = "1a2b3c4d5e6f7g8h";

    public static string Encrypt(string plainText)
    {
        if (plainText.Equals(string.Empty))
        {
            return string.Empty;
        }

        byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
        byte[] ivBytes = Encoding.UTF8.GetBytes(IV);

        using Aes aes = Aes.Create();
        aes.Key = keyBytes;
        aes.IV = ivBytes;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using MemoryStream ms = new();
        using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
        using (StreamWriter sw = new(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string cipherText)
    {
        if (cipherText.Equals(string.Empty))
        {
            return string.Empty;
        }

        byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
        byte[] ivBytes = Encoding.UTF8.GetBytes(IV);
        byte[] buffer = Convert.FromBase64String(cipherText);

        using Aes aes = Aes.Create();
        aes.Key = keyBytes;
        aes.IV = ivBytes;

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using MemoryStream ms = new(buffer);
        using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
        using StreamReader sr = new(cs);

        return sr.ReadToEnd();
    }
}
