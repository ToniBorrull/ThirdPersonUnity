using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericGun : MonoBehaviour
{
    [Header("Weapon")]
    public int clipMax = 30;
    public int clipCurrent = 30;
    public bool automatic = true;
    [Min(1f/60f)]
    public float fireRate = 0.1f;
    public float reloadTime = 0.5f;
    private float maxTime;
    [Header("Firing")]
    public UnityEvent onFire;
    public Transform firePoint;
    public GameObject bullet;
    [Header("Animation")]
    public float positionRecover;
    public float rotationRecover;
    public Vector3 knockbackPosition;
    public Vector3 knockbackRotation;
    Vector3 originalPosition;
    Quaternion originalRotation;
    [Header("Mira")]
    public Transform mira;
    RaycastHit hit;
    public float distance;
    public LayerMask layerMask;

    private float lastShot;
    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        maxTime = reloadTime;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, positionRecover * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, rotationRecover * Time.deltaTime);

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
            mira.transform.position = Camera.main.WorldToScreenPoint(hit.point);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * distance, Color.red);
        }
        //Recargar
        if (clipCurrent <= 0)
        {
            if (reloadTime <= 0)
            {
                clipCurrent = clipMax;
                reloadTime = maxTime;
            }
            else if (reloadTime > 0)
            {
                reloadTime -= Time.deltaTime;
            }
        }
        if(reloadTime < 0)
        {
            reloadTime = 0;
        }

        //Disparar
        if (automatic)
        {
            if (Input.GetButton("Fire1") && clipCurrent > 0 && Time.time > lastShot + fireRate)
            {
                Fire();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && clipCurrent > 0)
            {
                Fire();
            }
        }
    }
    public void Fire()
    {
        //Al disparar, poner la bala en la punta del arma
         bullet = ObjectPool.SharedInstance.GetPooledObject();
        if(bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.SetActive(true);
        }
        onFire.Invoke();
        clipCurrent--;
        lastShot = Time.time;
        StartCoroutine(Knockback_Corutine());
    }
    IEnumerator Knockback_Corutine()
    {
        yield return null;
        transform.localPosition -= new Vector3(Random.Range(-knockbackPosition.x, knockbackPosition.x), Random.Range(0, knockbackPosition.y), Random.Range(-knockbackPosition.z, -knockbackPosition.z * .5f));
        transform.localEulerAngles -= new Vector3(Random.Range(knockbackRotation.x * 0.5f, knockbackRotation.x), Random.Range(-knockbackRotation.y, knockbackRotation.y), Random.Range(-knockbackRotation.z, knockbackRotation.z));
    }
}
