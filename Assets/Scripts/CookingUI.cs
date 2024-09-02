using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CookingUI : MonoBehaviour
{
    [SerializeField]
    private Slot[] slots;

    [SerializeField]
    private CookingSlot[] cookSlots;

    [SerializeField]
    private Item[] item;

    private int cookCnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (slots is null)
            return;

        Cook();
    }

    private void Cook()
    {
        for (int i = 0; i < cookSlots.Length; i++)
        {
            if (cookSlots[i].IsCooking)
            {
                ProcessCooking(i, item[i]);
                return;
            }
        }
    }

    private void ProcessCooking(int cookSlotIndex, Item newItem)
    {
        if (cookSlots[cookSlotIndex] is null || newItem is null)
        {
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item.itemName == newItem.itemName)
            {
                slots[i].Item = newItem;
                slots[i].UpdateSlotUI();
                cookSlots[cookSlotIndex].IsCooking = false;
                AddCookCount();
                return;
            }
        }

        // 요리할 아이템이 없으면 요리 상태를 해제
        cookSlots[cookSlotIndex].IsCooking = false;
    }

    private void AddCookCount()
    {
        cookCnt++;

        AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.cook1, value: cookCnt);
    }
}
