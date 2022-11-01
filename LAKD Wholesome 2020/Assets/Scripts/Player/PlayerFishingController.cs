using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlayerFishingController : MonoBehaviour
{
    [SerializeField] private Vector2 _bobSecondsRange;
    [SerializeField] private float _pullWithinSeconds;
    
    private PlayerController _playerController;
    private bool isFishing;
    private bool hasPulled;
    private bool isHooked;
    
    [SerializeField] private Animator _bobAnimator;
    
    private Coroutine _bobAfterTimeCoroutine;
    
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public void StartFishing(Func<FishConfig.FishDetails> OnChooseFish, Action OnComplete)
    {
        StartCoroutine(FishingCoroutine(OnChooseFish, OnComplete));
    }

    private void Update()
    {
        if (isFishing && !hasPulled)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                hasPulled = true;
            }
        }
    }

    private IEnumerator FishingCoroutine(Func<FishConfig.FishDetails> OnChooseFish, Action OnComplete)
    {
        _playerController.IsInputEnabled = false;
        _playerController.ChangeAnimationState(PlayerAnimationState.Fishing);
        _playerController.SetTrigger("StartFishing");
        _playerController.FaceTowards(Vector2.right * 10f);
        AudioManager.PlaySound("SFX_FishOut", 1f);
        
        yield return new WaitForSeconds(0.5f);

        isFishing = true;
        hasPulled = false;

        _bobAfterTimeCoroutine = StartCoroutine(HookAfterTimeCoroutine());

        yield return new WaitUntil(() => hasPulled);

        hasPulled = false;
        isFishing = false;
        
        StopCoroutine(_bobAfterTimeCoroutine);
        _bobAnimator.SetBool("Hooked", false);

        _playerController.SetTrigger("FishingPull");

        yield return new WaitForSeconds(0.2f);
        AudioManager.PlaySound("SFX_FishIn", 1f);
        _playerController.IsInputEnabled = true;
        
        if (isHooked)
        {
            var chosenFish = OnChooseFish();
            _playerController.FlowerController.CollectFish(chosenFish, null);
            Inventory._instance.Add(chosenFish._name);
        }
        else
        {
            _playerController.ChangeAnimationState(PlayerAnimationState.Idle);
        }

        isHooked = false;
        OnComplete?.Invoke();
    }

    private IEnumerator HookAfterTimeCoroutine()
    {
        while (!hasPulled)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(_bobSecondsRange.x, _bobSecondsRange.y));
            _bobAnimator.SetBool("Hooked", true);
            isHooked = true;
            yield return new WaitForSeconds(_pullWithinSeconds);
            isHooked = false;
            _bobAnimator.SetBool("Hooked", false);
        }
    }
}
