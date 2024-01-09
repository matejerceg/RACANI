using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    // Start is called before the first frame update
    public Toggle soundToggle;

    void Start()
    {
        if (soundToggle != null)
        {
            soundToggle.isOn = SoundManager.instance.IsSoundOn();
            soundToggle.onValueChanged.AddListener(ToggleSound);
        }
    }

    void ToggleSound(bool isOn)
    {
        SoundManager.instance.ToggleSound();
    }
}
