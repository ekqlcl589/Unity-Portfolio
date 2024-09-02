using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField]
    private GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject currCamera = FindObjectOfType<PlayerCamera>().gameObject;

            LoadingSceneController.LoadScene("SafeHouse");
            //SceneManager.LoadScene("SafeHouse");

            Instantiate(camera, currCamera.transform.position, currCamera.transform.rotation);
        }
    }

}
