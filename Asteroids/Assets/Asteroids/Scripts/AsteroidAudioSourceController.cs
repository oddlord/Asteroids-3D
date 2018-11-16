using System.Collections;
using UnityEngine;

public class AsteroidAudioSourceController : MonoBehaviour
{
    #region Private attributes
    private AudioSource audioSource;
    #endregion

    #region OnEnable
    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DisableAudioSource(audioSource.clip.length));
        audioSource.Play();
    }
    #endregion

    #region Disable AudioSource
    private IEnumerator DisableAudioSource(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        gameObject.SetActive(false);
    }
    #endregion
}
