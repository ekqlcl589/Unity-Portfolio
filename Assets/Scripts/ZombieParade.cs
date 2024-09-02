using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieParade : MonoBehaviour
{
    [SerializeField]
    private Zombie_Dissolve[] zombies;

    [SerializeField]
    private Transform playerTransform;

    private List<Zombie_Dissolve> zombielist = new List<Zombie_Dissolve>();

    private float distance = 20f;

    private int cnt = 50;

    private bool safe = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.Last is true && GameManager.Instance.isGameover is false && zombielist.Count <= cnt)
        {
           Spawn();
        }

        if (cnt <= 45 && !safe)
        {
            GameManager.Instance.Clear = true;
            StartCoroutine(Cor_ShowSafeText5Sec());
        }
    }

    private void Spawn()
    {
        Vector3 spawnPosition =
        GetRandomPointOnNavMesh(playerTransform.position, distance);

        Zombie_Dissolve selectedItem = zombies[Random.Range(0, zombies.Length)];
        Zombie_Dissolve zombie = Instantiate(selectedItem, spawnPosition, Quaternion.identity);

        zombielist.Add(zombie);

        zombie.onDeath += () => Destroy(zombie.gameObject, Constants.DELETE_TIME);

        zombie.onDeath += () => cnt--;
        
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        // center를 중심으로 반지름이 maxDistance인 구 안에서의 랜덤한 위치 하나를 저장
        // Random.insideUnitSphere는 반지름이 1인 구 안에서의 랜덤한 한 점을 반환하는 프로퍼티
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        // 내비메시 샘플링의 결과 정보를 저장하는 변수
        NavMeshHit hit;

        // maxDistance 반경 안에서, randomPos에 가장 가까운 내비메시 위의 한 점을 찾음
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        // 찾은 점 반환
        return hit.position;
    }

    IEnumerator Cor_ShowSafeText5Sec()
    {
        UIManager.Instance.SafeHouse(true);
        yield return new WaitForSeconds(5f);
        UIManager.Instance.SafeHouse(false);
        safe = true;
    }

}
