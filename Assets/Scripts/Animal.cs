using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : LivingEntity
{
    [SerializeField]
    private GameObject itemPrefab;

    [SerializeField]
    private AudioClip dieSound;

    [SerializeField]
    private ParticleSystem hitEffect; // �ǰ� �� ����� ��ƼŬ ȿ��

    private LivingEntity targetEntity;

    private NavMeshAgent navMeshAgent;

    private Animator AnimalAnimator;

    private AudioSource pigAudio;

    public LayerMask whatIsTarget;

    public System.Action onDie;
    
    public bool hasTarget
    {
        get
        {
            // ������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            // �׷��� �ʴٸ� false
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
        pigAudio = GetComponent<AudioSource>();
        navMeshAgent.speed = Constants.PIG_DEFAULT_SPEED;
        health = Constants.PIG_DEFAULT_HEALTH;
        StartCoroutine(UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            return;
        }

        this.onDeath += () => Destroy(this);
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
        pigAudio.PlayOneShot(dieSound);

        AnimalAnimator.SetFloat("Move", Constants.ZERO_FORCE);
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
        while(!dead)
        {
            if(hasTarget)
            {
                //float movePower = move[Random.Range(0, move.Length)];
                AnimalAnimator.SetFloat("Move", 0.5f/*Random.Range(3, movePower)*/);
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, Constants.SPHERE_REDIUS_5, whatIsTarget);
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
}
