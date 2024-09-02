using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드 가져오기

// 좀비 AI 구현
public class Zombie_Dissolve : LivingEntity
{
    [SerializeField] 
    private Renderer _renderer;

    [SerializeField] 
    private Material mtrlOrg;

    [SerializeField] 
    private Material mtrlDissolve;

    [SerializeField]
    private LayerMask whatIsTarget; // 추적 대상 레이어

    [SerializeField] 
    private ParticleSystem hitEffect; // 피격 시 재생할 파티클 효과

    [SerializeField] 
    private ParticleSystem sunHitEffect;

    [SerializeField] 
    private AudioClip deathSound; // 사망 시 재생할 소리

    [SerializeField] 
    private AudioClip hitSound; // 피격 시 재생할 소리

    [SerializeField]
    private AudioClip sunHitSound;

    [SerializeField]
    private GameObject itemPrefab;

    private float dissolveTime = Constants.ZOMBIE_DEFAULT_DISSOLVE_TIME;

    private LivingEntity targetEntity; // 추적 대상
    private NavMeshAgent navMeshAgent; // 경로 계산 AI 에이전트

    private Animator zombieAnimator; // 애니메이터 컴포넌트
    private AudioSource zombieAudioPlayer; // 오디오 소스 컴포넌트
    private Renderer zombieRenderer; // 렌더러 컴포넌트

    public System.Action onDie;

    private float damage; // 공격력
    private float timeBetAttack; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점

    private bool sunHit = false;
    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    private bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                zombieAnimator.SetBool("Att", true);
                return true;
            }

            // 그렇지 않다면 false
            zombieAnimator.SetBool("Att", false);
            return false;
        }
    }

    private void Awake()
    {
        // 초기화
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioPlayer = GetComponent<AudioSource>();

        zombieRenderer = GetComponentInChildren<Renderer>();
        //zombieRenderer = GetComponent<Renderer>();
    }

    // 좀비 AI의 초기 스펙을 결정하는 셋업 메서드
    public void Setup(ZombieData zombieData)
    {
        startingHealth = zombieData.health;
        health = zombieData.health;

        damage = zombieData.damage;

        navMeshAgent.speed = zombieData.speed;

        zombieRenderer.material.color = zombieData.skinColor;

        timeBetAttack = 0.5f;
    }

    private void Start()
    {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
        StartCoroutine(DayDie());
        //this.onDie += AddKillPoint;

    }

    private void Update()
    {
        // 추적 대상의 존재 여부에 따라 다른 애니메이션 재생
        zombieAnimator.SetBool("HasTarget", hasTarget);
        //Onday(); // 이렇게 하면 낮 되면 뒤지는건 확인 코루틴으로 해보셈
    }

    // 주기적으로 추적할 대상의 위치를 찾아 경로 갱신
    private IEnumerator UpdatePath()
    {
        // 살아 있는 동안 무한 루프
        while (!dead)
        {
            if (hasTarget)
            {
                // 추격 대상이 존재하면 경로를 갱신하고 ai 이동을 계속 진행
                zombieAnimator.SetFloat("Move", 0.5f);

                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);
                                
            }
            else
            {
                navMeshAgent.isStopped = true;
                zombieAnimator.SetFloat("Move", 0f);

                //10유닛의 반지름을 가진 가상의 구를 그렸을 때 구와 겹치는 모든 콜라이더를 가져옴
                //단, whatisTarget 레이어를 가진 콜라이더만 가져오도록 필터링 진행

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
            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    // 데미지를 입었을 때 실행할 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (dead is false)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            zombieAudioPlayer.PlayOneShot(hitSound);
        }
        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        _renderer.material = mtrlDissolve;
        DoDissolve(Constants.ZOMBIE_START_DISSOLBE, Constants.ZOMBIE_DEST_DISSOLBE, dissolveTime);

        DropItem();
        zombieAnimator.SetBool("Att", false);
        //zombieAnimator.SetBool("Attack", false);

        // 다른 ai를 방해하지 않도록 자신의 모든 콜라이더 비활성화 
        Collider[] zombieColls = GetComponents<Collider>();

        for (int i = 0; i < zombieColls.Length; i++)
        {
            zombieColls[i].enabled = false;
        }

        // ai 추적을 중지하고 네비메시 컴포넌트 비활성화 
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        zombieAnimator.SetTrigger("Die");

        zombieAudioPlayer.PlayOneShot(deathSound);

        this.onDie();
    }

    private void OnTriggerStay(Collider other)
    {
        // 자신이 사망하지 않았고, 최근 공격 시점에서 timebetattack 이상 시간이 지났다면 공격
        if (false == dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            if (attackTarget is not null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;
                // 상대방의 피격 위치와 피격 방향을 근삿값으로 계산 
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                // 공격
                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
    }

    public void DropItem()
    {
        var go = Instantiate<GameObject>(this.itemPrefab);
        go.transform.position = this.gameObject.transform.position;
        go.SetActive(false);

        this.onDie = () =>
        {
            //AddKillPoint();
            go.SetActive(true);
            //go.GetComponent<FiledItems>().GetItem();
            go.GetComponent<FiledItems>().SetItem(go.GetComponent<FiledItems>().GetItem());
        };
    }

    private IEnumerator DayDie()
    {
        while(dead is false)
        {
            if(GameManager.Instance.IsNight is false && GameManager.Instance.Last is false)
            {
                zombieAudioPlayer.PlayOneShot(sunHitSound);
                sunHit = true;
                sunHitEffect.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                sunHitEffect.Play();

                health -= Constants.ZOMBIE_DEFAULT_DAMAGE;
                //zombieAudioPlayer.PlayOneShot(hitSound);

                if (health <= 0 && dead is false)
                {
                    //AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.sunKill,
                    //     sun: 1);

                    AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.sunKill, value: 1);

                    Die();
                }

            }
            
            yield return new WaitForSeconds(1f);
        }
    }

    void DoDissolve(float start, float dest, float time) // 매번 값 갱신, 갱신 될 때마다 호출되는 콜백함수 역할
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", start, "to", dest, "time", time,
            "onupdatetarget", gameObject,
            "onupdate", "TweenOnUpdate",
            "oncomplte", "TweenOnComplte",
            "easetype", iTween.EaseType.easeInOutCubic));
    }

    void TweenOnUpdate(float value)
    {
        _renderer.material.SetFloat("_SpllitValue", value);
    }

    void TweenOnComplte()
    {
        _renderer.material = mtrlOrg;
    }

}