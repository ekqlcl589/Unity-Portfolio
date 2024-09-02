using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    [SerializeField]
    private GameObject soundPanel;

    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        soundPanel.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            active = !active;
            soundPanel.SetActive(active);
        }

    }
}
