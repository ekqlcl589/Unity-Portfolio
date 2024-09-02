using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private AudioClip pickUpClip;

    private AudioSource playerAudioPlayer;

    private int foodCnt = Constants.DEFAULT_NUMBER_0;

    private List<Item> items = new List<Item>();

    private int slotCnt;

    public delegate void OnSlotCountChange(int value);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public delegate void OnDeleteItem(int value);
    public OnDeleteItem onDeleteItem;

    private static Inventory instance;
    public List<Item> Items
    {
        get => items;
        set
        {
            items = value;
            onSlotCountChange.Invoke(slotCnt);
        }
    }

    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            onSlotCountChange.Invoke(slotCnt);
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        playerAudioPlayer = GetComponent<AudioSource>();

    }

    // Start is called before the first frame update
    void Start()
    {
        slotCnt = Constants.MAX_INVENTORY_SLOT; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddItem(Item _item)
    {
        if(items.Count < SlotCnt)
        {
            items.Add(_item);
            if(onChangeItem != null)
            onChangeItem.Invoke();
            return true;
        }
        return false;
    }

    public void ReMoveItem(int _index)
    {
        items.RemoveAt(_index);
        onChangeItem.Invoke();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("FieldItem"))
        {
            playerAudioPlayer.PlayOneShot(pickUpClip);
            FiledItems filedItems = other.GetComponent<FiledItems>();

            if(filedItems.GetItem().itemName != "Coin" && filedItems.GetItem().itemName != "치즈케이크")
                AddFoodCount();

            if(filedItems.GetItem().itemName == "치즈케이크")
            {
                //AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.specialFood,
                //    special: 1);
                AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.specialFood, value: 1);

            }

            if (AddItem(filedItems.GetItem()))
                filedItems.DestroyItem();
        }
    }

    void AddFoodCount()
    {
        foodCnt++;

        //AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.food1,
        //    food: foodCnt);

        AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.food1, value: foodCnt);

    }
}
