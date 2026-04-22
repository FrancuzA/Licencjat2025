using UnityEngine;

public class PlayerJumpState : State
{
    private Rigidbody _rb;
    private Transform mainBody;
    private float _timer;
    private float _mouseSens = 0.5f;
    private float jumpForce = 5f;
    private float HeightOfModel = 1;
    private AudioManager audio;
    private Dependencies _dep;
    private CameraTilt _cameraT;
    

    public PlayerJumpState(StateMachine stateMachine) : base(stateMachine) { }


    public override void Enter()
    {
        _dep = Dependencies.Instance;
        audio = _dep.GetDependancy<AudioManager>();
        _cameraT = _dep.GetDependancy<CameraTilt>();
        jumpForce = _dep.GetDependancy<StartPlayerMovement>().jumpForce;
        _rb = _stateMachine.GetComponent<Rigidbody>();
        mainBody = _rb.GetComponent<Transform>();
        var Player = _stateMachine.gameObject;
        var Model = Player.GetComponent<MeshFilter>();
        HeightOfModel = Model.mesh.bounds.size.y *Player.transform.localScale.y;
        if (!Physics.Raycast(_stateMachine.transform.position, Vector3.down, HeightOfModel / 2 + 0.1f)) return;
        audio.JumpPhase = "Jump";
        audio.PlayJump();
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _timer = 0f;
    }

    public override void Update()
    {
        if(_cameraT.UILock == true) _stateMachine.SetState(new PlayerPauseState(_stateMachine));
        _mouseSens = _cameraT.mouseSensitivity;
        float mouseX = Input.GetAxis("Mouse X");

      
        _stateMachine.CurrentRotationAngle += mouseX * _mouseSens * 300 * Time.fixedDeltaTime;

        mainBody.rotation = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0);

        if (_timer < 0.2f)
        {
            _timer += Time.deltaTime;
            return;
        }

        if (Physics.Raycast(_stateMachine.transform.position, Vector3.down, HeightOfModel/2 + 0.1f))
        {
            audio.JumpPhase = "Land";
            audio.PlayJump();
            _stateMachine.SetState(new PlayerGroundedState(_stateMachine));
        }
    }
}