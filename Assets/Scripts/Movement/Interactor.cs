using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float interactRange = 3f;
    [SerializeField] private float exitRange = 3.5f;
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
    private Collider _currentCollider;

    private void Start()
    {
        playerObject = transform.parent;
        _playerRb = gameObject.GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        if (interactableInRange && _currentCollider != null)
        {
            float dist = Vector3.Distance(transform.position, _currentCollider.transform.position);
            if (dist > exitRange)
            {
                interactText?.SetActive(false);
                interactable?.EndInteraction();
                interactableInRange = false;
                _currentCollider = null;
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
            interactText?.SetActive(true);
            interactable = interactObj;
            interactableInRange = true;
            _currentCollider = other;
        }
    }

}