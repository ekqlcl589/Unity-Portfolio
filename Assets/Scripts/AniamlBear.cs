using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AniamlBear : LivingEntity
{
    [SerializeField]
    private AudioClip dieSound;

    [SerializeField]
    private AudioClip angrySound;

    [SerializeField]
    private GameObject itemPrefab;

    [SerializeField]
    private ParticleSystem hitEffect; // 피격 시 재생할 파티클 효과

    private LivingEntity targetEntity;

    private NavMeshAgent navMeshAgent;

    private Animator AnimalAnimator;

    private AudioSource bearAudio;

    private float lastAttackTime;

    private bool half = true;

    public LayerMask whatIsTarget;

    public System.Action onDie;

    public bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            // 그렇지 않다면 false
            return false;
        }
    }

    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        AnimalAnimator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        bearAudio = GetComponent<AudioSource>();
        navMeshAgent.speed = Constants.BEAR_DEFAULT_SPEED;
        health = Constants.BEAR_DEFAULT_HEALTH;
        StartCoroutine(UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            return;
        }
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (dead is false)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
        }

        base.OnDamage(damage, hitPoint, hitNormal);
    }
    //
    public override void Die()
    {
        base.Die();
        bearAudio.PlayOneShot(dieSound);

        DropItem();
        Collider[] colliders = GetComponents<Collider>();

        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = false;

        //AnimalAnimator.SetBool("Dead", true);

        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        AnimalAnimator.SetTrigger("Die");
        this.onDie();

    }

    public virtual IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                //float movePower = move[Random.Range(0, move.Length)];
                AnimalAnimator.SetFloat("Move", 0.5f);
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);

                if(health <= 50 && half)
                {
                    bearAudio.PlayOneShot(angrySound);
                    AnimalAnimator.SetFloat("Move", Constants.BEAR_DEFAULT_SPEED);
                    navMeshAgent.speed = 2.5f;
                    half = false;
                }
            }
            else
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, Constants.SPHERE_REDIUS_10, whatIsTarget);
                navMeshAgent.isStopped = true;
                AnimalAnimator.SetFloat("Move", Constants.ZERO_FORCE);

                for (int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity live = colliders[i].GetComponent<LivingEntity>();
                    if (live != null && !live.dead)
                    {
                        targetEntity = live;
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

    }
    public virtual void DropItem()
    {
        var go = Instantiate<GameObject>(this.itemPrefab);
        go.transform.position = this.gameObject.transform.position;
        go.SetActive(false);

        this.onDie = () =>
        {
            go.SetActive(true);
            //go.GetComponent<FiledItems>().GetItem();
            go.GetComponent<FiledItems>().SetItem(go.GetComponent<FiledItems>().GetItem());
        };

    }

    private void OnTriggerStay(Collider other)
    {
        if (!dead)
        {
            AnimalAnimator.SetTrigger("Attack");
            if (Time.time >= lastAttackTime + Constants.TIME_BET_ATTACK)
            {
                LivingEntity attackTarget = other.GetComponent<LivingEntity>();

                if (attackTarget != null && attackTarget == targetEntity)
                {
                    lastAttackTime = Time.time;
                    // 상대방의 피격 위치와 피격 방향을 근삿값으로 계산 
                    Vector3 hitPoint = other.ClosestPoint(transform.position);
                    Vector3 hitNormal = transform.position - other.transform.position;
                    // 공격
                    attackTarget.OnDamage(Constants.BEAR_DEFAULT_DAMAGE, hitPoint, hitNormal);
                }

            }

        }

    }

}
