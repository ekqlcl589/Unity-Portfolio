using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CusorManager : MonoBehaviour
{
    private Texture2D hand;
    private Texture2D original;

    // Start is called before the first frame update
    void Start()
    {
        hand = Resources.Load<Texture2D>("Cursor_Hand_1");
        original = Resources.Load<Texture2D>("Cursor_cross_hair");

        if(hand is null || original is null)
        {
            Debug.Log("커서 이미지를 찾을 수 없음");
        }
    }

    public void OnMouseOver()
    {
        if (hand is null)
            return;

        Cursor.SetCursor(hand, new Vector2(hand.width / 3, 0), CursorMode.Auto);
    }

    public void OnMouseExit()
    {
        if (original is null)
            return;

        Cursor.SetCursor(original, new Vector2(0, 0), CursorMode.Auto);
    }

    private void OnDisable()
    {
        if (original is null)
            return;

        Cursor.SetCursor(original, Vector2.zero, CursorMode.Auto);
        
    }
}
