using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{

    // Required for unity to interact with volume sliders
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    //Set Volume if player changed settings previously
    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            LoadVolume();
        }
        else 
        {
            SetVolume();
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadMusicVolume();
        }
        else 
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadSFXVolume();
        }
        else 
        {
            SetSFXVolume();
        }
    }

    // Updates all sliders in real time, defaulting to 50% if no stored preferences were located
    public void SetVolume()
    {
        float volume = volumeSlider.value;
        myMixer.SetFloat("MasterVol", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    private void LoadVolume()
    {
        volumeSlider.value=PlayerPrefs.GetFloat("masterVolume");

        SetVolume();
    }

    public void SetMusicVolume()
    {
        float mvolume = musicSlider.value;
        myMixer.SetFloat("MusicVol", Mathf.Log10(mvolume)*20);
        PlayerPrefs.SetFloat("musicVolume", mvolume);
    }

    private void LoadMusicVolume()
    {
        musicSlider.value=PlayerPrefs.GetFloat("musicVolume");

        SetMusicVolume();
    }
    
    public void SetSFXVolume()
    {
        float svolume = SFXSlider.value;
        myMixer.SetFloat("SFXVol", Mathf.Log10(svolume)*20);
        PlayerPrefs.SetFloat("SFXVolume", svolume);
    }

    private void LoadSFXVolume()
    {
        SFXSlider.value=PlayerPrefs.GetFloat("SFXVolume");

        SetSFXVolume();
    }
}
