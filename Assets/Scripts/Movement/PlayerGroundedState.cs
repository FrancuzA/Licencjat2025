using UnityEngine;

public class PlayerGroundedState : State
{
    // Movement
    private Vector3 _input;
    private Vector3 _moveDirection;
    private float _baseMovementSpeed;
    private const float SprintMultiplier = 2f;

    // State flags
    private bool _isMoving = false;
    private bool _isRunning = false;

    // Mouse
    private float _mouseSens;

    // Components
    private Rigidbody _rb;
    private Transform _mainBody;
    private Animator _animator;

    // Dependencies
    private StartPlayerMovement _startP;
    private CameraTilt _cameraT;

    public PlayerGroundedState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        var dep = Dependencies.Instance;
        _startP = dep.GetDependancy<StartPlayerMovement>();
        _cameraT = dep.GetDependancy<CameraTilt>();

        _animator = _startP.gameObject.GetComponent<Animator>();
        _animator?.SetInteger("MoveState", 0);

        _baseMovementSpeed = _startP.walkSpeed;
        _mouseSens = _cameraT.mouseSensitivity;

        _rb = _stateMachine.GetComponent<Rigidbody>();
        _mainBody = _rb.transform;

        _stateMachine.CurrentRotationAngle = _rb.rotation.eulerAngles.y; // now _rb is ready

        _moveDirection = Vector3.zero;
        _isMoving = false;
        _isRunning = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void Update()
    {
        if (_cameraT.UILock)
        {
            _animator?.SetInteger("MoveState", 0);
            _isMoving = false;
            _stateMachine.SetState(new PlayerPauseState(_stateMachine));
            return;
        }

        HandleRotation();
        GatherMovementInput();
        HandleJump();
    }

    public override void FixedUpdate()
    {
        ApplyMovement();
    }

    // ─── PRIVATE ─────────────────────────────────────────────────────────────

    private void HandleRotation()
    {
        _mouseSens = _cameraT.mouseSensitivity;
        float mouseX = Input.GetAxis("Mouse X");
        _stateMachine.CurrentRotationAngle += mouseX * _mouseSens * 300f * Time.deltaTime * Time.timeScale;
        _mainBody.rotation = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0);
    }

    private void GatherMovementInput()
    {
        _input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (_input.magnitude > 0.1f)
        {
            _isMoving = true;
            _isRunning = Input.GetKey(KeyCode.LeftShift);

            float speed = _isRunning ? _baseMovementSpeed * SprintMultiplier : _baseMovementSpeed;

            _moveDirection = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0) * _input;
            _moveDirection = _moveDirection.normalized * speed * Time.fixedDeltaTime;

            _animator?.SetInteger("MoveState", _isRunning ? 2 : 1);
        }
        else
        {
            _isMoving = false;
            _isRunning = false;
            _moveDirection = Vector3.zero;
            _animator?.SetInteger("MoveState", 0);
        }
    }

    private void ApplyMovement()
    {
        float yVelocity = _rb.linearVelocity.y; 

        if (_isMoving)
            _rb.linearVelocity = new Vector3(_moveDirection.x, yVelocity, _moveDirection.z);
        else
            _rb.linearVelocity = new Vector3(0f, yVelocity, 0f);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _animator?.SetInteger("MoveState", 0);
            _isMoving = false;
            _stateMachine.SetState(new PlayerJumpState(_stateMachine));
        }
    }
}