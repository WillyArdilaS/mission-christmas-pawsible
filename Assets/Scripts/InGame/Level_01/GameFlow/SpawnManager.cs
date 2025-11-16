using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class SpawnManager : MonoBehaviour
{
    // === Spawning ===
    public enum SpanwedType { Obstacle, Powerup, Goal }
    [SerializeField] SpanwedType spanwedType;
    private TrackManager trackManager;
    [SerializeField] private float globalSpeed;

    // === Pools ===
    [SerializeField] private GameObject[] obstaclePools;
    [SerializeField] GameObject[] powerupPools;
    [SerializeField] GameObject goalPool;

    void Awake()
    {
        trackManager = transform.parent.Find("TrackManager").GetComponent<TrackManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (spanwedType)
            {
                case SpanwedType.Obstacle:
                    SpawnObstacle();
                    break;

                case SpanwedType.Powerup:
                    SpawnPowerup();
                    break;

                case SpanwedType.Goal:
                    SpawnGoal();
                    break;
            }
        }
    }

    // === Spawning methods ===

    public void SpawnObstacle()
    {
        GameObject randomObstaclePool = obstaclePools[Random.Range(0, obstaclePools.Length)];
        GameObject obstacleSpawned = randomObstaclePool.GetComponent<ObjectPool>().GetObjectFromPool();
        int trackIndex = Random.Range(0, trackManager.Tracks.Count());

        InitializeNewFollower(obstacleSpawned, trackIndex);
    }

    public void SpawnPowerup()
    {
        GameObject randomPowerupPool = powerupPools[Random.Range(0, powerupPools.Length)];
        GameObject powerupSpawned = randomPowerupPool.GetComponent<ObjectPool>().GetObjectFromPool();
        int trackIndex = Random.Range(0, trackManager.Tracks.Count());

        InitializeNewFollower(powerupSpawned, trackIndex);
    }

    public void SpawnGoal()
    {
        GameObject goalSpawned = goalPool.GetComponent<ObjectPool>().GetObjectFromPool();
        int trackIndex = trackManager.Tracks.Count() / 2;

        InitializeNewFollower(goalSpawned, trackIndex);
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
    }
}