using UnityEngine;

public class PlayerGroundedState : State
{
    private Vector3 _input;
    private float _CurrentmovementSpeed = 300f;
    public float _mouseSens = 0.5f;
    private Rigidbody _rb;
    private Transform mainBody;
    private Animator _animator;
    private bool isRunning = false;
    private bool isMoving = false;
    private Dependencies _dep;
    private StartPlayerMovement _startP;
    private CameraTilt _cameraT;

    public PlayerGroundedState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        _dep = Dependencies.Instance;
        _startP = _dep.GetDependancy<StartPlayerMovement>();
        _cameraT = _dep.GetDependancy<CameraTilt>();

        _animator = _startP.gameObject.GetComponent<Animator>();
        
         if(_animator != null) _animator.SetInteger("MoveState", 0);
        _mouseSens = _cameraT.mouseSensitivity;
        _CurrentmovementSpeed = _startP.walkSpeed;
        _rb = _stateMachine.GetComponent<Rigidbody>();
        mainBody = _rb.GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public override void Update()
    {
        if(_cameraT.UILock == true)
        {
            if (_animator != null) _animator.SetInteger("MoveState", 0);
            isMoving = false;
            _stateMachine.SetState(new PlayerPauseState(_stateMachine));
        }
        _mouseSens = _cameraT.mouseSensitivity;
        _input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        float mouseX = Input.GetAxis("Mouse X");
        _stateMachine.CurrentRotationAngle += mouseX * _mouseSens * 300 * Time.fixedDeltaTime * Time.timeScale;
       
        mainBody.rotation = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0);
       
        if (_input.magnitude > 0.1f)
        {
            _animator?.SetInteger("MoveState", 1);
            isMoving = true;
            Vector3 moveDirection = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0) * _input;
            moveDirection = moveDirection.normalized * _CurrentmovementSpeed * Time.fixedDeltaTime;
            moveDirection.y = _rb.linearVelocity.y;
            _rb.linearVelocity = moveDirection;
        }
        else
        {
            if (isMoving) StopedMoving();
            _animator?.SetInteger("MoveState", 0);
            isMoving = false;
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && isMoving)
        {
            isRunning = true;
            _CurrentmovementSpeed *= 2;
             _animator?.SetInteger("MoveState", 2);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            _CurrentmovementSpeed /= 2;
        }

        if (Input.GetButtonDown("Jump"))
        {
            _animator?.SetInteger("MoveState", 0);
            isMoving = false;   
            _stateMachine.SetState(new PlayerJumpState(_stateMachine));
        }
    }

    private void StopedMoving()
    {
        _rb.linearVelocity = new Vector3(0, 0, 0);
    }
}