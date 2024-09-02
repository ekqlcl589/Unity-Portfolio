using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeHouse : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.safeHouse,
            //    safeHouse: 1);

            AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.safeHouse, value: 1);

            GameManager.Instance.SafeHouse = true;

            UIManager.Instance.DayText.text = "생존 성공";
        }
    }
}
