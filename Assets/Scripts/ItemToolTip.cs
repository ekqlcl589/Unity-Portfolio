using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Text contentText;

    private RectTransform rect;
    private CanvasScaler canvasScaler;

    // Start is called before the first frame update
    void Awake()
    {
        //Init();
        Hide();
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
    // Update is called once per frame

    private void Init()
    {
        TryGetComponent(out rect);
        rect.pivot = new Vector2(0f, 1f); // 레프트 탑
        canvasScaler = GetComponentInParent<CanvasScaler>();

        DisableAllChildrenRaycastTarget(transform);
    }

    private void DisableAllChildrenRaycastTarget(Transform tr)
    {
        // 본인이 그래픽(ui)를 상속하면 레이캐스트 타켓 해제
        rect.TryGetComponent(out Graphic graphic);
        if (graphic != null)
            graphic.raycastTarget = false;

        // 자식이 없으면 종료
        int childCnt = rect.childCount;
        if (childCnt == 0)
            return;

        for (int i = 0; i < childCnt; i++)
            DisableAllChildrenRaycastTarget(rect.GetChild(i));
    }

    public void SetItemInfo(Item item)
    {
        titleText.text = item.itemName;
        contentText.text = item.itemToolTip;
    }
}
