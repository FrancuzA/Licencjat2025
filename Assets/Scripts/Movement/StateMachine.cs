using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private StateStack _stack;
    public State CurrentState { get; private set; }
    private State _PreviousState;
    public void Begin(State state)
    {
        _stack = new StateStack();
        _stack.Push(state);
        CurrentState = state;
        CurrentState.Enter();
    }

    public void SetState(State state)
    {
        if (CurrentState != null) { CurrentState.Exit(); }

        CurrentState = state;
        _stack.Push(state);
        CurrentState.Enter();
    }

    public void Dispose()
    {
        if (_stack.Count() == 0)
            return;

        CurrentState.Exit();
        CurrentState = null;
        _stack.Pop();

        if (_stack.Count() == 0)
            return;

        CurrentState = _stack.Peek();
        CurrentState.Enter();
    }

    private void Update()
    {
        Debug.Log("state machine Update");
        if (CurrentState == null)
            return;

        CurrentState.Update();
    }
}