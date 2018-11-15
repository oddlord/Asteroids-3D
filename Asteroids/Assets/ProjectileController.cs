using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        StartCoroutine("WaitToDeactivateProjectile");
    }
    #endregion

    #region Projectile deactivation
    IEnumerator WaitToDeactivateProjectile()
    {
        yield return new WaitForSeconds(projectileLife);
        gameObject.SetActive(false);
    }

    public void AsteroidHit()
    {
        CancelInvoke("WaitToDeactivateProjectile");
        gameObject.SetActive(false);
    }
    #endregion
}
