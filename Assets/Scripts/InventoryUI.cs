using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private Slot[] slots;

    [SerializeField]
    private Transform slotHolder;

    private Inventory inventory;
    
    private bool active = false;

    private BitFlag.UIStateFlags currentUIState = BitFlag.UIStateFlags.none;

    // 슬롯 간 드래그 앤 드롭 기능에 필요 
    private GraphicRaycaster raycaster;
    private PointerEventData eventData;
    private List<RaycastResult> raycastResults;

    private Slot beginDragSlot;
    private Transform beginDragIconTransfrom;

    private Vector3 beginDragIconPoint;   // 드래그 시작 시 슬롯의 위치
    private Vector3 beginDragCursorPoint; // 드래그 시작 시 커서의 위치
    private int beginDragSlotSiblingIndex;
    //

    private Item item;
    private void Start()
    {
        slots = GetComponentsInChildren<Slot>();

        inventory = Inventory.instance;
        inventory.onSlotCountChange += SlotChange;
        inventory.onChangeItem += RedrawSlotUI;
        
        inventoryPanel.SetActive(active);

        StartCoroutine(CheckForKeyIPress());

        DontDestroyOnLoad(gameObject);
    }

    // slotcnt 갯수 만큼만 활성화 
    private void SlotChange(int value)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].SlotNum = i;
            if (i < inventory.SlotCnt)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;

        }
    }
    private void Update()
    {

    }

    private IEnumerator CheckForKeyIPress()
    {
        while (true)
        {
            //if (Input.GetKeyDown(KeyCode.I))
            //{
            //    inventoryPanel.SetActive(inventoryPanel.activeSelf ? false : true);
            //}
            //yield return null;

            if (Input.GetKeyDown(KeyCode.I))
            {
                // inventoryPanel이 활성화될 때의 상태 체크
                if (inventoryPanel.activeSelf is false)
                {
                    inventoryPanel.SetActive(true);

                    // 처음으로 활성화될 때만 플래그 설정
                    if ((currentUIState & BitFlag.UIStateFlags.inventoryActivated) == 0)
                    {
                        currentUIState |= BitFlag.UIStateFlags.inventoryActivated;
                        Debug.Log("Inventory Panel active");
                        //AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.uiInventory,
                        //    inventory: 1);
                        AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.uiInventory, value: 1);


                    }
                }
                else
                {
                    inventoryPanel.SetActive(false);
                }
            }
            yield return null;
        }
    }

    public void ShutdownInventory() 
    {
        inventoryPanel.SetActive(false);
    }

    void RedrawSlotUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();

        }
        for (int i = 0; i < inventory.Items.Count; i++)
        {
            slots[i].Item = inventory.Items[i];
            slots[i].UpdateSlotUI();
        }
    }

    private T RaycastAndGetFirstComponent<T>() where T : Component
    {
        raycastResults.Clear();

        raycaster.Raycast(eventData, raycastResults);

        if (raycastResults.Count == Constants.DEFAULT_NUMBER_0)
            return null;

        return raycastResults[0].gameObject.GetComponent<T>();
    }

    private void OnPointerDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //beginDragSlot = RaycastAndGetFirstComponent<Slot>();
            beginDragSlot = GetComponent<Slot>();

            if (beginDragSlot != null /* 아이템을 가지고 있는 동안만*/)
            {
                beginDragIconTransfrom = beginDragSlot.ItemIcon.transform;
                beginDragIconPoint = beginDragIconTransfrom.position;
                beginDragCursorPoint = Input.mousePosition;

                beginDragSlotSiblingIndex = beginDragSlot.transform.GetSiblingIndex();
                beginDragSlot.transform.SetAsLastSibling();

                
            }
            else
            {
                beginDragSlot = null;
            }
        }
    }

    private void OnPointerDrag()
    {

    }

    private void OnPointerUp()
    {

    }

    private void ShowOrHideItemToolTip()
    {
        
    }

    private void UpdateToolTipUI()
    {
        
    }
}
