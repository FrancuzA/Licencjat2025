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

    public PlayerGroundedState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        _animator = Dependencies.Instance.GetDependancy<StartPlayerMovement>().gameObject.GetComponent<Animator>();
        
         if(_animator != null) _animator.SetInteger("MoveState", 0);
        _mouseSens = Dependencies.Instance.GetDependancy<CameraTilt>().mouseSensitivity;
        _CurrentmovementSpeed = Dependencies.Instance.GetDependancy<StartPlayerMovement>().walkSpeed;
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
       
        mainBody.rotation = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0);
       
        if (_input.magnitude > 0.1f)
        {
             if(!isRunning && _animator != null) _animator.SetInteger("MoveState", 1);
            Vector3 moveDirection = Quaternion.Euler(0, _stateMachine.CurrentRotationAngle, 0) * _input;
            moveDirection = moveDirection.normalized * _CurrentmovementSpeed * Time.fixedDeltaTime;
            moveDirection.y = _rb.linearVelocity.y;
            _rb.linearVelocity = moveDirection;
        }
        else
        {
            if (_animator != null) _animator.SetInteger("MoveState", 0);
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
            _CurrentmovementSpeed *= 2;
            if (_animator != null) _animator.SetInteger("MoveState", 2);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            _CurrentmovementSpeed /= 2;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (_animator != null) _animator.SetInteger("MoveState", 0);
            _stateMachine.SetState(new PlayerJumpState(_stateMachine));
        }
    }
}