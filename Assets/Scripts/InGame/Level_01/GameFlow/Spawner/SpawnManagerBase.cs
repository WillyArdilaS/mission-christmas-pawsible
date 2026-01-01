using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class SpawnManagerBase : MonoBehaviour
{
    // === Spawning ===
    public enum SpanwedType { Obstacle, Powerup, Goal }
    [SerializeField] private SpanwedType spanwedType;
    private TrackManager trackManager;
    [SerializeField] private float spawnRate;
    [SerializeField] private float globalSpeed;

    // === Pools ===
    [SerializeField] private GameObject[] objectPools;

    // === Properties ===
    public float SpawnRate { get => spawnRate; set => spawnRate = value; }
    public float GlobalSpeed { get => globalSpeed; set => globalSpeed = value; }

    void Start()
    {
        trackManager = LevelManager1.instance.TrackManager;
    }

    // === Spawning Methods ===
    public void SpawnObject(int amountToSpawn)
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            SpawnSingle();
        }
    }

    private void SpawnSingle()
    {
        GameObject randomObjectPool = objectPools[Random.Range(0, objectPools.Length)];
        GameObject objectSpawned = randomObjectPool.GetComponent<ObjectPool>().GetObjectFromPool();
        int trackIndex;

        if (spanwedType != SpanwedType.Goal)
        {
            trackIndex = trackManager.GetAvailableTrackIndex();

            if (trackIndex == -1)
            {
                Debug.LogWarning("No hay carriles disponibles para spawnear");
                return;
            }
        }
        else
        {
            trackIndex = trackManager.Tracks.Count() / 2;
        }

        trackManager.SetTrackAvailable(trackIndex, false);
        InitializeNewFollower(objectSpawned, trackIndex);
    }

    private void InitializeNewFollower(GameObject obstacle, int trackIndex)
    {
        if (!obstacle.TryGetComponent<SplineFollower>(out var splineFollower))
        {
            Debug.LogError("El objeto instanciado no tiene un SplineFollower");
            return;
        }

        SplineContainer spline = trackManager.GetTrack(trackIndex);

        obstacle.transform.position = spline.EvaluatePosition(0f); // Position at the start point of the spline
        splineFollower.InitializeFollower(spline, globalSpeed);

        StartCoroutine(ReleaseTrackNextFrame(trackIndex));
    }

    private IEnumerator ReleaseTrackNextFrame(int index)
    {
        yield return new WaitForSeconds(1);
        trackManager.SetTrackAvailable(index, true);
    }
}