using UnityEngine;

// 게임 점수를 증가시키는 아이템
public class Coin : MonoBehaviour, IItem {

    public void Use(GameObject target) {
        //PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();
        //
        //if (playerShooter != null && playerShooter.gun != null)
        //    playerShooter.gun.ammoRemain += ammo;
        //
        //Destroy(gameObject);
    }

    public void Auto(GameObject target)
    {

    }
    public bool Used(GameObject target)
    {
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        if (playerShooter != null && playerShooter.Gun != null)
            playerShooter.Gun.ammoRemain += Constants.DEFAULT_ADD30;

        return true;
    }

}