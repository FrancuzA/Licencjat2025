
using UnityEngine;

public class StartPlayerMovement : StateMachine
{
    private void Start()
    {
        Begin(new PlayerGroundedState(this));
    }


}