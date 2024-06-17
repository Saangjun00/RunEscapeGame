using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acceler : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private Rigidbody rigidbody3D;

    private void Awake()
    {
        rigidbody3D = GetComponent<Rigidbody>();
    }

    public void MoveTo(Vector3 direction, float force=0)
    {
        Vector3 moveForce = Vector3.zero;

        if(force == 0)
        {
            direction.y = 0;
            moveForce = direction.normalized * moveSpeed;
        }
        else
        {
            moveForce = direction * force;
        }
        rigidbody3D.AddForce(moveForce);
    }
   
}
