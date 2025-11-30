using UnityEngine;
using UnityEngine.UIElements;

public class CameraTilt : MonoBehaviour
{
    [SerializeField] private Transform followTarget; 
    [SerializeField] public float mouseSensitivity = 0.5f;
    [SerializeField] private float sensitivityMin = 0.1f;
    [SerializeField] private float sensitivityMax = 1f;
    [SerializeField] private float minTilt = -45f;
    [SerializeField] private float maxTilt = 75f;
    [SerializeField] private bool InMenu = false;

    private float tiltAngle = 0f;

    public void Start()
    {
        Dependencies.Instance.RegisterDependency<CameraTilt>(this);
    }

    public void ChangeSens(float value)
    {
        mouseSensitivity = Mathf.Lerp(sensitivityMin, sensitivityMax, value) ;
    }
    void Update()
    {

        float mouseY = Input.GetAxis("Mouse Y");
        tiltAngle -= mouseY * mouseSensitivity * 300  * Time.fixedDeltaTime;
        tiltAngle = Mathf.Clamp(tiltAngle, minTilt, maxTilt);
        if (followTarget != null && !InMenu)
        {
            Vector3 euler = followTarget.localEulerAngles;
            euler.x = tiltAngle;
            followTarget.localEulerAngles = euler;
        }

        if (Input.GetKeyDown(KeyCode.N) && !Dependencies.Instance.GetDependancy<NoteBookManager>().isWriting)
        {
            InMenu = !InMenu;
        }
    }
}