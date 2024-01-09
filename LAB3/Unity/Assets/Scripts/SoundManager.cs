using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private bool isSoundOn = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsSoundOn()
    {
        return isSoundOn;
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        UpdateSoundState();
    }

    private void UpdateSoundState()
    {
        AudioListener.volume = isSoundOn ? 1.0f : 0.0f;
    }
}
