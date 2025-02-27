
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    public GroundDetector gd;
    public CharacterMover cm;
    public Camera cam;
    Animator anim;
    public float rotationScale;
    public Transform gunPivot;
    public Transform gunRightHand;
    public Transform gunLeftHand;

    [Range(0f, 1f)]
    public float lookAtMaxAngle = 0.5f;
    public RaycastLookAt cameraLookAt;
    public float lookAtSpeed = 10;
    Vector3 lookat;

    [Range(0f, 1f)] public float rightHandIKWeight = 1f;
    [Range(0f, 1f)] public float leftHandIKWeight = 1f;
    [Range(0f, 1f)] public float lookIKWeight = 1f;


    private void Start()
    {
        anim = GetComponent<Animator>();
        if(cam == null)
        {
            cam = Camera.main;
        }

        lookat = cameraLookAt.lookingAt;
    }
    void Update()
    {
        anim.SetFloat("Sideways", cm.velocity.x);
        anim.SetFloat("Upwards", cm.velocity.y);
        anim.SetFloat("Forward", cm.velocity.z);
        anim.SetFloat("Rotation", cm.velocityAngular * rotationScale);
        anim.SetBool("Grounded", gd.grounded);

        FixLookat();

        gunPivot.LookAt(lookat);
    }

    private void FixLookat()
    {
        lookat = Vector3.Lerp(lookat, cameraLookAt.lookingAt, lookAtSpeed * Time.deltaTime);

        Vector3 forwardLookAt = (lookat - cameraLookAt.transform.position).normalized;
        float dot = Vector3.Dot(forwardLookAt, transform.forward);
        if (dot < lookAtMaxAngle)
        {
            Vector3 axis = Vector3.Cross(forwardLookAt, transform.forward);
            float angle = Vector3.SignedAngle(forwardLookAt, transform.forward, axis);
            float distance = Vector3.Distance(lookat, cameraLookAt.transform.position);
            float maxAngle = Mathf.Acos(lookAtMaxAngle) * Mathf.Rad2Deg;
            forwardLookAt = Quaternion.AngleAxis(angle > 0 ? -maxAngle : maxAngle, axis) * transform.forward;
            lookat = cameraLookAt.transform.position + forwardLookAt * distance;
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        
        if (anim)
        {
            //Poner manos en el punto del arma correspondiente
            if (gunRightHand != null)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandIKWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandIKWeight);
                anim.SetIKPosition(AvatarIKGoal.RightHand, gunRightHand.position);
                anim.SetIKRotation(AvatarIKGoal.RightHand, gunRightHand.rotation);
            }

            if (gunLeftHand != null)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, gunLeftHand.position);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, gunLeftHand.rotation);
            }
            //Que la cabeza mire donde mira el arma
            if (lookat != null)
            {
                anim.SetLookAtWeight(lookIKWeight);
                anim.SetLookAtPosition(lookat);
            }
        }
    }
}
