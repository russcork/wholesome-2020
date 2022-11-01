using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioClip[] _clips;

    private string[] _footstepSoundNames = new[]
    {
        "SFX_Footstep_01",
        "SFX_Footstep_02",
        "SFX_Footstep_02",
        "SFX_Footstep_02"
    };

    private int _lastFootstepIndex = 0;
    
    public void Awake() 
    {
        _instance = this; 
    }

    public void PlayFootstep()
    {
        PlaySound(_footstepSoundNames[_lastFootstepIndex], 0.4f);
        _lastFootstepIndex++;
        
        if (_lastFootstepIndex >= _footstepSoundNames.Length)
        {
            _lastFootstepIndex = 0;
        }
    }
    
    public static void PlaySound(string name, float vol)
    {
        _instance._sfxSource.PlayOneShot(_instance._clips.First(c => c.name == name), vol);
    }
}
