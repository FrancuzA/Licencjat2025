using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{
    public SaveData _saveData;
    private List<ISaveSystemElement> _saveSystemElem = new();

    public bool UseEncr;

    private void Awake()
    {
        Dependencies.Instance.RegisterDependency(this);
    }

    private void Start()
    {
        LoadGame();
    }

    public void LoadGame()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, "SaveData.json");
        if (!File.Exists(fullPath))
        {
           NewGame();
        }
        else
        {
            try
            {
                string dataJson = " ";


                FileStream fs = new FileStream(fullPath, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                dataJson = sr.ReadToEnd();

                sr.Close();
                fs.Close();
                if (UseEncr)
                    dataJson = EncryptDecrypt(dataJson);

                _saveData = JsonUtility.FromJson<SaveData>(dataJson);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading game. {e}");
            }
        }

        foreach (ISaveSystemElement SSE in _saveSystemElem)
        {
            SSE.LoadData(_saveData);
        }
    }

    public void SaveGame()
    {
        foreach (ISaveSystemElement SSE in _saveSystemElem)
        {
            SSE.SaveData(_saveData);
        }

        string fullPath = Path.Combine(Application.persistentDataPath, "SaveData.json");
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string data = JsonUtility.ToJson(_saveData);
            if(UseEncr)
                data = EncryptDecrypt(data);

            FileStream fs = new FileStream(fullPath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(data);

            sw.Close();
            fs.Close();
        }
        catch(Exception e)
        {
            Debug.LogError($"Error saving game. {e}");
        }

        
    }

    public void NewGame()
    {
        _saveData = new SaveData();
    }

    public void RegisterToSaveList(ISaveSystemElement element)
    {
        _saveSystemElem.Add(element);
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        string encryptedKey = "2137";

        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptedKey[i % encryptedKey.Length]); 
        }
        return modifiedData;
    }
}
