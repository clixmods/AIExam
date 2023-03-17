using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class AgentPattern : MonoBehaviour
{
    private MovableAgent _movableAgent;
    public Transform[] patternPoints;
    public int indexPattern = 0;

    private void Awake()
    {
        _movableAgent = GetComponent<MovableAgent>();
    }

    public void Round(bool force = false)
    {
        if (!_movableAgent.IsMoving || force)
        {
            if (indexPattern >= patternPoints.Length)
            {
                indexPattern = 0;
                Array.Reverse(patternPoints);
                //patternPoints = patternPoints.Reverse();
            }
            _movableAgent.SetDestination(patternPoints[indexPattern].position);
            indexPattern++;
        }
    }
    
    
    
}
