using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_Boss : LivingEntity
{
    [SerializeField]
    private GameObject itemPrefab;

    [SerializeField]
    private ParticleSystem hitEffect; // 피격 시 재생할 파티클 효과

    [SerializeField]
    private AudioClip deathSound; // 사망 시 재생할 소리
    
    [SerializeField] 
    private AudioClip hitSound; // 피격 시 재생할 소리

    [SerializeField]
    private AudioClip angrySound; // 체력이 일정 수치 이하로 내려갈 시 재생할 사운드 
    
    [SerializeField]
    private LivingEntity targetEntity; // 추적 대상
    
    private NavMeshAgent navMeshAgent; // 경로 계산 AI 에이전트

    private Animator zombieAnimator;

    private AudioSource zombieAudioPlayer;

    private float lastAttackTime;

    private bool angry = false;

    private float maxHealth = 1000f;

    public System.Action onDie;

    public LayerMask whatIsTarget; // 추적 대상 레이어

    private bool hasTarget
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

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioPlayer = GetComponent<AudioSource>();

    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        StartCoroutine(UpdatePath());
        //StartCoroutine(cor_ShowZombieParade());

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            return;
        }

    }

    private IEnumerator UpdatePath()
    {
        while(!dead)
        {
            if (hasTarget)
            {
                // 추격 대상이 존재하면 경로를 갱신하고 ai 이동을 계속 진행
                zombieAnimator.SetFloat("Move", 0.5f);

                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);

                if(health <= 500f)
                {
                    zombieAnimator.SetFloat("Move", 1f);
                    navMeshAgent.speed = 4.5f;

                    if(!angry)
                    {
                        zombieAudioPlayer.PlayOneShot(angrySound);
                        angry = true;
                    }
                }

            }
            else
            {
                navMeshAgent.isStopped = true;
                zombieAnimator.SetFloat("Move", Constants.ZOMVBIE_DIE);

                Collider[] colliders = Physics.OverlapSphere(transform.position, Constants.SPHERE_REDIUS_10, whatIsTarget);
                // 모든 콜라이더를 순회하면서 살아 있는 LiveingEntiry 찾기
                for (int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity live = colliders[i].GetComponent<LivingEntity>();

                    // 컴포넌트가 존재하고 해당 컴포넌트가 살아 있다면
                    if (live != null && !live.dead)
                    {
                        targetEntity = live;
                        break;
                    }
                }

            }

            yield return new WaitForSeconds(1f);
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            zombieAudioPlayer.PlayOneShot(hitSound);
        }

        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        base.Die();

        Collider[] zombieColls = GetComponents<Collider>();

        for (int i = 0; i < zombieColls.Length; i++)
        {
            zombieColls[i].enabled = false;
        }

        zombieAnimator.SetTrigger("Death");

        zombieAudioPlayer.PlayOneShot(deathSound);

        StartCoroutine(cor_ShowZombieParade());
        //this.onDie();

    }

    private void OnTriggerEnter(Collider other)
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (!dead)
        {
            zombieAnimator.SetTrigger("Attack");
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
                    attackTarget.OnDamage(Constants.ZOMBIE_DEFAULT_DAMAGE, hitPoint, hitNormal);
                }

            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
    }

    IEnumerator cor_ShowZombieParade()
    {
        UIManager.Instance.ZombieParade(true);
        yield return new WaitForSeconds(5f);
        UIManager.Instance.ZombieParade(false);
        GameManager.Instance.Last = true;

    }
}
