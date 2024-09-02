using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class AchievementSlot : MonoBehaviour
{
    [SerializeField]
    private Text achivementText;

    private LocalizedString localizedString = new LocalizedString { TableReference = "LanguageData" };

    private int slotNum;

    public AchievementsManager.Achievements ach;

    void Start()
    {

    }

    public int SlotNum
    {
        get => slotNum;
        set
        {
            slotNum = value;
        }
    }

    public void UpdateSlotUI()
    {
        switch(ach)
        {
            case AchievementsManager.Achievements.kill1:
                localizedString.TableEntryReference = "ach kill1";
                break;

            case AchievementsManager.Achievements.food1:
                localizedString.TableEntryReference = "ach food1";
                break;

            case AchievementsManager.Achievements.cook1:
                localizedString.TableEntryReference = "ach cook1";
                break;

            case AchievementsManager.Achievements.kill10:
                localizedString.TableEntryReference = "ach kill10";
                break;

            case AchievementsManager.Achievements.day3:
                localizedString.TableEntryReference = "ach day3";
                break;

            case AchievementsManager.Achievements.day7:
                localizedString.TableEntryReference = "ach day7";
                break;

            case AchievementsManager.Achievements.specialFood:
                localizedString.TableEntryReference = "ach specialFood";
                break;

            case AchievementsManager.Achievements.sunKill:
                localizedString.TableEntryReference = "ach sunKill";
                break;

            case AchievementsManager.Achievements.safeHouse:
                localizedString.TableEntryReference = "ach safeHouse";
                break;

            case AchievementsManager.Achievements.uiInventory:
                localizedString.TableEntryReference = "ach Ui Inventory";
                break;

            case AchievementsManager.Achievements.uiAchievement:
                localizedString.TableEntryReference = "ach Ui ack";
                break;

            case AchievementsManager.Achievements.uiCooking:
                localizedString.TableEntryReference = "ach Ui Cook";
                break;

            case AchievementsManager.Achievements.uiOption:
                localizedString.TableEntryReference = "ach Ui Option";
                break;

            default:
                localizedString.TableEntryReference = "";
                break;
        }

        localizedString.StringChanged += UpdateText;
    }

    private void UpdateText(string localizedText)
    {
        achivementText.text = localizedText;
    }

    private void OnDestroy()
    {
        localizedString.StringChanged -= UpdateText;
    }
}
