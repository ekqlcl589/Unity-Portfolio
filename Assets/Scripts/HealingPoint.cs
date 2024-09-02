using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPoint : MonoBehaviour, IItem
{
    private LivingEntity targetEntiry;

    private float lastHealTime; // ������ ü�� ȸ�� ����

    private bool hasTarget
    {
        get
        {
            if (targetEntiry != null && !targetEntiry.dead)
                return true;
            
            return false;
        }

    }

    public LayerMask whatIsTarget; // �÷��̾ ȸ�� ���Ѿ� ��

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
                // ������Ʈ�� �����ϰ� �ش� ������Ʈ�� ��� �ִٸ�
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

        // LivingEntity������Ʈ�� �ִٸ�
        if (life != null)
        {
            if (life.Temperature >= life.MaxTemperature)
                return;
            // ü�� ȸ�� ����
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
