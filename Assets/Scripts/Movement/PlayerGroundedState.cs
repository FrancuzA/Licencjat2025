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
        _stateMachine.CurrentRotationAngle += mouseX * _mouseSens * 300 * Time.fixedDeltaTime * Time.timeScale;

        // FIX: Use mainBody's rotation to calculate movement direction
        Vector3 moveDirection = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0) * _input;
        moveDirection = moveDirection.normalized * _CurrentmovementSpeed * Time.fixedDeltaTime;
        moveDirection.y = _rb.linearVelocity.y;
        _rb.linearVelocity = moveDirection;

        // Apply rotation to the same transform we use for movement
        mainBody.rotation = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0);

        if (Input.GetButtonDown("Jump"))
        {
            _stateMachine.SetState(new PlayerJumpState(_stateMachine));
        }
    }
}