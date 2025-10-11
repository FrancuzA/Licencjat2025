using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class State
{
    public string name;
    protected StateMachine _stateMachine;

    public State(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        name = GetType().Name;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}