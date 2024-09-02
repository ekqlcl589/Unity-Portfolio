using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETCItem : ItemEffect
{
    private LivingEntity targetEntity;

    private Collider playerCollirder;
 
    private LayerMask isTarget;

    // Start is called before the first frame update
    public override bool ExecuteRole()
    {
        if (playerCollirder.gameObject.tag == ("Player"))
        {
            LivingEntity live = playerCollirder.GetComponent<LivingEntity>();
        
            if (live != null && !live.dead)
            {
                targetEntity = live;
        
                //if (live.health >= live.maxHealth)
                //    return false;
        
                live.RestoreHealth(Constants.DEFAULT_NUMBER_10);
            }
        }

        return true;
    }
}
