using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    [SerializeField] private Transform followTarget; 
    [SerializeField] private float mouseSensitivity = 0.5f;
    [SerializeField] private float minTilt = -45f;
    [SerializeField] private float maxTilt = 75f;

    private float tiltAngle = 0f;

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        tiltAngle -= mouseY * mouseSensitivity;
        tiltAngle = Mathf.Clamp(tiltAngle, minTilt, maxTilt);

        if (followTarget != null)
        {
            Vector3 euler = followTarget.localEulerAngles;
            euler.x = tiltAngle;
            followTarget.localEulerAngles = euler;
        }
    }
}