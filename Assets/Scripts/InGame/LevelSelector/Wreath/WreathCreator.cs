using UnityEngine;

public class WreathCreator : MonoBehaviour
{
    // === Icon Changers ===
    private IconChanger bowChanger;
    private IconChanger fruitsChanger;
    private IconChanger lightsChanger;

    void Start()
    {
        bowChanger = transform.Find("Bow").GetComponent<IconChanger>();
        fruitsChanger = transform.Find("Fruits").GetComponent<IconChanger>();
        lightsChanger = transform.Find("Lights").GetComponent<IconChanger>();

        SelectIcons();
    }

    private void SelectIcons()
    {
        switch (LevelSelectorManager.nextLevel)
        {
            case LevelSelectorManager.NextLevel.Level_01:
                bowChanger.ChangeIcon("Available");
                fruitsChanger.ChangeIcon("NotAvailable");
                lightsChanger.ChangeIcon("NotAvailable");
                break;

            case LevelSelectorManager.NextLevel.Level_02:
                bowChanger.ChangeIcon("Completed");
                fruitsChanger.ChangeIcon("Available");
                lightsChanger.ChangeIcon("NotAvailable");
                break;

            case LevelSelectorManager.NextLevel.Level_03:
                bowChanger.ChangeIcon("Completed");
                fruitsChanger.ChangeIcon("Completed");
                lightsChanger.ChangeIcon("Available");
                break;

            case LevelSelectorManager.NextLevel.Finished:
                bowChanger.ChangeIcon("Completed");
                lightsChanger.ChangeIcon("Completed");
                fruitsChanger.ChangeIcon("Completed");
                break;
        }
    }
}