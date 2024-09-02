using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerUpHandler
{
    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private GameObject player;

    private int slotNum;

    private Item item;

    public int SlotNum
    {
        get => slotNum;
        set
        {
            slotNum = value;
        }
    }

    public Item Item
    {
        get => item;
        set
        {
            item = value;
        }
    }

    public Image ItemIcon
    {
        get => itemIcon;
        set
        {
            itemIcon = value;
        }
    }

    public void UpdateSlotUI()
    {
        
        itemIcon.sprite = item.itemImage;
        itemIcon.gameObject.SetActive(true);
    }

    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bool isUse = item.Used(player);
        GameManager.Instance.IsInput = false;

        if (isUse)
        {
            Inventory.instance.ReMoveItem(slotNum);
        }
    }
}
