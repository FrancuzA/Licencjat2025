using UnityEngine;

public class PlayerPauseState : State
{
    public Dependencies _dependencies;
    private Rigidbody _rb;
    private Transform mainBody;
    public CameraTilt _cameraTilt;

    public PlayerPauseState(StateMachine stateMachine) : base(stateMachine) { }


    public override void Enter()
    {
        _dependencies = Dependencies.Instance;
        _cameraTilt = _dependencies?.GetDependancy<CameraTilt>();
        _rb = _stateMachine.GetComponent<Rigidbody>();
        mainBody = _rb.GetComponent<Transform>();
    }

    public override void Update()
    {
        if (_cameraTilt.UILock == false) 
        {
            _stateMachine.ReturnToState();
        }
        mainBody.rotation = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0);
    }


    public override void Exit() 
    {
    }
}
