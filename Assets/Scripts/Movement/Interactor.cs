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
    private bool interactableInRange;
    private IInteractable interactable;

    private float _exitCooldown = 0f;
    private const float ExitDelay = 0.15f;

    private bool _uiWasOpen = false;

    private void Start()
    {
        playerObject = transform.parent;
        _playerRb = transform.parent.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleExitCooldown();
        HandleInteractTextVisibility();
        HandleInteractInput();
    }

    // ─── UI VISIBILITY ───────────────────────────────────────────────────────

    private bool IsAnyUIOpen()
    {
        return noteBookObject?.activeInHierarchy == true ||
               settingsObject?.activeInHierarchy == true ||
               dialogueScreen?.activeInHierarchy == true;
    }

    private void HandleInteractTextVisibility()
    {
        bool uiOpen = IsAnyUIOpen();

        if (uiOpen && !_uiWasOpen)
        {
            interactText?.SetActive(false);
            _uiWasOpen = true;
        }

        if (!uiOpen && _uiWasOpen)
        {
            _uiWasOpen = false;
            if (interactableInRange)
                interactText?.SetActive(true);
        }
    }

    // ─── EXIT COOLDOWN ───────────────────────────────────────────────────────

    private void HandleExitCooldown()
    {
        if (_exitCooldown <= 0f) return;

        _exitCooldown -= Time.deltaTime;
        if (_exitCooldown <= 0f)
        {
            interactText?.SetActive(false);
            interactable?.EndInteraction();
            interactableInRange = false;
            interactable = null;
        }
    }

    // ─── INTERACT INPUT ──────────────────────────────────────────────────────

    private void HandleInteractInput()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;
        if (!interactableInRange) return;
        if (IsAnyUIOpen()) return;

        MonoBehaviour mb = interactable as MonoBehaviour;
        if (mb != null && mb.gameObject != null)
        {
            interactable.Interact();
            interactText?.SetActive(false);

            interactedObject = mb.gameObject;

            Vector3 flatDirection = new Vector3(
                interactedObject.transform.position.x - playerObject.position.x,
                0f,
                interactedObject.transform.position.z - playerObject.position.z
            );

            Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
            _playerRb?.MoveRotation(targetRotation);
            if (_playerRb != null) _playerRb.angularVelocity = Vector3.zero;
        }
        else
        {
            interactableInRange = false;
            interactable = null;
            interactText?.SetActive(false);
        }
    }

    // ─── TRIGGERS ────────────────────────────────────────────────────────────

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out IInteractable interactObj)) return;

        if (interactable != null && interactable != interactObj)
            interactable.EndInteraction();

        _exitCooldown = 0f;
        interactable = interactObj;
        interactableInRange = true;

        if (!IsAnyUIOpen())
            interactText?.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable _))
            _exitCooldown = ExitDelay;
    }
}