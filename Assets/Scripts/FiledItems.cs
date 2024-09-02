using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiledItems : MonoBehaviour
{
    [SerializeField]
    private Item item;
    //public SpriteRenderer image;

    public void SetItem(Item _item)
    {
        item.itemName = _item.itemName;
        item.itemImage = _item.itemImage;
        item.type = _item.type;
        item.itemToolTip = _item.itemToolTip;
        //image.sprite = item.itemImage;
        //item.itemImage = image.sprite;
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
