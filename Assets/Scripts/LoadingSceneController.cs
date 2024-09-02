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
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene); // ���� �񵿱�� �θ�
        op.allowSceneActivation = false; // ���� �񵿱�� �ҷ����� �� �ڵ����� �ҷ��� ������ �̵��� ������ ���� �ϴ� �� false�� ���� 90 ���� �θ��� ��ٸ� true�� �ٲٸ� ���� �ҷ��� ��
        // �ε��� �ʹ� ������ �� ȭ���� �ʹ� ���� �������� false�� �����ؼ� ����ũ �ε��� �־���
        // �ε�ȭ�鿡�� �ҷ��;� �ϴ� �� �� �� �ִ°� �ƴ� ������ Ŀ���� ���� ����� ������ �θ��� �̰� �о� �;� �ϴµ� 
        // ���ҽ� �ε��� ������ ���� �� �ε��� ������ ������Ʈ���� ������ ����
        float timer = 0f;
        while(!op.isDone)
        {
            yield return new WaitForSeconds(0.01f); // �� ���ָ� �ٰ� �Ѿ�� �� �� ����

            if(op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else // ����ũ �ε� 1�ʰ� ä���� ���� �ҷ���
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); // 1�ʿ� ���ļ� ä����
                if(progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
