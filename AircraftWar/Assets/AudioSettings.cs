using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
   public AudioMixer mixer;
    public void SetVolume(float volume)
{
	mixer.setFloat("volume", volume);
}
}
