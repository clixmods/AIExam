using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThiefSteal : MonoBehaviour
{
    private MovableAgent _movableAgent;
    [SerializeField] private GameObject objectToSteal;
    public bool HasObject;
    private Animator _animator;
    public Transform ExtractionTransform;
    private Vector3 _extractionPoint => ExtractionTransform.position;
    private static readonly int HasObjectID = Animator.StringToHash("HasObject");
    private Patrol _patrol;
    private static readonly int RunForestID = Animator.StringToHash("RunForest");
    private float _baseSpeed ;
  
    private void Awake()
    {
        _movableAgent = GetComponent<MovableAgent>();
        _animator = GetComponent<Animator>();
        //_extractionPoint = transform.position;
        _patrol = FindObjectOfType<Patrol>();
        _baseSpeed = _movableAgent.speed ;
    }

    // Start is called before the first frame update
    public void GoStealObject()
    {
        _movableAgent.SetDestination(objectToSteal.transform.position);
    }

    public void GoToExtractionPoint()
    {
        _movableAgent.SetDestination(_extractionPoint);
    }

    public void RunForest()
    {
        _movableAgent.SetDestination(_extractionPoint);
    }
    Vector3 RandomPointOnCircleEdge(float radius)
    {
        var vector2 = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vector2.x, vector2.y, 0);
    }

   
    // Update is called once per frame
    void Update()
    {
        
        if ( Vector2.Distance(_patrol.transform.position, transform.position) > 10 
             || !_patrol.GetComponent<MovableAgent>().IsMoving || _patrol.IsAlerted)
        {
            _movableAgent.speed = _baseSpeed  ;
        }
        else
        {
            _movableAgent.speed = _baseSpeed /2f;
        }
            
        if (Vector2.Distance(objectToSteal.transform.position, transform.position) < 1)
        {
            objectToSteal.transform.SetParent(transform);
            HasObject = true;
        }
        _animator.SetBool(HasObjectID , HasObject);
        _animator.SetBool(RunForestID,_patrol.IsAlerted);
    }

    private void OnDisable()
    {
        if (HasObject)
        {
            HasObject = false;
            objectToSteal.transform.SetParent(null);
        }
        


    }
}
