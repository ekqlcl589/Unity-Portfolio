using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScene : MonoBehaviour
{
    private const float delayTime = 3f;

    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(delayTime);
    }
}
