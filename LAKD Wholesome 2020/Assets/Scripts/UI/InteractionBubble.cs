using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private RectTransform _rectTransform;
    
    public void SetText(string text)
    {
        _text.text = text;
        
        // lmao
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }
}
