using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectDespawner : MonoBehaviour
{
    [SerializeField] private float minYPos;

    void Start()
    {
        if(GameManagerLevel1.instance != null) GameManagerLevel1.instance.LapRestarted += Despawn;
    }

    void Update()
    {
        if(transform.position.y < minYPos) Despawn();
    }

    private void Despawn()
    {
        gameObject.SetActive(false);
    }
}