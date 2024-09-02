using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AchievementUI : MonoBehaviour
{
    [SerializeField]
    private GameObject AchievementPanel;

    [SerializeField]
    private AchievementSlot[] slots;

    private Achievement achievement;

    private bool active = false;

    private BitFlag.UIStateFlags currentUIState = BitFlag.UIStateFlags.none;

    // Start is called before the first frame update
    void Start()
    {
        slots = GetComponentsInChildren<AchievementSlot>();

        achievement = Achievement.instance;
        achievement.onSlotCountChange += SlotChange;
        achievement.onChangeAch += RedrawSlotUI;

        AchievementPanel.SetActive(active);

        StartCoroutine(CheckForKeyJPress());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator CheckForKeyJPress()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (AchievementPanel.activeSelf is false)
                {
                    AchievementPanel.SetActive(true);

                    if ((currentUIState & BitFlag.UIStateFlags.achActivated) == 0)
                    {
                        currentUIState |= BitFlag.UIStateFlags.achActivated;

                        AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.uiAchievement, value: 1);
                    }
                }
                else
                {
                    AchievementPanel.SetActive(false);
                }
            }
            yield return null;
        }
    }
    private void SlotChange(int value)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SlotNum = i;
            if (i < achievement.AchievementSlot)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;

        }
    }

    private void RedrawSlotUI()
    {
        for(int i = 0; i < achievement.Achievements.Count; i++)
        {
            slots[i].ach = achievement.Achievements[i];
            slots[i].UpdateSlotUI();
        }
    }
}
