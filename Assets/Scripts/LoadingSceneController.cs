using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField]
    private Image progressBar;

    static string nextScene;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneProcess());

        //Invoke("RandomTipText", 0.3f);
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); // 씬을 비동기로 부름
        op.allowSceneActivation = false; // 씬을 비동기로 불러들일 때 자동으로 불러온 씬으로 이동할 것인지 설정 하는 것 false는 씬을 90 까지 부르고 기다림 true로 바꾸면 씬이 불러와 짐
        // 로딩이 너무 빠르면 팁 화면이 너무 빨리 지나가서 false로 설정해서 페이크 로딩을 넣어줌
        // 로딩화면에서 불러와야 하는 게 씬 만 있는게 아님 볼륨이 커지면 에셋 번들로 나눠서 부르고 이걸 읽어 와야 하는데 
        // 리소스 로딩이 끝나기 전에 씬 로딩이 끝나면 오브젝트들이 깨져서 보임
        float timer = 0f;
        while(!op.isDone)
        {
            yield return new WaitForSeconds(0.01f); // 안 해주면 바가 넘어가는 게 안 보임

            if(op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else // 페이크 로딩 1초간 채워서 씬을 불러옴
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); // 1초에 걸쳐서 채워짐
                if(progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
