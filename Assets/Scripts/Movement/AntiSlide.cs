using UnityEngine;

public class AntiSlide : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check for movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool hasInput = Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f;

        // Freeze XZ position when no input, unfreeze when input
        if (hasInput)
        {
            // Allow movement
            rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
            rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            // Freeze XZ position
            rb.constraints |= RigidbodyConstraints.FreezePositionX;
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;

            // Also stop any existing velocity
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }


}