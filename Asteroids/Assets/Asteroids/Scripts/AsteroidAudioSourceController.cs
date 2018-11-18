using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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
        SoundManager.Instance.PlaySFX(audioSource);
    }
    #endregion

    #region Disable AudioSource
    private IEnumerator DisableAudioSource(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        PoolsManager.Instance.GetAsteroidAudioSourcesPool().SetAvailable(gameObject);
    }
    #endregion
}
