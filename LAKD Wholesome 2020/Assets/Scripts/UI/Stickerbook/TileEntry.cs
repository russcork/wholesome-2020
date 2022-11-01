using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private StickerbookController _stickerbookController;
    [SerializeField] private Image _image;

    [SerializeField] private Sprite _normalBorderSprite;
    [SerializeField] private Sprite _hoveredBorderSprite;
    [SerializeField] private Image _borderImage;
    [SerializeField] private Sprite _lockedSprite;
    [SerializeField] private TextMeshProUGUI _countText;
    
    public Sprite Sprite;
    public string ItemName;
    public string Tooltip;

    void Awake()
    {
        _stickerbookController = GetComponentInParent<StickerbookController>();
    }
    
    public void Setup(Sprite sprite, string name, string tooltip, int collectedAmount)
    {
        Sprite = sprite;
        ItemName = name;
        Tooltip = tooltip;

        if (collectedAmount > 0)
        {
            _image.sprite = sprite;
            _countText.gameObject.SetActive(true);
            _countText.text = collectedAmount.ToString();
        }
        else
        {
            _image.sprite = _lockedSprite;
            ItemName = "???";
            _countText.gameObject.SetActive(false);
        }
        
        SetHovered(false);
    }

    private void SetHovered(bool hovered)
    {
        _borderImage.sprite = hovered ? _hoveredBorderSprite : _normalBorderSprite;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _stickerbookController.StartHoveringTile(this);
        SetHovered(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _stickerbookController.StopHoveringTile(this);
        SetHovered(false);
    }
}
