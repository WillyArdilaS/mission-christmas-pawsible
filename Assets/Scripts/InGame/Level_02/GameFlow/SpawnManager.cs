using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // === Spawning ===
    [SerializeField] private float minXPos;
    [SerializeField] private float maxXPos;
    [SerializeField] private float spawnRate;

    // === Pools ===
    [SerializeField] private GameObject[] fruitPools;
    [SerializeField] private GameObject coalPool;

    // === Properties ===
    public float SpawnRate { get => spawnRate; set => spawnRate = value; }

    public void SpawnFruit()
    {
        GameObject randomFruitPool = fruitPools[Random.Range(0, fruitPools.Length)];
        GameObject fruitSpawned = randomFruitPool.GetComponent<ObjectPool>().GetObjectFromPool();
        float randomXPos = Random.Range(minXPos, maxXPos);

        fruitSpawned.transform.position = new Vector3(randomXPos, transform.position.y , transform.position.z);
    }
    
    public void SpawnCoal()
    {
        GameObject coalSpawned = coalPool.GetComponent<ObjectPool>().GetObjectFromPool();
        float randomXPos = Random.Range(minXPos, maxXPos);

        coalSpawned.transform.position = new Vector3(randomXPos, transform.position.y , transform.position.z);
    }
}