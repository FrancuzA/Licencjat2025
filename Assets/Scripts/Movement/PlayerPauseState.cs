using UnityEngine;

public class PlayerPauseState : State
{
    public Dependencies _dependencies;
    public CameraTilt _cameraTilt;
    public PlayerPauseState(StateMachine stateMachine) : base(stateMachine) { }


    public override void Enter()
    {
        _dependencies = Dependencies.Instance;
        _cameraTilt = _dependencies?.GetDependancy<CameraTilt>();
    }

    public override void Update()
    {
        if (_cameraTilt.UILock == false) 
        {
            _stateMachine.ReturnToState();
        }
    }


    public override void Exit() 
    {
    }
}
