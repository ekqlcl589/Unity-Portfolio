using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] items;

    [SerializeField]
    private Transform playerTransform;

    private List<Item> itemList = new List<Item>();

    private float maxDistance = Constants.DISTANCE50;

    private float timeBetSpawnMax = Constants.SPAWN_MAX_TIME; // �ִ� �ð� ����
    private float timeBetSpawnMin = Constants.SPAWN_MIN_TIME; // �ּ� �ð� ����
    private float timeBetSpawn; // ���� ����

    private float lastSpawnTime; // ������ ���� ����

    // Start is called before the first frame update
    void Start()
    {
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = Constants.DEFAULT_NUMBER_0;

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastSpawnTime + timeBetSpawn && playerTransform != null)
        {
            // ������ ���� �ð� ����
            lastSpawnTime = Time.time;
            // ���� �ֱ⸦ �������� ����
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            // ������ ���� ����
            //if(itemList.Count <= 1)
                Spawn();
        }

    }

    private void Spawn()
    {
        // �÷��̾� ��ó���� ����޽� ���� ���� ��ġ ��������
        Vector3 spawnPosition =
            GetRandomPointOnNavMesh(playerTransform.position, maxDistance);
        // �ٴڿ��� 0.86��ŭ ���� �ø���
        //spawnPosition += Vector3.up * 0.86f;

        // ������ �� �ϳ��� �������� ��� ���� ��ġ�� ����
        GameObject selectedItem = items[Random.Range(0, items.Length)];
        GameObject item = Instantiate(selectedItem, spawnPosition, Quaternion.identity);

        //itemList.Add(item);
        // ������ �������� 5�� �ڿ� �ı�
        Destroy(item, Constants.DELETE_TIME);

    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        // center�� �߽����� �������� maxDistance�� �� �ȿ����� ������ ��ġ �ϳ��� ����
        // Random.insideUnitSphere�� �������� 1�� �� �ȿ����� ������ �� ���� ��ȯ�ϴ� ������Ƽ
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        // ����޽� ���ø��� ��� ������ �����ϴ� ����
        NavMeshHit hit;

        // maxDistance �ݰ� �ȿ���, randomPos�� ���� ����� ����޽� ���� �� ���� ã��
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas);

        // ã�� �� ��ȯ
        return hit.position;
    }
}
