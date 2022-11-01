using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StickerbookController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Transform _flowerTileContainer;
    [SerializeField] private Transform _fishTileContainer;
    [SerializeField] private FlowerConfig _flowerConfig;
    [SerializeField] private FishConfig _fishConfig;
    [SerializeField] private Tooltip _tooltip;

    private Inventory _inventory;
    private TileEntry[] _flowerEntries;
    private TileEntry[] _fishEntries;
    
    private TileEntry _currentlyHoveredTile = null;
    
    void Awake()
    {
        _flowerEntries = _flowerTileContainer.GetComponentsInChildren<TileEntry>();
        _fishEntries = _fishTileContainer.GetComponentsInChildren<TileEntry>();
        _inventory = Inventory._instance;
    }

    public void Close()
    {
        _currentlyHoveredTile = null;
        _tooltip.gameObject.SetActive(false);
    }
    
    private void UpdateTileEntries()
    {
        for (var i = 0; i < _flowerConfig._flowers.Count; i++)
        {
            var flower = _flowerConfig._flowers[i];
            var entry = _flowerEntries[i];
            var collectedAmount = _inventory.GetCollectionAmount(flower._name);
            
            entry.Setup(flower._08, flower._name, collectedAmount > 0 ? flower._toolTip : "You haven't found this flower yet!", collectedAmount);
        }
        
        for (var i = 0; i < _fishConfig._fish.Count; i++)
        {
            var fish = _fishConfig._fish[i];
            var entry = _fishEntries[i];
            var collectedAmount = _inventory.GetCollectionAmount(fish._name);
            
            entry.Setup(fish._sprite, fish._name, collectedAmount > 0 ? fish._toolTip : "You haven't found this fish yet!", collectedAmount);
        }
    }

    private void Update()
    {
        if (_currentlyHoveredTile != null)
        {
            if (!_tooltip.gameObject.activeSelf) _tooltip.gameObject.SetActive(true);
            
            var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _canvas.planeDistance);
            _tooltip.transform.position = _camera.ScreenToWorldPoint(mousePosition);
        }
        else
        {
            if (_tooltip.gameObject.activeSelf) _tooltip.gameObject.SetActive(false);
        }
    }

    public void StartHoveringTile(TileEntry tile)
    {
        _currentlyHoveredTile = tile;
        _tooltip.Setup(tile.ItemName, tile.Tooltip);
    }

    public void StopHoveringTile(TileEntry tile)
    {
        if (_currentlyHoveredTile == tile)
        {
            _currentlyHoveredTile = null;
        }
    }
    
    void OnEnable()
    {
        UpdateTileEntries();
    }
}
