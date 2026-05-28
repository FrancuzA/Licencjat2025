using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Firstnpc : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float arrivalDistance = 1f;
    [SerializeField] private float waitForPlayerDistance = 4f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Checkpoints")]
    public GameObject eventIndicator;
    public List<Transform> checkPoints = new List<Transform>();
    public List<Transform> runPoints = new List<Transform>();

    private Animator npcAnim;
    private Rigidbody rb;
    private Transform player;
    private DictionaryManager _Distionary;
    private bool inDialogue;
    private bool hasTranslated = false;
    private WaitForSecondsRealtime waveCycle = new WaitForSecondsRealtime(5);

    void Start()
    {
        _Distionary = DictionaryManager.Instance;
        npcAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
    }

    private void FixedUpdate()
    {
        hasTranslated = _Distionary.hasTranslated;
    }

    private IEnumerator Wave()
    {
        do
        {
            npcAnim.SetTrigger("Greet");
            yield return waveCycle;
        }
        while (!inDialogue);
    }

    public void StopWaveing()
    {
        inDialogue = true;
        npcAnim.SetTrigger("Idle");
    }

    public void AssignPlayer()
    {
        player = Dependencies.Instance.GetDependancy<StartPlayerMovement>().gameObject.transform;
    }

    public void StartGoing()
    {
        StartCoroutine(GoToCheckpoints());
    }

    public void StartGoingHome()
    {
        StartCoroutine(RunToHome());
    }
    public void CloseEventIndicator()
    {
        eventIndicator.SetActive(false); 
    }

    private IEnumerator GoToCheckpoints()
    {
        npcAnim.SetTrigger("Greet");
        yield return new WaitUntil(()=> hasTranslated == true);
        AssignPlayer();
        npcAnim.SetTrigger("Walk");

        foreach (Transform checkpoint in checkPoints)
        {
            while (Vector3.Distance(transform.position, checkpoint.position) > arrivalDistance)
            {
                Vector3 direction = (checkpoint.position - transform.position).normalized;

                Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);
                if (flatDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
                    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
                }

                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

                yield return new WaitForFixedUpdate();
            }

            if (player != null && Vector3.Distance(transform.position, player.position) > waitForPlayerDistance)
            {
                npcAnim.SetTrigger("Greet");
                
                while (Vector3.Distance(transform.position, player.position) > waitForPlayerDistance)
                {
                    Vector3 flatDirection = new Vector3(player.position.x, 0f, player.position.z);
                    Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
                    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
                    yield return null;
                }

                npcAnim.SetTrigger("Walk");
            }
        }

        npcAnim.SetTrigger("Idle");
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private IEnumerator RunToHome()
    {
        AssignPlayer();
        npcAnim.SetTrigger("Walk");

        foreach (Transform checkpoint in runPoints)
        {
            while (Vector3.Distance(transform.position, checkpoint.position) > arrivalDistance)
            {
                Vector3 direction = (checkpoint.position - transform.position).normalized;

                Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);
                if (flatDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
                    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
                }

                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

                yield return new WaitForFixedUpdate();
            }
        }

        npcAnim.SetTrigger("Idle");
        Vector3 flatDirection2 = new Vector3(player.position.x, 0f, player.position.z);
        Quaternion targetRotation2 = Quaternion.LookRotation(flatDirection2);
        rb.MoveRotation(targetRotation2);
        StartCoroutine(Wave());
    }
}