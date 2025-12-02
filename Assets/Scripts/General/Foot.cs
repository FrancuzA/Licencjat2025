using UnityEngine;

public class Foot : MonoBehaviour
{
    private float distToGround;
    private AudioManager Audio;
    private bool onGround;
    private float _timer;
    private Rigidbody _rb;
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        
        Audio = Dependencies.Instance.GetDependancy<AudioManager>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.5f))
        {
            Audio.GroundType = hit.collider.tag;
            onGround = true;
        }
        else onGround = false;

       
    }

    private void FixedUpdate()
    {
        if (_timer < 0.6f)
        {
            _timer += Time.deltaTime;
            return;
        }

        if (onGround && _rb.linearVelocity.magnitude >0.1) Audio.PlayStep();

        _timer = 0f;
    }

}
