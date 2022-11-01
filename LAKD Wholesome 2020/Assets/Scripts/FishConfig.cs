using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "FishConfig", menuName = "FishConfig")]
public class FishConfig : ScriptableObject
{
    [System.Serializable]
    public struct FishDetails
    {
        public string _name;
        public string _toolTip;
        public float _chance;
        public Weather.TimeOfDayDescriptive _time;
        public Sprite _sprite;
    }

    public List<FishDetails> _fish;
    
}
