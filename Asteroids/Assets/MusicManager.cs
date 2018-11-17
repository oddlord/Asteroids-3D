using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    #region Singleton pattern
    private static MusicManager instance;

    public static MusicManager Instance { get { return instance; } }

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
    #endregion

    #region Private attributes
    private AudioSource audioSource;
    #endregion

    #region Utility functions
    private void InitAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }
    #endregion

    #region Play tracks
    public void PlayMenuTrack()
    {
        if (audioSource == null)
        {
            InitAudioSource();
        }

        audioSource.clip = menuTrack;
        audioSource.Play();
    }

    public void PlayRandomGameTrack()
    {
        if (audioSource == null)
        {
            InitAudioSource();
        }

        int randomTrackIdx = Random.Range(0, gameTracks.Count);
        audioSource.clip = gameTracks[randomTrackIdx];
        audioSource.Play();
    }
    #endregion
}
