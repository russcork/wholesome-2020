using System;
using UnityEngine;

public enum PlayerAnimationState
{
    Idle,
    Running,
    Watering,
    Fishing,
    CollectionCelebration
}
public class PlayerController : MonoBehaviour
{
    public PlayerFlowerController FlowerController;
    public PlayerFishingController FishingController;
    
    [SerializeField] private int houseTimeFactor;
    [SerializeField] private float timeBetweenFootsteps;
    private float _sinceLastFootstep;
        
    private static int k_idleTrigger = Animator.StringToHash("Idle");
    private static int k_runningTrigger = Animator.StringToHash("Running");
    private static int k_fishingTrigger = Animator.StringToHash("Fishing");
    private static int k_wateringTrigger = Animator.StringToHash("Watering");
    private static int k_collectionCeleberationTrigger = Animator.StringToHash("CollectionCelebration");

    public bool IsInputEnabled
    {
        get => _inputDisablers == 0;

        set
        {
            if (value)
            {
                _inputDisablers--;
                if (_inputDisablers < 0) _inputDisablers = 0;
            }
            else
            {
                _inputDisablers++;
            }
        }
    }

    private int _inputDisablers;
    
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sprite;

    public float MovementSpeed;
    public float Movement; // -1 go left at full speed, 1 is go right at full speed

    private bool isInHouse;
    public GameObject _waterVFX;
    private bool _facingRight;
    private PlayerAnimationState _currentState = PlayerAnimationState.Idle;

    void Awake()
    {
        FlowerController = GetComponent<PlayerFlowerController>();
        FishingController = GetComponent<PlayerFishingController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("House"))
        {
            isInHouse = true;
            AudioManager.PlaySound("SFX_Door_Open01", 0.7f);
            Time.timeScale = houseTimeFactor;
            AudioManager.PlaySound("SFX_TimeSpeed01", 1f);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("House"))
        {
            isInHouse = false;
            AudioManager.PlaySound("SFX_Door_Close01", 0.7f);
            Time.timeScale = 1;
            AudioManager.PlaySound("SFX_TimeSlow01", 1f);
        }
    }

    void Update()
    {
        if (!IsInputEnabled)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }
        
        _rigidbody.velocity = new Vector2(MovementSpeed * Movement, 0f);

        if (Movement > 0) _facingRight = false;
        else if (Movement < 0) _facingRight = true;

        var isMoving = Mathf.Approximately(Movement, 0f) == false;
        switch (_currentState)
        {
            case PlayerAnimationState.Idle:
                if (isMoving)
                {
                    ChangeAnimationState(PlayerAnimationState.Running);
                }

                break;
            case PlayerAnimationState.Running:
                Footstep();
                
                if (!isMoving)
                {
                    ChangeAnimationState(PlayerAnimationState.Idle);
                }

                break;
        }
        
        _sprite.flipX = _facingRight;
    }

    private void Footstep()
    {
        if (isInHouse)
        {
            return;
        }
        
        _sinceLastFootstep += Time.deltaTime;
        if (_sinceLastFootstep > timeBetweenFootsteps)
        {
            _sinceLastFootstep = 0;
            AudioManager._instance.PlayFootstep();
        }
    }

    public void FaceTowards(Vector3 position)
    {
        var xDiff = (this.transform.position - position).x;

        if (xDiff > 0)
        {
            _sprite.flipX = true;
            _waterVFX.transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            _sprite.flipX = false;
            _waterVFX.transform.localScale = new Vector3(1,1,1);
        }
    }
    
    
    public void SetTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
    }

    public void ChangeAnimationState(PlayerAnimationState newState)
    {
        _currentState = newState;

        foreach (var trigger in new[] {k_idleTrigger, k_runningTrigger, k_wateringTrigger, k_collectionCeleberationTrigger, k_fishingTrigger})
        {
            _animator.SetBool(trigger, false);
        }

        switch (newState)
        {
            case PlayerAnimationState.Idle:
                _animator.SetBool(k_idleTrigger, true);
                break;
            case PlayerAnimationState.Running:
                _animator.SetBool(k_runningTrigger, true);
                break;
            case PlayerAnimationState.Watering:
                _animator.SetBool(k_wateringTrigger, true);
                break;
            case PlayerAnimationState.Fishing:
                _animator.SetBool(k_fishingTrigger, true);
                break;
            case PlayerAnimationState.CollectionCelebration:
                _animator.SetBool(k_collectionCeleberationTrigger, true);
                break;
        }
    }
}
