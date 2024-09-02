using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPoint : MonoBehaviour, IItem
{
    private LivingEntity targetEntiry;

    private float lastHealTime; // 마지막 체력 회복 시점

    private bool hasTarget
    {
        get
        {
            if (targetEntiry != null && !targetEntiry.dead)
                return true;
            
            return false;
        }

    }

    public LayerMask whatIsTarget; // 플레이어만 회복 시켜야 함

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(UpdateHealing());
    }

    // Update is called once per frame

    private IEnumerator UpdateHealing()
    {
        if(hasTarget)
        {
           // life.RestoreHealth(health);
        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, Constants.SPHERE_REDIUS_5, whatIsTarget);
           
            for (int i = 0; i < colliders.Length; i++)
            {
                LivingEntity live = colliders[i].GetComponent<LivingEntity>();
                live.RestoreTemperature(Constants.HEALING_POINT);
                // 컴포넌트가 존재하고 해당 컴포넌트가 살아 있다면
                if (live != null && !live.dead)
                {
                    targetEntiry = live;
                    break;
                }
            }
        }

        yield return new WaitForSeconds(0.25f);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == ("Player") && Time.time >= lastHealTime + Constants.TIMEBET_HEAL)
        {
            lastHealTime = Time.time;

            Auto(other.gameObject);
        }

    }

    public void Auto(GameObject gameObject)
    {
        LivingEntity life = gameObject.GetComponent<LivingEntity>();

        // LivingEntity컴포넌트가 있다면
        if (life != null)
        {
            if (life.Temperature >= life.MaxTemperature)
                return;
            // 체온 회복 실행
            life.RestoreTemperature(Constants.HEALING_POINT);
        }

    }
    public void Use(GameObject gameObject)
    {

    }
    public bool Used(GameObject target)
    {
        return true;
    }

}
