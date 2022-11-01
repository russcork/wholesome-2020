using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlowerController : MonoBehaviour
{
    [SerializeField] private float wateringTime;
    [SerializeField] private float collectionCelebrationTime;
    [SerializeField] private SpriteRenderer collectionView;
    
    private PlayerController _playerController;

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public void CollectFlower(Flower plant, Action OnComplete)
    {
        StartCoroutine(CollectCoroutine(plant._flowerType._08, plant.transform.position, OnComplete));
    }

    public void CollectFish(FishConfig.FishDetails fish, Action OnComplete)
    {
        StartCoroutine(CollectCoroutine(fish._sprite, Vector2.right * 10f, OnComplete));
    }
    
    public void WaterFlower(Flower plant, Action OnComplete)
    {
        StartCoroutine(WaterFlowerCoroutine(plant, OnComplete));
    }

    private IEnumerator CollectCoroutine(Sprite sprite, Vector3 position, Action OnComplete)
    {
        collectionView.sprite = sprite;
        _playerController.IsInputEnabled = false;
        _playerController.ChangeAnimationState(PlayerAnimationState.CollectionCelebration);
        _playerController.FaceTowards(position);
        
        yield return new WaitForSeconds(collectionCelebrationTime);
        
        _playerController.ChangeAnimationState(PlayerAnimationState.Idle);
        _playerController.IsInputEnabled = true;
        
        OnComplete?.Invoke();
    }
    
    private IEnumerator WaterFlowerCoroutine(Flower plant, Action OnComplete)
    {
        _playerController.IsInputEnabled = false;
        _playerController.ChangeAnimationState(PlayerAnimationState.Watering);
        _playerController.FaceTowards(plant.transform.position);
        
        yield return new WaitForSeconds(collectionCelebrationTime);
        
        _playerController.ChangeAnimationState(PlayerAnimationState.Idle);
        _playerController.IsInputEnabled = true;
        
        OnComplete?.Invoke();
    }
}
