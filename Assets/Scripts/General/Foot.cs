using Unity.VisualScripting;
using UnityEngine;

public class Foot : MonoBehaviour
{
    private float distToGround;
    private AudioManager Audio;
    
    void Start()
    {
        Audio = Dependencies.Instance.GetDependancy<AudioManager>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.5f))
        {
            Audio.GroundType = hit.collider.tag;
          
        }
    }

    private void TakeStep()
    {
        Audio.PlayStep();
    }
}
