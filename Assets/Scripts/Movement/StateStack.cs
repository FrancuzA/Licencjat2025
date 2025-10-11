using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class StateStack
{
    [SerializeField] private List<State> _Stack = new();

    public void Push(State state)
    {
        _Stack.Add(state);
    }

    public State Pop()
    {
        State lastState = Peek();
        _Stack.RemoveAt(_Stack.Count - 1);
        return lastState;
    }

    public State Peek()
    {
        if (_Stack.Count == 0)
        {
            return null;
        }

        return _Stack[^1];
    }

    public int Count()
    {
        return _Stack.Count;
    }

    public List<State> GetStack()
    {
        return _Stack;
    }
}