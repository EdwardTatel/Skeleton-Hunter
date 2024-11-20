using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset; 

    private void Start()
    {
    }
    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;

            transform.position = targetPosition;

            transform.LookAt(target.position);
        }
    }
}
