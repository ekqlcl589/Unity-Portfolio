using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CookingSlot : MonoBehaviour, IPointerUpHandler
{
    [SerializeField]
    private Slider slider;

    private float maxdata = Constants.COOK_MAX_GAUGE;
    private float startingdata = Constants.COOK_START_HAUGE; // 시작 요리 게이지

    private bool isCooking = false;

    public float cook { get; protected set; } // 현재 요리 게이지

    public bool IsCooking { get; set; }

    void Start()
    {
        
    }

    void Update()
    {
        if (isCooking is true)
            Cooking();
    }

    void OnEnable()
    {
        cook = startingdata;
        slider.gameObject.SetActive(false);
        slider.maxValue = maxdata;
        slider.value = cook;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (CheckItem())
        {
            StartCooking();
        }
        else
        {
            Debug.Log("Item check failed.");
        }
    }

    private bool CheckItem()
    {
        return Inventory.instance.Items.Count >= 1;
    }

    private void StartCooking()
    {
        slider.gameObject.SetActive(true);
        Debug.Log("Cooking started");
        isCooking = true;
    }

    private void Cooking()
    {
        cook += Time.time * 0.05f;
        slider.value = cook;

        if (cook >= maxdata)
        {
            FinishCooking();
        }
    }

    private void FinishCooking()
    {
        cook = 0f;
        slider.value = cook;

        Debug.Log("Cooking completed");

        slider.gameObject.SetActive(false);
        IsCooking = true;
        isCooking = false;
    }
}
