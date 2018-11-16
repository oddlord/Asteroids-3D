using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    #region SerializeField attributes
    [Header("Music Tracks")]
    [SerializeField]
    private List<AudioClip> gameTracks;
    #endregion

    #region Private attributes
    private AudioSource audioSource;
    #endregion
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        PlayRandomGameTrack();
    }
	
	private void PlayRandomGameTrack()
    {
        int randomTrackIdx = Random.Range(0, gameTracks.Count);
        audioSource.clip = gameTracks[randomTrackIdx];
        audioSource.Play();
    }
}
