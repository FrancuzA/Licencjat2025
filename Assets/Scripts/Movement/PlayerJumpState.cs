using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : State
{
    private Rigidbody _rb;
    private float _timer;
    public PlayerJumpState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        _rb = _stateMachine.GetComponent<Rigidbody>();
        _rb.AddForce(Vector3.up * 30f, ForceMode.Impulse);
    }

    public override void Update()
    {
        if (_timer < 0.2f)
        {
            _timer += Time.deltaTime;

            return;
        }

        if (Physics.Raycast(_stateMachine.transform.position, Vector3.down, 1f))
        {
            _stateMachine.Begin(new PlayerGroundedState(_stateMachine));
        }
    }
}