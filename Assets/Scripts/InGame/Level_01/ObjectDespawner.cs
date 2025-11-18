using UnityEngine;

public class ObjectDespawner : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.LapRestarted += Despawn;
    }

    private void Despawn()
    {
        gameObject.SetActive(false);
    }
}