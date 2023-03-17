using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoundBehavior : PatrolStateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override  void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator,stateInfo,layerIndex);
        _patrol.IsAlerted = false;
        _agentPattern.Round(true);
        _movableAgent.speed = 2;
    }
}
