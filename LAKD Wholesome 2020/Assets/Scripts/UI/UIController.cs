using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private StickerbookController _stickerbook;

    [SerializeField] private Button _openStickerBookButton;

    [SerializeField] private PlayerController _player;

    [SerializeField] private TextMeshProUGUI _collectedText, _clockText, _dayTimeText;

    [SerializeField] private GameObject _notificationAlert;

    [SerializeField] private Animator _houseTutorial,_moveTutorial;

    public GameObject _youWin;
    public TextMeshProUGUI _youWinTime;

    private bool _houseTutorialShown = false;

    private Weather _weather;

    public static UIController _instance;

    void Awake()
    {
        _instance = this;
        Inventory.InventoryAddedEvent += OnInventoryAdded;
    }

    void Start()
    {
        _weather = Weather._instance;
    }

    private void OnInventoryAdded(string item)
    {
        _collectedText.text = $"Collected: {Inventory._instance.TotalCollected()}";
        _notificationAlert.SetActive(true);
        
        CheckForWin();
    }

    private void CheckForWin()
    {
        if (Inventory._instance._collectedCount.All(x => x >= 1))
        {
            Show_YouWin();
        }
    }
    
    public void ShowHouseTutorial()
    {
        if (!_houseTutorialShown)
        {
            _houseTutorialShown = true;
            _moveTutorial.Play("Hide");
            _houseTutorial.Play("Show");
        }
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            Toggle_Stickerbook();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Close_Stickerbook();
        }

        _clockText.text = _weather.GetFriendlyTime();
        _dayTimeText.text = _weather._timeOfDayDescriptive.ToString();
    }

    public void Toggle_Stickerbook()
    {
        if (_stickerbook.gameObject.activeSelf)
        {
            Close_Stickerbook();
        }
        else
        {
            Open_Stickerbook();
        }
    }
    
    public void Open_Stickerbook()
    {
        SetButtonsVisible(false);
        _player.IsInputEnabled = false;
        _stickerbook.gameObject.SetActive(true);
        _notificationAlert.SetActive(false);
    }

    public void Show_YouWin()
    {
        // TODO: Need to hook this up to appear when at least one of each thing is collected
        SetButtonsVisible(false);

        var elapsed = TimeSpan.FromDays(_weather._daysElapsed);
        
        _youWinTime.text = $"{elapsed.Days} Day{(elapsed.Days == 1 ? "" : "s")}, {elapsed.Hours} Hour{(elapsed.Hours == 1 ? "" : "s")}";
            
        _player.IsInputEnabled = false;
        _youWin.SetActive(true);
    }

    public void Close_YouWin()
    {
        SetButtonsVisible(true);
        _player.IsInputEnabled = true;
        _youWin.SetActive(false);
    }

    public void Close_Stickerbook()
    {
        SetButtonsVisible(true);
        _player.IsInputEnabled = true;
        _stickerbook.Close();
        _stickerbook.gameObject.SetActive(false);
    }
    
    private void SetButtonsVisible(bool visible)
    {
        foreach (var button in new[] {_openStickerBookButton})
        {
            button.gameObject.SetActive(visible);
        }
    }
}
