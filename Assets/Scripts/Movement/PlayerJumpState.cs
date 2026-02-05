using UnityEngine;

public class PlayerJumpState : State
{
    private Rigidbody _rb;
    private Transform mainBody;
    private float _timer;
    private float _mouseSens = 0.5f;
    private float jumpForce = 5f;
    private AudioManager audio;
    

    public PlayerJumpState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        audio = Dependencies.Instance.GetDependancy<AudioManager>();
        audio.JumpPhase = "Jump";
        audio.PlayJump();
        jumpForce = Dependencies.Instance.GetDependancy<StartPlayerMovement>().jumpForce;
        _rb = _stateMachine.GetComponent<Rigidbody>();
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        mainBody = _rb.GetComponent<Transform>();
        _timer = 0f;
    }

    public override void Update()
    {
        _mouseSens = Dependencies.Instance.GetDependancy<CameraTilt>().mouseSensitivity;
        float mouseX = Input.GetAxis("Mouse X");

        // Use the SAME shared rotation from state machine
        _stateMachine.CurrentRotationAngle += mouseX * _mouseSens * 300 * Time.fixedDeltaTime;

        // Apply shared rotation
        mainBody.rotation = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0);

        if (_timer < 0.2f)
        {
            _timer += Time.deltaTime;
            return;
        }

        if (Physics.Raycast(_stateMachine.transform.position, Vector3.down, 1f))
        {
            audio.JumpPhase = "Land";
            audio.PlayJump();
            _stateMachine.SetState(new PlayerGroundedState(_stateMachine));
        }
    }
}