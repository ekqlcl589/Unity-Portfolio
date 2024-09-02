using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cook : MonoBehaviour
{
    [SerializeField]
    private GameObject cookPanel;

    private static Cook instance;
    
    private bool active = false;

    private BitFlag.UIStateFlags currentUIState = BitFlag.UIStateFlags.none;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }

    public void Start()
    {
       cookPanel.SetActive(active);

       StartCoroutine(CheckForKeyFPress());

    }

    public void Update()
    {

    }

    private IEnumerator CheckForKeyFPress()
    {
        while (true)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                if(cookPanel.activeSelf is false)
                {
                    cookPanel.SetActive(true);

                    if((currentUIState & BitFlag.UIStateFlags.cookActivated) == 0)
                    {
                        currentUIState |= BitFlag.UIStateFlags.cookActivated;

                        AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.uiCooking, value: 1);

                    }
                }
                else
                {
                    cookPanel.SetActive(false);
                }
            }
            yield return null;
        }
    }

}
