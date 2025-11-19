using UnityEngine;

public class PlayerGroundedState : State
{
    private Vector3 _input;
    private float _CurrentmovementSpeed = 300f;
    public float _mouseSens = 0.5f;
    private Rigidbody _rb;
    private Transform mainBody;

    public PlayerGroundedState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        _mouseSens = Dependencies.Instance.GetDependancy<CameraTilt>().mouseSensitivity;
        _rb = _stateMachine.GetComponent<Rigidbody>();
        mainBody = _rb.GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void Update()
    {
        _mouseSens = Dependencies.Instance.GetDependancy<CameraTilt>().mouseSensitivity;
        _input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        float mouseX = Input.GetAxis("Mouse X");
        // Use shared rotation from state machine
        _stateMachine.CurrentRotationAngle += mouseX * _mouseSens * 300 * Time.fixedDeltaTime;

        Vector3 moveDirection = _stateMachine.transform.TransformDirection(_input);
        moveDirection.y = 0;

        _rb.linearVelocity = moveDirection * _CurrentmovementSpeed * Time.fixedDeltaTime;

        // Apply shared rotation
        mainBody.rotation = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0);

        if (Input.GetButtonDown("Jump"))
        {
            _stateMachine.SetState(new PlayerJumpState(_stateMachine));
        }
    }
}