
using UnityEngine;

public class StartPlayerMovement : StateMachine,ISaveSystemElement
{
    private Transform playerTransform;
    private SaveSystemManager SSM;
    private void Awake()
    {
        SSM = Dependencies.Instance.GetDependancy<SaveSystemManager>();
       SSM.RegisterToSaveList(this);
       playerTransform = transform;
       Time.timeScale = 1;
    }
    private void Start()
    {
        Begin(new PlayerGroundedState(this));
    }

    /*
    protected override void Update()
    {
        base.Update();
        
    }*/


    public void LoadData(SaveData saveData)
    {
        if (saveData.playerPosition != Vector3.zero)
        {
            playerTransform.SetPositionAndRotation(saveData.playerPosition, Quaternion.identity);

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    public void SaveData(SaveData saveData)
    {
       saveData.playerPosition = playerTransform.position;
    }
}