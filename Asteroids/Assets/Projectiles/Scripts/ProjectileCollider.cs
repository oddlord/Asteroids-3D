using UnityEngine;

[RequireComponent(typeof(ProjectileController))]
public class ProjectileCollider: MonoBehaviour
{
    #region Private attributes
    ProjectileController projectileController;
    #endregion

    #region Start
    private void Start()
    {
        projectileController = GetComponent<ProjectileController>();
    }
    #endregion

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameManager.Instance.GetAsteroidTag())
        {
            projectileController.AsteroidHit();
        }
    }
    #endregion
}
