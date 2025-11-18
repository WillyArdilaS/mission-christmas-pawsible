using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CollisionManager), typeof(PowerupManager))]
public class LifeManager : MonoBehaviour
{
    // === Collision ===
    private CollisionManager collisionManager;
    private PowerupManager powerupManager;

    // === Lives ===
    [SerializeField] private GameObject[] livesUI;
    private int lifeCounter;

    // === Properties ===
    public int LifeCounter => lifeCounter;

    void Awake()
    {
        collisionManager = GetComponent<CollisionManager>();
        powerupManager = GetComponent<PowerupManager>();

        collisionManager.ObstacleHit += SubstractLife;

        lifeCounter = livesUI.Count();
    }

    public void AddLife()
    {
        livesUI.FirstOrDefault(life => life.activeSelf == false).SetActive(true);
        lifeCounter++;
    }

    private void SubstractLife()
    {
        if (lifeCounter > 0)
        {
            GameObject lostLife = livesUI.LastOrDefault(life => life.activeSelf == true);
            lostLife.SetActive(false);
            lifeCounter--;

            StartCoroutine(powerupManager.StartInvincibility(0.5f, 0.5f));
        }
    }

    public void ResetLives()
    {
        foreach (var life in livesUI)
        {
            life.SetActive(true);
        }

        lifeCounter = livesUI.Count();
    }
}