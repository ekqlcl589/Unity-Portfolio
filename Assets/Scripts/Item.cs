using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    EquipMent,
    Use,
    RawFood,
    bullet,
    special,
    Etc
}

[System.Serializable]
public class Item : IItem
{
    public ItemType type;
    public string itemName;
    public Sprite itemImage;
    public StateUI stateUI;
    public string itemToolTip;

    public bool Used(GameObject target)
    {
        if(type == ItemType.bullet)
        {
            PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

            if (playerShooter != null && playerShooter.Gun != null)
                playerShooter.Gun.ammoRemain += Constants.DEFAULT_ADD30;

        }

        else if (type == ItemType.Use)
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();

            if (playerHealth != null && !playerHealth.dead)
            {
                if (playerHealth.Hunger <= Constants.PLAYER_MAX_HUNGER)
                    playerHealth.RestoreHunger(Constants.DEFAULT_ADD30);  


                playerHealth.RestoreHealth(Constants.DEFAULT_ADD20);
            }
        }

        else if(type == ItemType.RawFood)
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.dead)
            {
                if (playerHealth.Hunger <= Constants.PLAYER_MAX_HUNGER)
                    playerHealth.RestoreHunger(Constants.DEFAULT_ADD30);

                playerHealth.Diminish(Constants.DEFAULT_ADD30);
                //stateUI.UpdateState();
            }
        }
        else if(type == ItemType.special)
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();

            if (playerHealth != null && !playerHealth.dead)
            {
                if (playerHealth.Hunger <= Constants.PLAYER_MAX_HUNGER)
                    playerHealth.RestoreHunger(Constants.DEFAULT_ADD30);

                playerHealth.RestoreHealth(Constants.DEFAULT_ADD50);
            }

        }
        else if (type == ItemType.Etc || type == ItemType.EquipMent)
        {
            return false;
        }

        return true;
    }

    public ItemType Type
    {
        get => type;

    }

    public void Use(GameObject target)
    {
    
    }
    public void Auto(GameObject target)
    {
    
    }
}
