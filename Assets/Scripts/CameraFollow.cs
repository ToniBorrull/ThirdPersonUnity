using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public LayerMask layerMask;
    public float speed = 10f;
    public float distance;
    public float cameraRadius;
    Vector3 dir1;
    private float vertical;
    private float horizontal;

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

        vertical -= Input.GetAxisRaw("Mouse Y");//Input.GetAxis("Vertical") * Time.deltaTime * speed;
        horizontal += Input.GetAxisRaw("Mouse X");//Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        camara.transform.LookAt(this.transform.position + transform.right * 0.5f);
        vertical = Mathf.Clamp(vertical, -90f, 90f);

        Vector3 dir = new Vector3(-vertical, horizontal, 0) * speed;
        transform.rotation = Quaternion.Euler(vertical, horizontal, 0);
        Vector3 pos = camara.transform.position;
        dir1 = (camara.transform.position - this.transform.position).normalized;


        RaycastHit hit;
        if (Physics.SphereCast(transform.position, cameraRadius, dir1, out hit, distance, layerMask))
        {
            Debug.DrawRay(transform.position, dir1 * hit.distance, Color.yellow);
            camara.transform.localPosition = Vector3.back * hit.distance;
            Debug.Log(hit.collider.name);
        }
        else
        {
            Debug.DrawRay(transform.position, dir1 * 1000, Color.white);
            Vector3 targetPos = transform.position - transform.forward * distance;
            camara.transform.position = Vector3.Lerp(camara.transform.position, targetPos, lerpSpeed * Time.deltaTime);
            //Debug.Log("Did not Hit");
        }
    }
}
