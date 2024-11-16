using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRaycast : MonoBehaviour
{

    public float distance;
    public GameObject point;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Raycast();
    }
    void Raycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
            point.transform.position = hit.point;
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * distance, Color.red);
            Debug.Log("Not Hit");
        }
    }
}
