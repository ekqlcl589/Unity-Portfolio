using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour
{
    private int achievementSlot;

    private List<AchievementsManager.Achievements> achievements = new List<AchievementsManager.Achievements>();

    public static Achievement instance;

    public delegate void OnSlotCountChange(int value);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeAch();
    public OnChangeAch onChangeAch;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        achievementSlot = Constants.DEFAULT_INVENTORY_SLOT;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<AchievementsManager.Achievements> Achievements
    {
        get => achievements;
        set
        {
            if (instance is null)
                return;

            achievements = value;
            onSlotCountChange.Invoke(achievementSlot);

        }
    }

    public int AchievementSlot
    {
        get => achievementSlot;
        set
        {
            if (instance is null)
                return;

            achievementSlot = value;
            onSlotCountChange.Invoke(achievementSlot);
        }
    }

    public bool Addachievement(AchievementsManager.Achievements ach)
    {
        if (achievements.Count < AchievementSlot)
        {
            achievements.Add(ach);
            if (onChangeAch != null)
                onChangeAch.Invoke();
            return true;
        }
        return false;
    }
}
