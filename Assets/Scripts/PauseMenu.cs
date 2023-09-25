using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        musicSlider.onValueChanged.AddListener(delegate { musicVolumeChange(); });
        sfxSlider.onValueChanged.AddListener(delegate { sfxVolumeChange(); });
    }

    private void musicVolumeChange()
    {
        AudioManager.current.setBackgroundMusicPercent(musicSlider.value);
    }
    private void sfxVolumeChange()
    {
        AudioManager.current.setSFXPercent(sfxSlider.value);
    }
    public void close()
    {
        panel.SetActive(false);
    }
    private void OnDisable()
    {
        panel.SetActive(false); 
    }
}
