using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolReplaceDiamondBehavior : PatrolStateMachineBehaviour
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _patrol.ReplaceDiamond();
        _movableAgent.speed = 4;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
     public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {    
        _patrol.DropDiamond();
    }
    
}
