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
            // Player를 찾음
            tPlayer = GameObject.FindWithTag("Player");

            // Player가 존재하면 카메라의 Follow Target으로 설정
            if (tPlayer != null)
            {
                tFollowTarget = tPlayer.transform;
                vcam.Follow = tFollowTarget;
            }
            else
            {
                // Player가 파괴된 경우 카메라의 Follow Target을 해제
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
