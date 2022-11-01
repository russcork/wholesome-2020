using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flower : MonoBehaviour
{
    public enum GrowState
    {
        Locked = 0,
        Plot = 1,
        Growing = 2,
        Done = 3
    }

    public FlowerConfig _flowerConfig;
    public GameObject _lockedView;
    public GameObject _plotView;
    public GameObject _flowerView;
    public GameObject _highlightView;
    public GameObject _unlockableView;
    public GameObject _toBeCollectedView;
    public TextMeshPro _flowerName;
    public SpriteRenderer _flower;
    public FlowerConfig.FlowerDetails _flowerType;
    private bool _flowerTypeDetermined = false;

    public GrowState _growState;
    public int _growth = 0;
    private int _timeToGrow = 2100;

    public bool _unlockable = false;

    private Interactable _interactable;


    public Weather.TimeOfDayDescriptive _plantTime;
    public int _dawnWaters; 
    public int _dayWaters; 
    public int _duskWaters; 
    public int _nightWaters;


    void Awake()
    {
        _interactable = GetComponent<Interactable>();
        _highlightView.SetActive(false);
    }

    void Start()
    {
        _timeToGrow = _flowerConfig._timeToGrow;
        switch (_growState)
        {
            case GrowState.Plot:
                Unlock();
                break;
            case GrowState.Locked:
                Lock();
                break;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        var canInteract = false;
        
        if (_growState == GrowState.Locked)
        {
            if (_unlockable)
            {
                canInteract = true;
                _interactable.InteractTooltip = "<line-height=75%><size=50%>Press 'Space' to \n<b><size=90%>Unlock";
            }
        } if (_growState == GrowState.Growing)
        {
            canInteract = true;
            _interactable.InteractTooltip = "<line-height=75%><size=50%>Press 'Space' to \n<b><size=90%>Water";
            
            _growth += (int)Time.timeScale;
            ChangeVisual();
            if (_growth > _timeToGrow)
            {
                _toBeCollectedView.SetActive(true);
                _growState ++;
            }
        } else if (_growState == GrowState.Plot)
        {
            canInteract = true;
            _interactable.InteractTooltip = "<line-height=75%><size=50%>Press 'Space' to \n<b><size=90%>Plant";
        } else if (_growState == GrowState.Done)
        {
            canInteract = true;
            _interactable.InteractTooltip = "<line-height=75%><size=50%>Press 'Space' to \n<b><size=90%>Collect";
        }

        _interactable.IsInteractable = canInteract;

        UpdateFlowerName();
    }

    private void UpdateFlowerName()
    {
        if (_growState == GrowState.Growing || _growState == GrowState.Done)
        {
            _flowerName.text = _flowerTypeDetermined ? _flowerType._name : "???";
        } else
        {
            _flowerName.text = String.Empty;
        }
    }

    public GrowState GetState()
    {
        return _growState;
    }

    public void ShowHighlight()
    {
        if (_growState != GrowState.Locked || _unlockable)
            _highlightView.SetActive(true);
    }

    public void HideHighlight()
    {
        _highlightView.SetActive(false);
    }
    
    public void OnInteract(PlayerController player)
    {
        // Plant, Water or Collect depending on state

        switch (_growState)
        {
            case GrowState.Locked:
                TryUnlock();
                break;
            case GrowState.Plot:
                Plant(player);
                break;
            case GrowState.Growing:
                Water(player);
                break;
            case GrowState.Done:
                Collect(player);
                break;
        }
    }

    public void OnInteractableEnter(PlayerController player)
    {
        ShowHighlight();
    }
    
    public void OnInteractableExit(PlayerController player)
    {
        HideHighlight();
    }

    public void ChangeVisual()
    {
        if (_growth > _timeToGrow)
        {
            _flower.sprite = _flowerType._08;
        }
        else if (_growth > (_timeToGrow/7)*6)
        {
            _flower.sprite = _flowerType._07;
        }
        else if (_growth > (_timeToGrow/7)*5)
        {
            _flower.sprite = _flowerType._06;
        }
        else if (_growth > (_timeToGrow/7)*4)
        {
            DetermineFlower();
            _flower.sprite = _flowerType._05;
        }
        else if (_growth > (_timeToGrow/7)*3)
        {
            _flower.sprite = _flowerConfig._04;
        }
        else if (_growth > (_timeToGrow/7)*2)
        {
            _flower.sprite = _flowerConfig._03;
        }
        else if (_growth > (_timeToGrow/7))
        {
            _flower.sprite = _flowerConfig._02;
        }
    }

    public void DetermineFlower()
    {
        if (_flowerTypeDetermined)
            return;
        
        int f = Random.Range(0,_flowerConfig._flowers.Count);

        // cbf doing this nicely
        if (_plantTime == Weather.TimeOfDayDescriptive.Dawn)
        {
            f = 0;
        }
        else if (_plantTime == Weather.TimeOfDayDescriptive.Day)
        {
            f = 1;
        }
        else if (_plantTime == Weather.TimeOfDayDescriptive.Dusk)
        {
            f = 2;
        }
        else if (_plantTime == Weather.TimeOfDayDescriptive.Night)
        {
            f = 3;
        }

        if (_dawnWaters == 2)
        {
            f = 4;
        }
        else if (_dayWaters == 2)
        {
            f = 5;
        }
        else if (_duskWaters == 2)
        {
            f = 6;
        }
        else if (_nightWaters == 2)
        {
            f = 7;
        }

        if (_dawnWaters >= 3)
        {
            f = 8;
        }
        else if (_dayWaters >= 3)
        {
            f = 9;
        }
        else if (_duskWaters >= 3)
        {
            f = 10;
        }
        else if (_nightWaters >= 3)
        {
            f = 11;
        }

        if (_nightWaters > 0 && _dawnWaters > 0)
        {
            f = 12;
        }
        else if (_dawnWaters > 0 && _dayWaters > 0)
        {
            f = 13;
        }
        else if (_dayWaters > 0 && _duskWaters > 0)
        {
            f = 14;
        }
        else if (_duskWaters > 0 && _nightWaters > 0)
        {
            f = 15;
        }
        
        

        _flowerType = _flowerConfig._flowers[f];
        _flowerTypeDetermined = true;
    }

    int TotalWaters()
    {
        return _dawnWaters + _dayWaters + _duskWaters + _nightWaters;
    }

    void TryUnlock()
    {
        if (_unlockable)
        {
            Unlock();
        }
    }

    void Unlock()
    {
        _interactable.IsInteractable = true;
        _growState = GrowState.Plot;
        _plotView.SetActive(true);
        _lockedView.SetActive(false);
        _flowerView.SetActive(false);
        _unlockableView.SetActive(false);
        
        PlotManager._instance.PlotUnlocked(this);
    }

    void Plant(PlayerController player)
    {
        UIController._instance.ShowHouseTutorial();
        
        _growState = GrowState.Growing;
        _plotView.SetActive(false);
        _flowerView.SetActive(true);
        _lockedView.SetActive(false);
        _flower.sprite = _flowerConfig._01;
        _plantTime = Weather._instance._timeOfDayDescriptive;
    }

    void Water(PlayerController player)
    {
        player.FlowerController.WaterFlower(this, FinishWatering);
    }

    private void FinishWatering()
    {
        switch (Weather._instance._timeOfDayDescriptive)
        {
            case Weather.TimeOfDayDescriptive.Dawn:
                _dawnWaters ++;
                break;
            case Weather.TimeOfDayDescriptive.Dusk:
                _duskWaters ++;
                break;
            case Weather.TimeOfDayDescriptive.Day:
                _dayWaters ++;
                break;
            case Weather.TimeOfDayDescriptive.Night:
                _nightWaters ++;
                break;
        }
    }

    void Collect(PlayerController player)
    {
        _flowerView.SetActive(false);
        Inventory._instance.Add(_flowerType._name);
        
        player.FlowerController.CollectFlower(this, Reset);
    }

    public void Reset()
    {
        _toBeCollectedView.SetActive(false);
        _growState = GrowState.Plot;
        _plotView.SetActive(true);
        _lockedView.SetActive(false);
        _growth = 0;
        _dawnWaters = 0;
        _nightWaters = 0;
        _duskWaters = 0;
        _dayWaters = 0;
        _flowerTypeDetermined = false;
    }

    void Lock()
    {
        _interactable.IsInteractable = false;
        _growState = GrowState.Locked;
        _plotView.SetActive(false);
        _lockedView.SetActive(true);
        _flowerView.SetActive(false);
        
        UpdateUnlockable();
    }

    public void UpdateUnlockable()
    {
        var isUnlockable = _unlockable && _growState == GrowState.Locked;
        
        _unlockableView.SetActive(isUnlockable);
    }
}
