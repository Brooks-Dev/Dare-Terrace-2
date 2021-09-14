using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("MovableBox") && Vector3.Distance(transform.position, other.transform.position) < 0.05f)
        {
            Rigidbody body = other.GetComponent<Rigidbody>();
            if( body != null)
            {
                body.isKinematic = true;
            }
            Material material = GameObject.Find("Display").GetComponent<MeshRenderer>().material;
            if (material != null)
            {
                material.color = Color.blue;
            }
            other.transform.position = transform.position;
            Destroy(this);
        }
    }
}
