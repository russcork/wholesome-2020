using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private InteractionBubble _interactionBubble;
    [SerializeField] private Transform _interactionBubblePosition;
    private PlayerController _playerController;

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public Interactable CurrentInteractable = null;

    private void Update()
    {
        if (!_playerController.IsInputEnabled) return;
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (CurrentInteractable != null && CurrentInteractable.IsInteractable)
            {
                CurrentInteractable.DoInteract(_playerController);
            }
        }
    }

    private void FixedUpdate()
    {
        if (CurrentInteractable != null && CurrentInteractable.IsInteractable && _playerController.IsInputEnabled)
        {
            _interactionBubble.transform.position = _interactionBubblePosition.position;
            _interactionBubble.SetText(CurrentInteractable.InteractTooltip);
            _interactionBubble.gameObject.SetActive(true);
        }
        else
        {
            _interactionBubble.gameObject.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            var interactable = other.GetComponentInParent<Interactable>();
            interactable.InteractableEnter(_playerController);
            CurrentInteractable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            var interactable = other.GetComponentInParent<Interactable>();
            interactable.InteractableExit(_playerController);

            if (CurrentInteractable == interactable)
            {
                CurrentInteractable = null;
            }
        }
    }
}
