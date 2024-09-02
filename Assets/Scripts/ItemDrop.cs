using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField]
    private GameObject[] items = new GameObject[3];

    [SerializeField]
    private Transform transfrom;

    [SerializeField]
    private GameObject obj;

    private bool isDrop;
    // Start is called before the first frame update
    void Start()
    {
        isDrop = false;

        transfrom = GetComponent<Transform>();
        obj = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Vector3 position = new Vector3(0f, -1f, 0f);
            transform.position = other.transform.position + position;

        }
    }

    IEnumerator dropTheItems()
    {
        if(isDrop)
        {
            int maxItems = 10;
            yield return new WaitForSeconds(0.25f);
            for(int i = 0;  i < maxItems; i++)
            {
                int rand = Random.Range(0, 3);
                yield return new WaitForSeconds(0.25f);
                Instantiate(items[rand], transfrom.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }
}
