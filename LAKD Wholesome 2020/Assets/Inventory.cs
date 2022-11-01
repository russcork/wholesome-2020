using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour
{
    public FishConfig _fishConfig;
    public FlowerConfig _flowerConfig;

    public List<string> _collected = new List<string>();
    public List<int> _collectedCount = new List<int>();

    public static Inventory _instance;
    
    public static Action<string> InventoryAddedEvent;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        foreach(FishConfig.FishDetails fish in _fishConfig._fish)
        {
            _collected.Add(fish._name);
            _collectedCount.Add(0);
        }
        foreach(FlowerConfig.FlowerDetails flower in _flowerConfig._flowers)
        {
            _collected.Add(flower._name);
            _collectedCount.Add(0);
        }
    }

    public void Log(int i)
    {
        Debug.Log(_collected[i]);
        Debug.Log(_collectedCount[i]);
    }

    public void Add(string name)
    {
        int i = GetIndex(name);
        if (i >= 0)
        {
            _collectedCount[i] = _collectedCount[i] + 1;
            InventoryAddedEvent?.Invoke(name);
        }
    }

    public int GetCollectionAmount(string name)
    {
        var index = GetIndex(name);
        return _collectedCount[index];
    }

    public int GetIndex(string name)
    {
        // this is hacky and more effor than a decent structure but brain no work good wen rush
        for (int i = 0; i < _collected.Count; i++)
        {
            if (_collected[i] == name)
            {
                return i;
            }
        }
        return -1;
    }

    public int TotalCollected()
    {
        int count = 0;
        foreach (int i in _collectedCount)
        {
            count += i;
        }
        return count;
    }

}
