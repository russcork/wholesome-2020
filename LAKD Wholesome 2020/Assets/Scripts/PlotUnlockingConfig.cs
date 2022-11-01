using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlotUnlockingConfig", menuName = "PlotUnlockingConfig")]
public sealed class PlotUnlockingConfig : ScriptableObject
{
    public List<int> UnlockAtCollected;
}