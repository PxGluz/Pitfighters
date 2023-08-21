using UnityEngine;

public class RotateToPoint : MonoBehaviour
{
    [HideInInspector] public float angle;
    [SerializeField] public static float originalRotationSpeed = 1;
    [HideInInspector] public float rotationSpeed;

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, angle, 0), rotationSpeed);
    }
}
