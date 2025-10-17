using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    [SerializeField] private Transform followTarget; 
    [SerializeField] private float mouseSensitivity = 0.5f;
    [SerializeField] private float minTilt = -45f;
    [SerializeField] private float maxTilt = 75f;
    [SerializeField] private bool InMenu = false;

    private float tiltAngle = 0f;

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        tiltAngle -= mouseY * mouseSensitivity * Time.fixedDeltaTime;
        tiltAngle = Mathf.Clamp(tiltAngle, minTilt, maxTilt);

        if (followTarget != null && !InMenu)
        {
            Vector3 euler = followTarget.localEulerAngles;
            euler.x = tiltAngle;
            followTarget.localEulerAngles = euler;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            InMenu = !InMenu;
        }
    }
}