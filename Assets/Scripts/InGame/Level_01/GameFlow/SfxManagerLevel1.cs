using UnityEngine;

public class SfxManagerLevel1 : MonoBehaviour
{
    // === Managers ===
    [SerializeField] private CollisionManagerLevel1 collisionManager;
    [SerializeField] private PowerupManager powerupManager;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameManager.instance.AudioManager;

        collisionManager.HitObstacle += PlayHitObstacleSfx;
        collisionManager.FinishLineCrossed += PlayFinishLineSfx;
        powerupManager.PowerupPickedUp += PlayPowerupSfx;
    }

    void OnDestroy()
    {
        collisionManager.HitObstacle -= PlayHitObstacleSfx;
        collisionManager.FinishLineCrossed -= PlayFinishLineSfx;
        powerupManager.PowerupPickedUp -= PlayPowerupSfx;
    }

    private void PlayHitObstacleSfx()
    {
        audioManager.PlaySFX(audioManager.SfxDictionary["HitObstacle"], audioManager.HitObstacleVol);
    }

    private void PlayFinishLineSfx()
    {
        if (LevelManager1.instance.CurrentLap == 1)
        {
            audioManager.PlaySFX(audioManager.SfxDictionary["RaceStart"], audioManager.RaceStartVol);
        }
        else
        {
            audioManager.PlaySFX(audioManager.SfxDictionary["LapUpdated"], audioManager.LapUpdatedVol);
        }
    }

    private void PlayPowerupSfx()
    {
        audioManager.PlaySFX(audioManager.SfxDictionary["Powerup"], audioManager.PowerupVol);
    }
}