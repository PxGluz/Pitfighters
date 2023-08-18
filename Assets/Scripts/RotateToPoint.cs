using UnityEngine;

public class RotateToPoint : MonoBehaviour
{
    [HideInInspector] public float angle;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
