using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{

    public AudioSource chopSFX;
    public AudioSource buildSFX;
    public void PlayChopSound()
    {
        chopSFX.Play();
    }

    public void PlayBuildSound()
    {
        buildSFX.Play();
    }
}
