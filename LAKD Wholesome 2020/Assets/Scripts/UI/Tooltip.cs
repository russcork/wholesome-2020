using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private RectTransform _rectTransform;

    public void Setup(string title, string description)
    {
        _title.text = title;
        _description.text = description;
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }
}
