using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firstnpc : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float arrivalDistance = 1f;
    [SerializeField] private float waitForPlayerDistance = 4f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Character Controller")]
    [SerializeField] private float gravity = -9.81f;

    [Header("Checkpoints")]
    public GameObject eventIndicator;
    public List<Transform> checkPoints = new List<Transform>();
    public List<Transform> runPoints = new List<Transform>();

    private Animator npcAnim;
    private CharacterController _cc;
    private Transform player;
    private DictionaryManager _dictionary;
    private bool inDialogue;
    private bool hasTranslated = false;
    private float _verticalVelocity = 0f;
    private WaitForSecondsRealtime waveCycle = new WaitForSecondsRealtime(5);

    void Start()
    {
        _dictionary = DictionaryManager.Instance;
        npcAnim = GetComponent<Animator>();
        _cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        hasTranslated = _dictionary.hasTranslated;
        ApplyGravity();
    }

    // ─── GRAVITY ─────────────────────────────────────────────────────────────

    private void ApplyGravity()
    {
        if (_cc.isGrounded)
            _verticalVelocity = -1f;
        else
            _verticalVelocity += gravity * Time.deltaTime;

        _cc.Move(new Vector3(0, _verticalVelocity * Time.deltaTime, 0));
    }

    // ─── ROTATION ────────────────────────────────────────────────────────────

    private void RotateToward(Vector3 targetPosition)
    {
        Vector3 flatDirection = new Vector3(
            targetPosition.x - transform.position.x,
            0f,
            targetPosition.z - transform.position.z
        );

        if (flatDirection == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // ─── MOVEMENT ────────────────────────────────────────────────────────────

    private void MoveToward(Vector3 targetPosition, float speedMultiplier = 1f)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);
        _cc.Move(flatDirection * moveSpeed * speedMultiplier * Time.deltaTime);
    }

    // ─── PUBLIC ──────────────────────────────────────────────────────────────

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
        var interactionScript = GetComponent<NPCScript>();
        Destroy(interactionScript);
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

    // ─── COROUTINES ──────────────────────────────────────────────────────────

    private IEnumerator Wave()
    {
        do
        {
            npcAnim.SetTrigger("Greet");
            yield return waveCycle;
        }
        while (!inDialogue);
    }

    private IEnumerator GoToCheckpoints()
    {
        npcAnim.SetTrigger("Greet");
        yield return new WaitUntil(() => hasTranslated);
        AssignPlayer();
        npcAnim.SetTrigger("Walk");

        foreach (Transform checkpoint in checkPoints)
        {
            while (Vector3.Distance(transform.position, checkpoint.position) > arrivalDistance)
            {
                RotateToward(checkpoint.position);
                MoveToward(checkpoint.position);
                yield return null;
            }

            if (player != null && Vector3.Distance(transform.position, player.position) > waitForPlayerDistance)
            {
                npcAnim.SetTrigger("Greet");

                while (Vector3.Distance(transform.position, player.position) > waitForPlayerDistance)
                {
                    RotateToward(player.position);
                    yield return null;
                }

                npcAnim.SetTrigger("Walk");
            }
        }

        npcAnim.SetTrigger("Idle");
        _cc.enabled = false; 
        CloseEventIndicator();
    }

    private IEnumerator RunToHome()
    {
        AssignPlayer();
        npcAnim.SetTrigger("Walk");

        foreach (Transform checkpoint in runPoints)
        {
            while (Vector3.Distance(transform.position, checkpoint.position) > arrivalDistance)
            {
                RotateToward(checkpoint.position);
                MoveToward(checkpoint.position, speedMultiplier: 2f);
                yield return null;
            }
        }

        npcAnim.SetTrigger("Idle");

        while (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(new Vector3(
            player.position.x - transform.position.x,
            0f,
            player.position.z - transform.position.z))) > 1f)
        {
            RotateToward(player.position);
            yield return null;
        }

        StartCoroutine(Wave());
    }
}