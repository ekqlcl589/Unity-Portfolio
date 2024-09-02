using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundOption : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private GameObject soundPanel;

    private bool active = false;

    private BitFlag.UIStateFlags currentUIState = BitFlag.UIStateFlags.none;

    // Start is called before the first frame update
    void Start()
    {
        soundPanel.SetActive(active);

        StartCoroutine(CheckForKeyEscapePress());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator CheckForKeyEscapePress()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (soundPanel.activeSelf is false)
                {
                    soundPanel.SetActive(true);

                    if ((currentUIState & BitFlag.UIStateFlags.optionActivated) == 0)
                    {
                        currentUIState |= BitFlag.UIStateFlags.optionActivated;

                        AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.uiOption, value: 1);
                    }
                }
                else
                {
                    soundPanel.SetActive(false);
                }
            }
            yield return null;
        }
    }

    public void SetBgmVolume()
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(bgmSlider.value) * 20);
    }
    public void SetSfxVolume()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(bgmSlider.value) * 20);
    }

}
