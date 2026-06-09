using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float interactRange = 3f;
    public GameObject interactText;
    public GameObject playerCamera;
    public GameObject noteBookObject;
    public GameObject settingsObject;
    public GameObject dialogueScreen;

    private Transform playerObject;
    private GameObject interactedObject;
    private Rigidbody _playerRb;
    private bool _stopLooking;
    private bool interactableInRange;
    private IInteractable interactable;

    private float _exitCooldown = 0f;
    private const float ExitDelay = 0.15f; // seconds before exit is confirmed

    private void Start()
    {
        playerObject = transform.parent;
        _playerRb = gameObject.GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        // Count down exit cooldown — only actually exit when it reaches zero
        if (_exitCooldown > 0f)
        {
            _exitCooldown -= Time.deltaTime;
            if (_exitCooldown <= 0f)
            {
                interactText?.SetActive(false);
                interactable?.EndInteraction();
                interactableInRange = false;
                interactable = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && interactableInRange &&
            noteBookObject?.activeInHierarchy == false &&
            settingsObject?.activeInHierarchy == false &&
            dialogueScreen?.activeInHierarchy == false)
        {
            MonoBehaviour mb = interactable as MonoBehaviour;
            if (mb != null && mb.gameObject != null)
            {
                interactable?.Interact();
                interactText?.SetActive(false);

                interactedObject = mb.gameObject;
                _stopLooking = false;

                Vector3 flatDirection = new Vector3(
                    interactedObject.transform.position.x - playerObject.position.x,
                    0f,
                    interactedObject.transform.position.z - playerObject.position.z
                );

                Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
                _playerRb?.MoveRotation(targetRotation);
                _playerRb.angularVelocity = Vector3.zero;
            }
            else
            {
                interactableInRange = false;
                interactable = null;
                interactText?.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactObj))
        {
            _exitCooldown = 0f; // cancel any pending exit
            interactText?.SetActive(true);
            interactable = interactObj;
            interactableInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable _))
        {
            // Don't exit immediately — start the cooldown timer
            // If OnTriggerEnter fires again within ExitDelay, the exit is cancelled
            _exitCooldown = ExitDelay;
        }
    }
}