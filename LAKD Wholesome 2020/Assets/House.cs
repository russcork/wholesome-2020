using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public GameObject _litWindow;
    public ParticleSystem _chimneySmoke;

    void Start()
    {
        _chimneySmoke.enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 1)
        {
            _litWindow.SetActive(true);
            _chimneySmoke.enableEmission = true;
        }
        else
        {
            _litWindow.SetActive(false);
             _chimneySmoke.enableEmission = false;
        }
    }
}
