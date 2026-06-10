using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private StateStack _stack;
    public State CurrentState { get; private set; }
    public static StateMachine instance;
    private State _pendingState;

    public float CurrentRotationAngle { get; set; }

    private void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void Begin(State state)
    {
        _stack = new StateStack();
        _stack.Push(state);
        CurrentState = state;
        CurrentState.Enter();
    }

    public void SetState(State state)
    {
        CurrentState?.Exit();
        CurrentState = state;
        _stack.Push(state);
        CurrentState.Enter();
    }

    public void RequestState(State state)
    {
        _pendingState = state;
    }

    public void ReturnToState()
    {
        CurrentState?.Exit();
        CurrentState = _stack.Peek();
        _stack.Push(CurrentState);
        CurrentState.Enter();
    }

    public void Dispose()
    {
        if (_stack.Count() == 0) return;

        CurrentState.Exit();
        CurrentState = null;
        _stack.Pop();

        if (_stack.Count() == 0) return;

        CurrentState = _stack.Peek();
        CurrentState.Enter();
    }

    protected virtual void Update()
    {
        CurrentState?.Update();

        if (_pendingState != null)
        {
            SetState(_pendingState);
            _pendingState = null;
        }
    }

    protected virtual void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }
}