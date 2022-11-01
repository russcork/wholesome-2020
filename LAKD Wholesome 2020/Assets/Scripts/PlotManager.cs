using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    [SerializeField] private Transform _plotsContainer;
    
    public static PlotManager _instance;

    public PlotUnlockingConfig _unlockConfig;
    private List<int> _alreadyUnlockedThreshholds = new List<int>();
    private int _pendingUnlocks;
    
    private Flower[] _plots;
    void Awake()
    {
        _instance = this;
        _plots = _plotsContainer.GetComponentsInChildren<Flower>();
    }

    void Start()
    {
        Inventory.InventoryAddedEvent += OnInventoryAdded;
    }

    private void OnInventoryAdded(string item)
    {
        var total = Inventory._instance.TotalCollected();

        int chosenThreshhold = -1;
        foreach (int collectedThreshold in _unlockConfig.UnlockAtCollected.Except(_alreadyUnlockedThreshholds))
        {
            if (total >= collectedThreshold)
            {
                chosenThreshhold = collectedThreshold;
            }
        }

        if (chosenThreshhold != -1)
        {
            _alreadyUnlockedThreshholds.Add(chosenThreshhold);
            _pendingUnlocks++;
            UpdateUnlockables();
        }
    }

    public void PlotUnlocked(Flower flower)
    {
        _pendingUnlocks--;
        if (_pendingUnlocks < 0) _pendingUnlocks = 0;
        UpdateUnlockables();
    }

    private void UpdateUnlockables()
    {
        bool shouldMakeUnlockable = _pendingUnlocks > 0;
        
        foreach (var flower in _plots)
        {
            flower._unlockable = shouldMakeUnlockable;
            flower.UpdateUnlockable();
        }
    }
}
