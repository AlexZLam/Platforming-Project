using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Background Music Source")]
    public AudioSource backgroundSource;

    [Header("Sliders (only in first scene)")]
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider backgroundSlider;

    // Internal values that persist across scenes
    private float masterVolume = 1f;
    private float sfxVolume = 1f;
    private float backgroundVolume = 1f;

    private List<AudioSource> sfxSources = new List<AudioSource>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Load saved values (from PlayerPrefs if available)
            masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            backgroundVolume = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindSFXSources();
        StartCoroutine(DelayedSFXScan(2f));

        if (masterSlider != null) masterSlider.value = masterVolume;
        if (sfxSlider != null) sfxSlider.value = sfxVolume;
        if (backgroundSlider != null) backgroundSlider.value = backgroundVolume;

        UpdateVolumes();
    }

    private IEnumerator DelayedSFXScan(float delay)
    {
        yield return new WaitForSeconds(delay);
        FindSFXSources();
    }




    private void FindSFXSources()
    {
        sfxSources.Clear();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            AudioSource src = obj.GetComponent<AudioSource>();
            if (src != null) sfxSources.Add(src);
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            AudioSource src = obj.GetComponent<AudioSource>();
            if (src != null) sfxSources.Add(src);
        }
    }

    void Update()
    {
        // If sliders exist (first scene), update values from them
        if (masterSlider != null) masterVolume = masterSlider.value;
        if (sfxSlider != null) sfxVolume = sfxSlider.value;
        if (backgroundSlider != null) backgroundVolume = backgroundSlider.value;
        
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        if (backgroundSource != null)
            backgroundSource.volume = backgroundVolume * masterVolume;

        foreach (AudioSource src in sfxSources)
        {
            src.volume = sfxVolume * masterVolume;
        }

        // Save values so they persist across scenes
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("BackgroundVolume", backgroundVolume);
        PlayerPrefs.Save();
    }
}
