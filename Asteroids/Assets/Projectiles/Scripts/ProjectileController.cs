using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProjectileController : MonoBehaviour
{
    #region SerializeField attributes
    [Header("Projectile properties")]
    [SerializeField]
    private float projectileLife = 1f;
    #endregion

    #region OnEnable
    private void OnEnable()
    {
        StartCoroutine(WaitToDeactivateProjectile());

        AudioSource audioSource = GetComponent<AudioSource>();
        SoundManager.Instance.PlaySFX(audioSource);
    }
    #endregion

    #region Projectile deactivation
    IEnumerator WaitToDeactivateProjectile()
    {
        yield return new WaitForSeconds(projectileLife);
        PoolsManager.Instance.GetProjectilesPool().SetAvailable(gameObject);
    }

    public void AsteroidHit()
    {
        CancelInvoke("WaitToDeactivateProjectile");
        PoolsManager.Instance.GetProjectilesPool().SetAvailable(gameObject);
    }
    #endregion
}
