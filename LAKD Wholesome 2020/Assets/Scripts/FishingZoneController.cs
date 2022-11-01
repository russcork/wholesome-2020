using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishingZoneController : MonoBehaviour
{
    public FishConfig FishConfig;
    private Interactable _interactable;

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
    }

    void Start()
    {
        _interactable.InteractTooltip = "<line-height=75%><size=50%>Press 'Space' to \n<b><size=90%>Fish";
    }

    private FishConfig.FishDetails ChooseFish()
    {
        var fishForTimeOfDay = FishConfig._fish.Where(fish => fish._time == Weather._instance._timeOfDayDescriptive);
        var weights = fishForTimeOfDay.Select(fish => fish._chance);

        var chosenFish = fishForTimeOfDay.ToList()[GetRandomWeightedIndex(weights.ToArray())];

        return chosenFish;
    }
    
    
    // thanks unity forums
    public int GetRandomWeightedIndex(float[] weights)
    {
        if (weights == null || weights.Length == 0) return -1;
 
        float w;
        float t = 0;
        int i;
        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];
 
            if (float.IsPositiveInfinity(w))
            {
                return i;
            }
            else if (w >= 0f && !float.IsNaN(w))
            {
                t += weights[i];
            }
        }
 
        float r = Random.value;
        float s = 0f;
 
        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];
            if (float.IsNaN(w) || w <= 0f) continue;
 
            s += w / t;
            if (s >= r) return i;
        }
 
        return -1;
    }
    
    public void OnInteract(PlayerController player)
    {
        _interactable.IsInteractable = false;
        
        player.FishingController.StartFishing(ChooseFish, () =>
        {
            _interactable.IsInteractable = true;
        });
    }
}
