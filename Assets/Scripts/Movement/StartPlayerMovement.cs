
using UnityEngine;

public class StartPlayerMovement : StateMachine,ISaveSystemElement
{
    private Transform playerTransform;
    private SaveData _saveData;
    private void Awake()
    {
       // Dependencies.Instance.GetDependancy<SaveSystemManager>().RegisterToSaveList(this);
    }
    private void Start()
    {
        Begin(new PlayerGroundedState(this));
    }

    public void LoadData(SaveData saveData)
    {
        if(saveData.playerPosition != Vector3.zero)
            playerTransform.position =  saveData.playerPosition;
    }

    public void SaveData(SaveData saveData)
    {
       saveData.playerPosition = playerTransform.position;
    }
}