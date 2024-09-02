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
        rect.pivot = new Vector2(0f, 1f); // ����Ʈ ž
        canvasScaler = GetComponentInParent<CanvasScaler>();

        DisableAllChildrenRaycastTarget(transform);
    }

    private void DisableAllChildrenRaycastTarget(Transform tr)
    {
        // ������ �׷���(ui)�� ����ϸ� ����ĳ��Ʈ Ÿ�� ����
        rect.TryGetComponent(out Graphic graphic);
        if (graphic != null)
            graphic.raycastTarget = false;

        // �ڽ��� ������ ����
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
