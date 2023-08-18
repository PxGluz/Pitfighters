using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private float smoothTime, offsetSize;

    private Vector3 finalDestination, current, mouseDiff;

    private void CameraFollow()
    {
        mouseDiff = new Vector3(Input.mousePosition.x - Screen.width / 2f, Input.mousePosition.y - Screen.height / 2f, 0f).normalized;
        finalDestination = destination.position + transform.TransformDirection(mouseDiff) * offsetSize;
        transform.position = Vector3.SmoothDamp(transform.position, finalDestination, ref current, smoothTime);
    }
    
    void Update()
    {
        CameraFollow();
    }
}
