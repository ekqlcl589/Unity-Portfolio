using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectWeaponUI : MonoBehaviour
{
    [SerializeField]
    private Image _weaponUI;

    public Image weaponUI
    {
        get
        {
            if(_weaponUI is null)
            {
                GameObject ui = GameObject.Find("HUD Canvas/WeaponUI/OutLine/Image");
                if (ui != null)
                    _weaponUI = ui.GetComponent<Image>();
            }
            return _weaponUI;
        }
    }

    static SelectWeaponUI _instance;

    public static SelectWeaponUI Instance
    {
        get
        {
            if (_instance is null)
            {
                GameObject obj = new GameObject("SelectWeaponUI");
                _instance = obj.AddComponent<SelectWeaponUI>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }
    //혹시 모르니 적어둠 모델 무기 잡는 손 이름 Weapon_R
}
