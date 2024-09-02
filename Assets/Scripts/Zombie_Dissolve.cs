using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, ������̼� �ý��� ���� �ڵ� ��������

// ���� AI ����
public class Zombie_Dissolve : LivingEntity
{
    [SerializeField] 
    private Renderer _renderer;

    [SerializeField] 
    private Material mtrlOrg;

    [SerializeField] 
    private Material mtrlDissolve;

    [SerializeField]
    private LayerMask whatIsTarget; // ���� ��� ���̾�

    [SerializeField] 
    private ParticleSystem hitEffect; // �ǰ� �� ����� ��ƼŬ ȿ��

    [SerializeField] 
    private ParticleSystem sunHitEffect;

    [SerializeField] 
    private AudioClip deathSound; // ��� �� ����� �Ҹ�

    [SerializeField] 
    private AudioClip hitSound; // �ǰ� �� ����� �Ҹ�

    [SerializeField]
    private AudioClip sunHitSound;

    [SerializeField]
    private GameObject itemPrefab;

    private float dissolveTime = Constants.ZOMBIE_DEFAULT_DISSOLVE_TIME;

    private LivingEntity targetEntity; // ���� ���
    private NavMeshAgent navMeshAgent; // ��� ��� AI ������Ʈ

    private Animator zombieAnimator; // �ִϸ����� ������Ʈ
    private AudioSource zombieAudioPlayer; // ����� �ҽ� ������Ʈ
    private Renderer zombieRenderer; // ������ ������Ʈ

    public System.Action onDie;

    private float damage; // ���ݷ�
    private float timeBetAttack; // ���� ����
    private float lastAttackTime; // ������ ���� ����

    private bool sunHit = false;
    // ������ ����� �����ϴ��� �˷��ִ� ������Ƽ
    private bool hasTarget
    {
        get
        {
            // ������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            if (targetEntity != null && !targetEntity.dead)
            {
                zombieAnimator.SetBool("Att", true);
                return true;
            }

            // �׷��� �ʴٸ� false
            zombieAnimator.SetBool("Att", false);
            return false;
        }
    }

    private void Awake()
    {
        // �ʱ�ȭ
        navMeshAgent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioPlayer = GetComponent<AudioSource>();

        zombieRenderer = GetComponentInChildren<Renderer>();
        //zombieRenderer = GetComponent<Renderer>();
    }

    // ���� AI�� �ʱ� ������ �����ϴ� �¾� �޼���
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
        // ���� ������Ʈ Ȱ��ȭ�� ���ÿ� AI�� ���� ��ƾ ����
        StartCoroutine(UpdatePath());
        StartCoroutine(DayDie());
        //this.onDie += AddKillPoint;

    }

    private void Update()
    {
        // ���� ����� ���� ���ο� ���� �ٸ� �ִϸ��̼� ���
        zombieAnimator.SetBool("HasTarget", hasTarget);
        //Onday(); // �̷��� �ϸ� �� �Ǹ� �����°� Ȯ�� �ڷ�ƾ���� �غ���
    }

    // �ֱ������� ������ ����� ��ġ�� ã�� ��� ����
    private IEnumerator UpdatePath()
    {
        // ��� �ִ� ���� ���� ����
        while (!dead)
        {
            if (hasTarget)
            {
                // �߰� ����� �����ϸ� ��θ� �����ϰ� ai �̵��� ��� ����
                zombieAnimator.SetFloat("Move", 0.5f);

                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);
                                
            }
            else
            {
                navMeshAgent.isStopped = true;
                zombieAnimator.SetFloat("Move", 0f);

                //10������ �������� ���� ������ ���� �׷��� �� ���� ��ġ�� ��� �ݶ��̴��� ������
                //��, whatisTarget ���̾ ���� �ݶ��̴��� ���������� ���͸� ����

                Collider[] colliders = Physics.OverlapSphere(transform.position, Constants.SPHERE_REDIUS_10, whatIsTarget);
                // ��� �ݶ��̴��� ��ȸ�ϸ鼭 ��� �ִ� LiveingEntiry ã��
                for (int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity live = colliders[i].GetComponent<LivingEntity>();

                    // ������Ʈ�� �����ϰ� �ش� ������Ʈ�� ��� �ִٸ�
                    if (live != null && !live.dead)
                    {
                        targetEntity = live;
                        break;
                    }
                }
            }
            // 0.25�� �ֱ�� ó�� �ݺ�
            yield return new WaitForSeconds(0.25f);
        }
    }

    // �������� �Ծ��� �� ������ ó��
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (dead is false)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            zombieAudioPlayer.PlayOneShot(hitSound);
        }
        // LivingEntity�� OnDamage()�� �����Ͽ� ������ ����
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // ��� ó��
    public override void Die()
    {
        // LivingEntity�� Die()�� �����Ͽ� �⺻ ��� ó�� ����
        base.Die();

        _renderer.material = mtrlDissolve;
        DoDissolve(Constants.ZOMBIE_START_DISSOLBE, Constants.ZOMBIE_DEST_DISSOLBE, dissolveTime);

        DropItem();
        zombieAnimator.SetBool("Att", false);
        //zombieAnimator.SetBool("Attack", false);

        // �ٸ� ai�� �������� �ʵ��� �ڽ��� ��� �ݶ��̴� ��Ȱ��ȭ 
        Collider[] zombieColls = GetComponents<Collider>();

        for (int i = 0; i < zombieColls.Length; i++)
        {
            zombieColls[i].enabled = false;
        }

        // ai ������ �����ϰ� �׺�޽� ������Ʈ ��Ȱ��ȭ 
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        zombieAnimator.SetTrigger("Die");

        zombieAudioPlayer.PlayOneShot(deathSound);

        this.onDie();
    }

    private void OnTriggerStay(Collider other)
    {
        // �ڽ��� ������� �ʾҰ�, �ֱ� ���� �������� timebetattack �̻� �ð��� �����ٸ� ����
        if (false == dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            if (attackTarget is not null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;
                // ������ �ǰ� ��ġ�� �ǰ� ������ �ٻ����� ��� 
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                // ����
                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
        // Ʈ���� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����
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

    void DoDissolve(float start, float dest, float time) // �Ź� �� ����, ���� �� ������ ȣ��Ǵ� �ݹ��Լ� ����
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