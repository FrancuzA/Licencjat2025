using UnityEngine;

public class PlayerPauseState : State
{
    private Rigidbody _rb;
    private CameraTilt _cameraTilt;

    public PlayerPauseState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        var dep = Dependencies.Instance;
        _cameraTilt = dep.GetDependancy<CameraTilt>();
        _rb = _stateMachine.GetComponent<Rigidbody>();

        _stateMachine.CurrentRotationAngle = _rb.rotation.eulerAngles.y;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    public override void Update()
    {
        if (!_cameraTilt.UILock)
            _stateMachine.ReturnToState();

        _rb.linearVelocity = Vector3.zero;
    }

    public override void Exit() { }
}