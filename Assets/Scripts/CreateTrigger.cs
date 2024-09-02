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
    private ZombieData[] zombieDatas; // 사용할 좀비 셋업 데이터들

    [SerializeField]
    private Transform[] spawnPoints; // 좀비 AI를 소환할 위치들

    [SerializeField]
    private Transform bossSpawn;

    private List<Zombie_Dissolve> zombies = new List<Zombie_Dissolve>(); // 생성된 좀비들을 담는 리스트

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

        // 생성 위치 랜덤
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 미리 만들어 두었던 좀비 프리팹으로부터 좀비 생성
        Zombie_Dissolve zombie;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            zombie = Instantiate(zombiePrefab, spawnPoints[i].position, spawnPoints[i].rotation);

            // 생성한 좀비의 능력치 추가
            zombie.Setup(zombieData);

            // 생성된 좀비의 수를 확인하기 위해 리스트에 등록
            zombies.Add(zombie);
            // 좀비의 onDeath 이벤트에 익명 메서드를 등록하고 사망한 좀비를 리스트에서 제거
            zombie.onDeath += () => zombies.Remove(zombie); // 람다식 이용 (입력) => 내용
            // 사망한 좀비는 5초 뒤에 파괴
            zombie.onDeath += () => Destroy(zombie.gameObject);

            // 사망 시 잡은 좀비 수 ++
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
