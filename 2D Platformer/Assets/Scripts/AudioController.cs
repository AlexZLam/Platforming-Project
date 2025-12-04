using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider maxSlider;
    public Slider sfxSlider;
    public Slider backSlider;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Load saved volume
        float maxSavedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        maxSlider.value = maxSavedVolume;
        SetMaxVolume(maxSavedVolume);
        float sfxSavedVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        maxSlider.value = sfxSavedVolume;
        SetSFXVolme(sfxSavedVolume);
        float backSavedVolume = PlayerPrefs.GetFloat("BackVolume", 0.75f);
        maxSlider.value = backSavedVolume;
        SetBackVolume(backSavedVolume);
    }

    public void SetMaxVolume(float volume)
    {
        // Convert slider value (0–1) to decibels
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void SetSFXVolme(float volume)
    {
        // Convert slider value (0–1) to decibels
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    public void SetBackVolume(float volume)
    {
        // Convert slider value (0–1) to decibels
        audioMixer.SetFloat("BackVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BackVolume", volume);
    }
}
