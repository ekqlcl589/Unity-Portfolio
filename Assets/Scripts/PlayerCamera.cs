using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject tPlayer;

    [SerializeField]
    private Transform tFollowTarget;

    private static PlayerCamera instance;

    private CinemachineVirtualCamera vcam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        tPlayer = null;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTargeting();
    }

    void LateUpdate()
    {
        if (tFollowTarget == null || vcam == null || vcam.Follow == null)
            return;

        FindingAnInvisibleObject();
    }

    private void PlayerTargeting()
    {
        if (tPlayer == null)
        {
            // Player�� ã��
            tPlayer = GameObject.FindWithTag("Player");

            // Player�� �����ϸ� ī�޶��� Follow Target���� ����
            if (tPlayer != null)
            {
                tFollowTarget = tPlayer.transform;
                vcam.Follow = tFollowTarget;
            }
            else
            {
                // Player�� �ı��� ��� ī�޶��� Follow Target�� ����
                vcam.Follow = null;
                tFollowTarget = null;
            }
        }

    }
    private void FindingAnInvisibleObject()
    {
        Vector3 direction = (tPlayer.transform.position - transform.position).normalized;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity, 1 << LayerMask.NameToLayer("EnvironmentObject"));

        for (int i = 0; i < hits.Length; i++)
        {
            TransparentObject[] obj = hits[i].transform.GetComponentsInChildren<TransparentObject>();

            for (int j = 0; j < obj.Length; j++)
            {
                //if(tPlayer.transform.position.y >= obj[j].transform.position.y)
                obj[j]?.BecomeTransparent();

            }
        }
    }

}
