using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    [SerializeField]
    private Animal animalPrefabs; 

    [SerializeField]
    private AnimalChicken animalPrefabs1; 

    [SerializeField]
    private Transform[] spawnPoints;

    private List<Animal> animals = new List<Animal>();
    private List<AnimalChicken> animals1 = new List<AnimalChicken>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            return;
        }

        if (animals.Count <= Constants.DEFAULT_NUMBER_0)
            SpawnAnimal();
    }

    private void SpawnAnimal()
    {
        for (int i = 0; i < Constants.DEFAULT_NUMBER_4; i++)
            CreateAnimal();

        for(int i = 0; i < Constants.DEFAULT_NUMBER_2; i ++)
        {
            CreateAnimal1();
        }
    }

    private void CreateAnimal()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Animal animal = Instantiate(animalPrefabs, spawnPoint.position, spawnPoint.rotation);

        animals.Add(animal);

        animal.onDeath += () => animals.Remove(animal);

        animal.onDeath += () => Destroy(animal.gameObject, Constants.DELETE_TIME);
    }

    private void CreateAnimal1()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        AnimalChicken animal = Instantiate(animalPrefabs1, spawnPoint.position, spawnPoint.rotation);

        animals1.Add(animal);

        animal.onDeath += () => animals1.Remove(animal);

        animal.onDeath += () => Destroy(animal.gameObject, Constants.DELETE_TIME);

    }

}
