using UnityEngine;

public class PlayerJumpState : State
{
    private Rigidbody _rb;
    private Transform _mainBody;
    private float _timer;
    private float _mouseSens = 0.5f;
    private float _jumpForce = 5f;
    private float _heightOfModel = 1f;
    private AudioManager _audio;
    private CameraTilt _cameraT;

    public PlayerJumpState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        var dep = Dependencies.Instance;
        _audio = dep.GetDependancy<AudioManager>();
        _cameraT = dep.GetDependancy<CameraTilt>();
        _jumpForce = dep.GetDependancy<StartPlayerMovement>().jumpForce;

        _rb = _stateMachine.GetComponent<Rigidbody>();
        _mainBody = _rb.transform;

        _mainBody.rotation = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0);

        var player = _stateMachine.gameObject;
        var model = player.GetComponent<MeshFilter>();
        _heightOfModel = model.mesh.bounds.size.y * player.transform.localScale.y;

        if (!Physics.Raycast(_stateMachine.transform.position, Vector3.down, _heightOfModel / 2 + 0.1f)) return;

        _audio.JumpPhase = "Jump";
        _audio.PlayJump();
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _timer = 0f;
    }

    public override void Update()
    {
        if (_cameraT.UILock)
        {
            _stateMachine.RequestState(new PlayerPauseState(_stateMachine));
            return;
        }

        _mouseSens = _cameraT.mouseSensitivity;
        float mouseX = Input.GetAxis("Mouse X");
        _stateMachine.CurrentRotationAngle += mouseX * _mouseSens * 300f * Time.deltaTime;
        _mainBody.rotation = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0);

        if (_timer < 0.2f)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            if (Physics.Raycast(_stateMachine.transform.position, Vector3.down, _heightOfModel / 2 + 0.1f))
            {
                _audio.JumpPhase = "Land";
                _audio.PlayJump();
                _stateMachine.RequestState(new PlayerGroundedState(_stateMachine));
            }
        }
    }
}