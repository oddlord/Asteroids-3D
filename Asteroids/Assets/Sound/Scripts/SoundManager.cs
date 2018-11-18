using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    #region Singleton pattern
    private static SoundManager instance;

    public static SoundManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    #region SerializeField attributes
    [Header("Music Tracks")]
    [SerializeField]
    private AudioClip menuTrack;
    [SerializeField]
    private List<AudioClip> gameTracks;

    [Header("Initial volumes")]
    [SerializeField]
    private float initialMusicVolume = 0.4f;
    [SerializeField]
    private float initialSoundEffectsVolume = 0.5f;

    [Header("SFX Preview")]
    [SerializeField]
    private AudioSource sfxPreviewAudioSource;
    [SerializeField]
    private float sfxPreviewTimeInterval = 0.5f;
    #endregion

    #region Private attributes
    private AudioSource musicAudioSource;
    private float musicVolume;
    private float sfxVolume;
    private float previousMusicVolume;
    private float previousSFXVolume;
    private float lastSFXPreviewPlayed;
    #endregion

    #region Utility functions
    private void InitAudioManager()
    {
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.loop = true;

        SetMusicVolume(initialMusicVolume);
        SetSFXVolume(initialSoundEffectsVolume);
        SaveVolumes();

        musicAudioSource.volume = musicVolume;

        lastSFXPreviewPlayed = -sfxPreviewTimeInterval;
    }
    #endregion

    #region Play audio
    public void PlayMenuTrack()
    {
        if (musicAudioSource == null)
        {
            InitAudioManager();
        }

        musicAudioSource.clip = menuTrack;
        musicAudioSource.Play();
    }

    public void PlayRandomGameTrack()
    {
        if (musicAudioSource == null)
        {
            InitAudioManager();
        }

        int randomTrackIdx = Random.Range(0, gameTracks.Count);
        musicAudioSource.clip = gameTracks[randomTrackIdx];
        musicAudioSource.Play();
    }

    public void PlaySFX(AudioSource audioSource)
    {
        audioSource.volume = SoundManager.Instance.GetSFXVolume();
        audioSource.Play();
    }
    #endregion

    #region Getters
    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }
    #endregion

    #region Volumes setters
    public void SetMusicVolumeSlider(Slider slider)
    {
        SetMusicVolume(slider.value);
    }

    public void SetSFXVolumeSlider(Slider slider)
    {
        SetSFXVolume(slider.value);
        if (!UIManager.Instance.IsInitialSFXSLiderSet())
        {
            if (sfxPreviewTimeInterval <= Time.time - lastSFXPreviewPlayed)
            {
                PlaySFX(sfxPreviewAudioSource);
                lastSFXPreviewPlayed = Time.time;
            }
        }
        else
        {
            UIManager.Instance.DisableInitialSFXSLiderSet();
        }
    }

    private void SetMusicVolume(float musicVolume)
    {
        this.musicVolume = Mathf.Clamp01(musicVolume);
        musicAudioSource.volume = this.musicVolume;
    }

    private void SetSFXVolume(float sfxVolume)
    {
        this.sfxVolume = Mathf.Clamp01(sfxVolume);
    }

    public void RollbackVolumes()
    {
        SetMusicVolume(previousMusicVolume);
        SetSFXVolume(previousSFXVolume);
    }

    public void SaveVolumes()
    {
        previousMusicVolume = musicVolume;
        previousSFXVolume = sfxVolume;
    }
    #endregion
}
