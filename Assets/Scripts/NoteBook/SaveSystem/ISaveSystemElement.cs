using UnityEngine;

public interface ISaveSystemElement 
{
    void LoadData(SaveData saveData);
   void SaveData(SaveData saveData);
}
