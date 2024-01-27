using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quickrotate : MonoBehaviour
{
    public Transform position;
    // Start is called before the first frame update
    void Start()
    {
        transform.right = position.position - transform.position;
    }

    private void Update()
    {
     
    }
}
