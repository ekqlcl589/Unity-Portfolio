using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTrigger : MonoBehaviour
{
    [SerializeField]
    private Zombie_Boss bossPrefab;

    [SerializeField]
    private Zombie_Dissolve zombiePrefab;

    [SerializeField]
    private ZombieData[] zombieDatas; // ����� ���� �¾� �����͵�

    [SerializeField]
    private Transform[] spawnPoints; // ���� AI�� ��ȯ�� ��ġ��

    [SerializeField]
    private Transform bossSpawn;

    private List<Zombie_Dissolve> zombies = new List<Zombie_Dissolve>(); // ������ ������� ��� ����Ʈ

    public System.Action action;
    private int killPoint = 0;

    private bool isCreate = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameover)
            return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isCreate && GameManager.Instance.ZombieCount >= 10)// && GameManager.instance.isNight)
        {
            //CreateZombie();
           StartCoroutine(cor_ShowBossUI());
           CreateBoss();
        }
    }

    private void CreateZombie()
    {
        ZombieData zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)];

        // ���� ��ġ ����
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // �̸� ����� �ξ��� ���� ���������κ��� ���� ����
        Zombie_Dissolve zombie;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            zombie = Instantiate(zombiePrefab, spawnPoints[i].position, spawnPoints[i].rotation);

            // ������ ������ �ɷ�ġ �߰�
            zombie.Setup(zombieData);

            // ������ ������ ���� Ȯ���ϱ� ���� ����Ʈ�� ���
            zombies.Add(zombie);
            // ������ onDeath �̺�Ʈ�� �͸� �޼��带 ����ϰ� ����� ���� ����Ʈ���� ����
            zombie.onDeath += () => zombies.Remove(zombie); // ���ٽ� �̿� (�Է�) => ����
            // ����� ����� 5�� �ڿ� �ı�
            zombie.onDeath += () => Destroy(zombie.gameObject);

            // ��� �� ���� ���� �� ++
            zombie.onDeath += () => GameManager.Instance.AddZombieCount(1);

            zombie.onDeath += () => action();
        }


    }

    void CreateBoss()
    {
        Zombie_Boss boss = Instantiate(bossPrefab, bossSpawn.position, bossSpawn.rotation);

        boss.onDeath += () => Destroy(boss.gameObject, 10f);

        isCreate = false;

    }

    void AddKillPoint()
    {
        killPoint++;

        AchievementsManager.Achievements combinedAchievements = AchievementsManager.Achievements.kill1 | AchievementsManager.Achievements.kill10;
        AchievementsManager.Instance.OnNotify(combinedAchievements, value: killPoint);
    }

    IEnumerator cor_ShowBossUI()
    {
        UIManager.Instance.BossUI(true);
        yield return new WaitForSeconds(5f);
        UIManager.Instance.BossUI(false);
    }
}
