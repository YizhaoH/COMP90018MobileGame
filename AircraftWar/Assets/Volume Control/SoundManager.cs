using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	[SerializeField] Slider volumeSlider;
    [SerializeField] Slider AlexvolumeSlider;
    [SerializeField] Slider JoyvolumeSlider;
    void Start()
    {
        if ((!PlayerPrefs.HasKey("bgmusic")))
        {
            PlayerPrefs.SetFloat("bgmusic", 1);
            Load();
        }
        else if((!PlayerPrefs.HasKey("alexander-nakarada-superepic")))
        {
            PlayerPrefs.SetFloat("alexander-nakarada-superepic", 1);
            Load();
        }
        else if((!PlayerPrefs.HasKey("joystock-write-your-story")))
        {
            PlayerPrefs.SetFloat("joystock-write-your-story", 1);
            Load();
        }
        else
        {
            Load();
        }

    }

    public void ChangeVolume(){
        if (volumeSlider)
	    AudioListener.volume = volumeSlider.value;
        if (AlexvolumeSlider)
	    AudioListener.volume = AlexvolumeSlider.value;
        if (JoyvolumeSlider)
	    AudioListener.volume = JoyvolumeSlider.value;
	    Save();
    }

    public void ResetVolume()
    {
        volumeSlider.value = 1;
        AlexvolumeSlider.value = 1;
        JoyvolumeSlider.value = 1;
        AudioListener.volume = 1;
	    Save();
    }
    private void Load(){
        if(volumeSlider)
        volumeSlider.value = PlayerPrefs.GetFloat("bgmusic");
        if (AlexvolumeSlider)
        AlexvolumeSlider.value = PlayerPrefs.GetFloat("alexander-nakarada-superepic");
        if (JoyvolumeSlider)
        JoyvolumeSlider.value = PlayerPrefs.GetFloat("joystock-write-your-story");
    }

    private void Save(){
        if(volumeSlider)
        PlayerPrefs.SetFloat("bgmusic",volumeSlider.value);
        if (AlexvolumeSlider)
        PlayerPrefs.SetFloat("alexander-nakarada-superepic",AlexvolumeSlider.value);
        if (JoyvolumeSlider)
        PlayerPrefs.SetFloat("joystock-write-your-story", JoyvolumeSlider.value);
    }
}
