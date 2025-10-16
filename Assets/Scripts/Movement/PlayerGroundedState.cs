using UnityEngine;

public class PlayerGroundedState : State
{
    private Vector3 _input;
    private float _rotationInput;
    private float _CurrentmovementSpeed = 1500f;
    private float _mouseSens = 3.0f; 
    private Rigidbody _rb;

    public PlayerGroundedState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        _rb = _stateMachine.GetComponent<Rigidbody>();
    }
    public override void Update()
    {
        _input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _rotationInput = Input.GetAxis("Mouse X");

        Vector3 moveDirection = _stateMachine.transform.TransformDirection(_input);
        moveDirection.y = 0;

        _rb.linearVelocity = moveDirection * _CurrentmovementSpeed * Time.deltaTime;

        if (Mathf.Abs(_rotationInput) > 0.01f)
        {
            _rb.angularVelocity = new Vector3(0, _rotationInput * _mouseSens, 0);
        }
        else
        {
            _rb.angularVelocity = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.SetState(new PlayerJumpState(_stateMachine));
        }
    }
}