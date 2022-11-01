using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "FlowerConfig", menuName = "FlowerConfig")]
public class FlowerConfig : ScriptableObject
{
    [System.Serializable]
    public struct FlowerDetails
    {
        public string _name;
        public string _toolTip;
        public Sprite _05;
        public Sprite _06;
        public Sprite _07;
        public Sprite _08;
    }

    public int _timeToGrow;

    public Sprite _plot;

    public string _name;

    public Sprite _01;
    public Sprite _02;
    public Sprite _03;
    public Sprite _04;
    
    public List<FlowerDetails> _flowers;


    
}
