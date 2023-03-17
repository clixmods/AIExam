using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public Vector3 originalPosition;
    public bool NeedToBeReplaced => Vector2.Distance(transform.position, originalPosition) > 3;
    private void Awake()
    {
        originalPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
