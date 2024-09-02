using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] 
    private float secondPerRealTimeSecond; // 게임 세계에서의 50초 = 현실 세계의 1초 

    [SerializeField] private float nightFogDensity; // 밤 상태의 안개 밀도
    private float dayFogDensity;

    [SerializeField] private float fogDensityCalc; // 증감량 비율
    private float currentFogDensity;

    private bool isNight = false;
    
    //public delegate void OnDayCount();
    //public OnDayCount onDayCount;

    // Start is called before the first frame update
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
        StartCoroutine(DayCount());

    }

    // Update is called once per frame
    void Update()
    {
        SunRotation();
    }

    private void SunRotation()
    {
        // 조명을 x 축으로 회전 현실시간 1초에 0.1f * secondPerRealTimeSecond 각도만큼 회전
        GameManager.Instance.IsNight = isNight;

        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);
        if (transform.eulerAngles.x >= 170)
            isNight = true;
        else if (transform.eulerAngles.x <= 10)
            isNight = false;

        if (isNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }

    private IEnumerator DayCount()
    {
        while(true)
        {
            //dayCount += 1;
            GameManager.Instance.AddDayCount(1);
            yield return new WaitForSeconds(72f);

        }

    }
}
