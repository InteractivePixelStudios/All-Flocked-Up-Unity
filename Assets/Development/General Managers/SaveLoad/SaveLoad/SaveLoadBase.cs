using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;


public static class SaveLoadBase
{
    private static string basePath = "";
    private static string encryptionKey = "ChangeThisKeyToSomethingSecure123!"; 
    public static string currentVersion = "1.0.0";



    public static void Save(SaveData data, int slot = 0, bool encrypt = false)
    {
        basePath = Application.persistentDataPath + "/Saves/";
        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        string path = GetSlotPath(slot);
        string json = JsonUtility.ToJson(data, true);

        if (encrypt)
            json = Encrypt(json);

        File.WriteAllText(path, json);
    }

    public static SaveData Load(int slot = 0, bool encrypted = false)
    {
        string path = GetSlotPath(slot);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"No save file found in slot {slot}");
            return null;
        }

        string json = File.ReadAllText(path);
        if (encrypted)
            json = Decrypt(json);

        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // Version check
        if (data.version != currentVersion)
        {
            Debug.Log($"Migrating save from {data.version} to {currentVersion}");
            data.version = currentVersion;
            data.lastSaved = DateTime.Now;
            Save(data, slot, encrypted);
        }

        return data;
    }

    private static string GetSlotPath(int slot)
    {
        return Path.Combine(basePath, $"SaveSlot_{slot}.dat");
    }

    public static bool SaveExists(int slot)
    {
        return File.Exists(GetSlotPath(slot));
    }

    public static void DeleteSave(int slot)
    {
        string path = GetSlotPath(slot);
        if (File.Exists(path))
            File.Delete(path);
    }

    public static string[] GetAllSaves()
    {
        basePath = Application.persistentDataPath + "/Saves/";
        if (!Directory.Exists(basePath)) return Array.Empty<string>();
        return Directory.GetFiles(basePath, "SaveSlot_*.dat");
    }

    private static string Encrypt(string plainText)
    {
        byte[] key = Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 32));
        byte[] iv = new byte[16];
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    private static string Decrypt(string cipherText)
    {
        byte[] key = Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 32));
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (MemoryStream ms = new MemoryStream(buffer))
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }

    public static string DecryptWrapper(string text)
    {
        try
        {
            return Decrypt(text);
        }
        catch
        {
            return text;
        }
    }
}




