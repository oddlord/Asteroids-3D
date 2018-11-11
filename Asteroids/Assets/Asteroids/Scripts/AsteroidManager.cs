using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    private int size;

    void SetSize(int s)
    {
        transform.localScale *= Mathf.Pow(AsteroidSpawner.Instance.GetFragmentRatio(), 3 - s);
        size = s;
    }

    public int GetSize()
    {
        return size;
    }

    public void Spawn(int s)
    {
        SetSize(s);
    }

    public void Explode()
    {
        AsteroidSpawner.Instance.SpawnFragments(transform.position, size);
        Destroy(this.gameObject);
    }
}
