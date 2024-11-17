using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public LayerMask layerMask;
    public float speed = 10f;
    public float distance;
    public float cameraRadius;
    Vector3 dir1;
    private float vertical;
    private float horizontal;
    private float camOffset = 0.6f;
    public float lerpSpeed;

    public GameObject camara;
    public GameObject pivot;

    void Start()
    {
        Debug.Log(distance);
        Cursor.lockState = CursorLockMode.Locked;
    }



    void LateUpdate()
    {
        transform.position = pivot.transform.position;

        vertical -= Input.GetAxis("Mouse Y");
        horizontal += Input.GetAxis("Mouse X");

        //Aplicarle un offset para que se vea a la derecha
        Vector3 rightPos = this.transform.position + transform.right * camOffset;
        camara.transform.LookAt(rightPos);
        
        //Limitar el angulo que la camara puede tener, solo en vertical
        float verticalClamp = Mathf.Clamp(vertical, -90f, 90f);

        transform.rotation = Quaternion.Euler(verticalClamp, horizontal, 0);   
        dir1 = (camara.transform.position - this.transform.position).normalized;

        //Raycast para mirar si la camara está chocando con algo
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, cameraRadius, dir1, out hit, distance, layerMask))
        {
            Debug.DrawRay(transform.position, dir1 * hit.distance, Color.yellow);
            camara.transform.localPosition = Vector3.back * hit.distance;
        }
        else
        {
            Debug.DrawRay(transform.position, dir1 * 1000, Color.white);
            Vector3 targetPos = transform.position - transform.forward * distance;
            camara.transform.position = Vector3.Lerp(camara.transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }
    }
}
