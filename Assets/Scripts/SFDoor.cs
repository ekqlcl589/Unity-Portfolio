using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject door;

    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        door.SetActive(active);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (door is null)
            return;

        if (SceneManager.GetActiveScene().buildIndex == Constants.BUILD_INDEX1)
        {
            if (GameManager.Instance.Clear)
                active = true;

            door.SetActive(active);
        }
        else
            this.enabled = false;

    }


}
