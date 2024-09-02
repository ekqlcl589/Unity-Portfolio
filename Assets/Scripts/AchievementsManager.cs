using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AchievementsManager : MonoBehaviour
{
    private static AchievementsManager _instance;

    private Dictionary<Achievements, Action<int>> achievementActions;

    private Dictionary<Achievements, bool> _dicAchievementUnlock =
            new Dictionary<Achievements, bool>(new AchievementsComparer());

    private Text _achievementText;

    private class AchievementsComparer : IEqualityComparer<Achievements>
    {
        public bool Equals(Achievements a, Achievements b)
        {
            return a == b;
        }
        public int GetHashCode(Achievements obj)
        {
            return ((int)obj).GetHashCode();
        }
    }

    [Flags]
    public enum Achievements
    {
        none = 0,
        kill1 = 1 << 0,
        food1 = 1 << 1,
        cook1 = 1 << 2,
        kill10 = 1 << 3,
        day3 = 1 << 4,
        day7 = 1 << 5,
        specialFood = 1 << 6,
        sunKill = 1 << 7,
        safeHouse = 1 << 8,
        uiInventory = 1 << 9,
        uiAchievement = 1 << 10,
        uiCooking = 1 << 11,
        uiOption = 1 << 12,
    }

    public AchievementsManager()
    {
        foreach (Achievements achv in Enum.GetValues(typeof(Achievements)))
        {
            _dicAchievementUnlock[achv] = false;
        }

        achievementActions = new Dictionary<Achievements, Action<int>>
        {
            { Achievements.kill1, UnlockKill1 },
            { Achievements.food1, UnlockFood },
            { Achievements.cook1, UnlockCook },
            { Achievements.kill10, UnlockKill10 },
            { Achievements.day3, UnlockAlive3 },
            { Achievements.day7, UnlockAlive7 },
            { Achievements.specialFood, UnlockSpecialFood },
            { Achievements.sunKill, UnlockSunKill },
            { Achievements.safeHouse, UnlockSafeHouse },
            { Achievements.uiInventory, UnlockInventoryUi },
            { Achievements.uiCooking, UnlockCookUi },
            { Achievements.uiAchievement, UnlockAchUi },
            { Achievements.uiOption, UnlockOptionUi }
        };
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public Text achievementText
    {
        get
        {
            if (_achievementText == null)
            {
                GameObject obj = GameObject.Find("HUD Canvas/Test");
                if (obj != null)
                {
                    _achievementText = obj.GetComponent<Text>();
                }
            }
            return _achievementText;
        }
    }

    void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        _achievementText = null;
    }

    public static AchievementsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("AchievementsManager");
                _instance = obj.AddComponent<AchievementsManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    public void OnNotify(Achievements achievements, int value = 0)
    {
        foreach (var achievement in achievementActions)
        {
            if (achievements.HasFlag(achievement.Key))
            {
                achievement.Value(value);
            }
        }
    }

    private void UnlockKill1(int toKill)
    {
        if (_dicAchievementUnlock[Achievements.kill1])
            return;

        if (toKill >= Constants.DEFAULT_NUMBER_3)
        {
            StartCoroutine(Cor_ShowText5Sec("업적 달성! \n 좀비 사냥꾼 \n"));
            _dicAchievementUnlock[Achievements.kill1] = true;
            Achievement.instance.Addachievement(Achievements.kill1);
        }
    }

    private void UnlockFood(int food)
    {
        if (_dicAchievementUnlock[Achievements.food1])
            return;

        if (food >= Constants.DEFAULT_NUMBER_1)
        {
            StartCoroutine(Cor_ShowText5Sec("업적 달성! \n 최초의 식량 \n"));
            _dicAchievementUnlock[Achievements.food1] = true;
            Achievement.instance.Addachievement(Achievements.food1);
        }
    }

    private void UnlockCook(int cook)
    {
        if (_dicAchievementUnlock[Achievements.cook1])
            return;

        if (cook >= Constants.DEFAULT_NUMBER_1)
        {
            StartCoroutine(Cor_ShowText5Sec("업적 달성! \n 요리사 등장 \n"));
            _dicAchievementUnlock[Achievements.cook1] = true;
            Achievement.instance.Addachievement(Achievements.cook1);
        }
    }

    private void UnlockKill10(int kill)
    {
        if (_dicAchievementUnlock[Achievements.kill10])
            return;

        if (kill >= Constants.DEFAULT_NUMBER_10)
        {
            StartCoroutine(Cor_ShowText5Sec("업적 달성! \n 좀비 학살자 \n"));
            _dicAchievementUnlock[Achievements.kill10] = true;
            Achievement.instance.Addachievement(Achievements.kill10);
        }
    }

    private void UnlockAlive3(int day)
    {
        if (_dicAchievementUnlock[Achievements.day3])
            return;

        if (day >= Constants.DEFAULT_NUMBER_3)
        {
            StartCoroutine(Cor_ShowText5Sec("업적 달성! \n 생존가 \n"));
            _dicAchievementUnlock[Achievements.day3] = true;
            Achievement.instance.Addachievement(Achievements.day3);
        }

    }
    private void UnlockAlive7(int day)
    {
        if (_dicAchievementUnlock[Achievements.day7])
            return;

        if (day >= Constants.DEFAULT_NUMBER_7)
        {
            StartCoroutine(Cor_ShowText5Sec("업적 달성! \n 생활의 달인 \n"));
            _dicAchievementUnlock[Achievements.day7] = true;
            Achievement.instance.Addachievement(Achievements.day7);
        }

    }

    private void UnlockSpecialFood(int food)
    {
        if (_dicAchievementUnlock[Achievements.specialFood])
            return;

        if (food >= Constants.DEFAULT_NUMBER_1)
        {
            StartCoroutine(Cor_ShowText5Sec("업적 달성! \n 완벽한 음식..? \n"));
            _dicAchievementUnlock[Achievements.specialFood] = true;
            Achievement.instance.Addachievement(Achievements.specialFood);
        }

    }

    private void UnlockSunKill(int kill)
    {
        if (_dicAchievementUnlock[Achievements.sunKill])
            return;

        if (kill >= Constants.DEFAULT_NUMBER_1)
        {
            StartCoroutine(Cor_ShowText5Sec("업적 달성! \n 돋보기 실험 \n"));
            _dicAchievementUnlock[Achievements.sunKill] = true;
            Achievement.instance.Addachievement(Achievements.sunKill);
        }
    }

    private void UnlockSafeHouse(int safe)
    {
        if (_dicAchievementUnlock[Achievements.safeHouse])
            return;

        if (safe >= Constants.DEFAULT_NUMBER_1)
        {
            StartCoroutine(Cor_ShowText5Sec("업적 달성! \n 은신처 도착\n"));
            _dicAchievementUnlock[Achievements.safeHouse] = true;
            Achievement.instance.Addachievement(Achievements.safeHouse);
        }

    }

    private void UnlockInventoryUi(int inventory)
    {
        if (_dicAchievementUnlock[Achievements.uiInventory])
            return;

        if( inventory >= Constants.DEFAULT_NUMBER_1)
        {
            StartCoroutine(Cor_ShowText5Sec("업적 달성! \n 인벤토리 최초 오픈!"));
            _dicAchievementUnlock[Achievements.uiInventory] = true;
            Achievement.instance.Addachievement(Achievements.uiInventory);
        }
    }

    private void UnlockAchUi(int ach)
    {
        if (_dicAchievementUnlock[Achievements.uiAchievement])
            return;

        if (ach >= Constants.DEFAULT_NUMBER_1)
        {
            StartCoroutine(Cor_ShowText5Sec("업적 달성! \n 업적 ui 최초 오픈!"));
            _dicAchievementUnlock[Achievements.uiAchievement] = true;
            Achievement.instance.Addachievement(Achievements.uiAchievement);
        }
    }

    private void UnlockCookUi(int cook)
    {
        if (_dicAchievementUnlock[Achievements.uiCooking])
            return;

        if (cook >= Constants.DEFAULT_NUMBER_1)
        {
            StartCoroutine(Cor_ShowText5Sec("업적 달성! \n 요리 ui 최초 오픈!"));
            _dicAchievementUnlock[Achievements.uiCooking] = true;
            Achievement.instance.Addachievement(Achievements.uiCooking);
        }
    }

    private void UnlockOptionUi(int option)
    {
        if (_dicAchievementUnlock[Achievements.uiOption])
            return;

        if (option >= Constants.DEFAULT_NUMBER_1)
        {
            StartCoroutine(Cor_ShowText5Sec("업적 달성! \n 옵션 ui 최초 오픈!"));
            _dicAchievementUnlock[Achievements.uiOption] = true;
            Achievement.instance.Addachievement(Achievements.uiOption);
        }
    }

    private IEnumerator Cor_ShowText5Sec(string text)
    {
        achievementText.text += text;
        yield return new WaitForSeconds(5f);
        achievementText.text = string.Empty;
    }
}
