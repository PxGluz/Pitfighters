using UnityEngine;

public class RotateToPoint : MonoBehaviour
{
    [HideInInspector] public float angle;
    public float rotationSpeed;

    void FixedUpdate()
    {
        float finalRotation = rotationSpeed / (Player.m.weaponManager.blocking ? 20f : 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, angle, 0), finalRotation);
    }
}
