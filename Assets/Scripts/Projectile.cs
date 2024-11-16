using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float disappearTime = 5f;
    public Vector3 forceMin = new Vector3(-1, -1, 50);
    public Vector3 forceMax = new Vector3(1, 1, 100);
    public LayerMask layers;
    public float collisionForceMultiplier = 2f;
    public float radius = .1f;
    public GameObject bulletHole;
    [HideInInspector]
    public Rigidbody rb;

    Transform parentObject;

    public Vector3 lastPos;
    private void OnEnable()
    { 
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddRelativeForce(new Vector3(Random.Range(forceMin.x, forceMax.x), Random.Range(forceMin.y, forceMax.y), Random.Range(forceMin.z, forceMax.z)));
        lastPos = transform.position;
        parentObject = transform.parent;
        transform.parent = null;
        lastPos = transform.position;
        
        StartCoroutine(BulletDisable());
       
    }
    IEnumerator BulletDisable()
    {
        yield return new WaitForSeconds(disappearTime);
        transform.parent = parentObject;
        transform.localPosition = Vector3.zero;
        transform.parent.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (this.gameObject.activeInHierarchy)
        {
            Vector3 dir = transform.position - lastPos;

            Debug.DrawRay(lastPos, dir, Color.blue, disappearTime);

            RaycastHit hit;

            if (Physics.SphereCast(lastPos, radius, dir, out hit, dir.magnitude, layers))
            {
                Hitted(hit);
            }

            lastPos = transform.position;
        }
    }

    void Hitted(RaycastHit hit)
    {
        GameObject bullet = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
        bullet.transform.SetParent(hit.transform);

        if (hit.rigidbody)
        {
            hit.rigidbody.AddForceAtPosition(rb.velocity * rb.mass * collisionForceMultiplier, this.transform.position);
        }
        
        
        //Destroy(gameObject);

    }
}
