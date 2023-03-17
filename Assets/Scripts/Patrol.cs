using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    private MovableAgent _movableAgent;
    private Diamond _diamond;
    private Animator _animator;
    private Transform _stealer;
    [SerializeField] private bool isAlerted;
    [SerializeField] private Transform lecoin;
    [SerializeField] private Transform atterisage;
    
    private static readonly int DistanceWithStealerID = Animator.StringToHash("DistanceWithStealer");
    private static readonly int DiamondNeedToBeReplacedID = Animator.StringToHash("DiamondNeedToBeReplaced");
    private static readonly int IsMovingID = Animator.StringToHash("IsMoving");

    public bool IsAlerted
    {
        get => isAlerted;
        set => isAlerted = value;
    }

    private void Awake()
    {
        _stealer = FindObjectOfType<ThiefSteal>().transform;
        _diamond = FindObjectOfType<Diamond>();
        _animator = GetComponent<Animator>();
        _movableAgent = GetComponent<MovableAgent>();
    }

    private void Update()
    {
        var distanceWithStealer = Vector2.Distance(transform.position, _stealer.position);
        _animator.SetFloat(DistanceWithStealerID , distanceWithStealer);
        _animator.SetBool(DiamondNeedToBeReplacedID, _diamond.NeedToBeReplaced);
        _animator.SetBool(IsMovingID, _movableAgent.IsMoving);
    }

    public void GrabStealerAndPiedAuQ()
    {
        _stealer.transform.SetParent(transform);
        _stealer.GetComponent<Animator>().enabled = false;
        _stealer.GetComponent<MovableAgent>().enabled = false;
        _stealer.GetComponent<ThiefSteal>().enabled = false;
        _movableAgent.SetDestination(lecoin.position);
        if (Vector2.Distance(transform.position, lecoin.position) < 3) 
        {
            _stealer.position = atterisage.position;
            _stealer.transform.SetParent(null);
            _stealer.GetComponent<Animator>().enabled = true;
            _stealer.GetComponent<MovableAgent>().enabled = true;
            _stealer.GetComponent<ThiefSteal>().enabled = true;
        }
    }

    public void ChaseStealer()
    {
        _movableAgent.SetDestination(_stealer.position, true);
        isAlerted = true;
    }
    
    public void ReplaceDiamond()
    {
        if (Vector2.Distance(transform.position, _diamond.transform.position) < 3)
        {
            _diamond.transform.SetParent(transform);
            _movableAgent.SetDestination(_diamond.originalPosition);
        }
        else
        {
            if (_diamond.transform.parent == transform)
            {
                _diamond.transform.SetParent(null);
            }
            _movableAgent.SetDestination(_diamond.transform.position);
        }
        
    }

    public void DropDiamond()
    {
        if (_diamond.transform.parent == transform)
        {
            _diamond.transform.SetParent(null);
        }
    }
}
