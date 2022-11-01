using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private PlayerController _playerController;
    
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        _playerController.Movement = Input.GetAxis("Horizontal");
    }
}
