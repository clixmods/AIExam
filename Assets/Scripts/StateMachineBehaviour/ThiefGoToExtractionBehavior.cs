using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefGoToExtractionBehavior : ThiefStateMachineBehaviour
{
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override  void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //base.OnStateEnter(animator, stateInfo,layerIndex);
        _thiefSteal.GoToExtractionPoint();
    }
}
