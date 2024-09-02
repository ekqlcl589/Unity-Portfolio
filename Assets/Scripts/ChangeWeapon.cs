using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject defaultWeapon;

    [SerializeField]
    private GameObject Weapon;

    // Start is called before the first frame update
    void Start()
    {
        defaultWeapon.SetActive(true);
        Weapon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        OnChangeWeapon();
    }

    void OnChangeWeapon()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            GameManager.Instance.WeaponNum = Constants.WEAPONE_NUMBER2;
            defaultWeapon.SetActive(false);
            Weapon.SetActive(true);

            Vector3 position = SelectWeaponUI.Instance.weaponUI.transform.localPosition;
            position.x = 65f;
            position.y = 0f;
            SelectWeaponUI.Instance.weaponUI.transform.localPosition = position;
        }
        else if(Input.GetKeyDown(KeyCode.V))
        {
            GameManager.Instance.WeaponNum = Constants.WEAPONE_NUMBER1;
            defaultWeapon.SetActive(true);
            Weapon.SetActive(false);

            Vector3 position = SelectWeaponUI.Instance.weaponUI.transform.localPosition;
            position.x = 0f;
            position.y = 0f;
            SelectWeaponUI.Instance.weaponUI.transform.localPosition = position;
        }
    }
}
