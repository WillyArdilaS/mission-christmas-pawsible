using UnityEngine;

public class ObjectDespawner : MonoBehaviour
{
    void Start()
    {
        GameManagerLevel1.instance.LapRestarted += Despawn;
    }

    private void Despawn()
    {
        gameObject.SetActive(false);
    }
}