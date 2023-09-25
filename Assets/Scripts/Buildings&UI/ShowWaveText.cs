using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowWaveText : MonoBehaviour
{
    public GameObject textPanel;
    public TextMeshProUGUI startWave,endWave;
    public float notificationDuration = 2f;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.current.onStartDefense += showStartWave;
        EventManager.current.onStartEcon += showEndWave;
    }

    void showStartWave()
    {
        textPanel.SetActive(true);
        startWave.gameObject.SetActive(true);
        StartCoroutine(delayedDeactivate(textPanel, startWave, notificationDuration));
    }
    void showEndWave()
    {
        if (gameManager.current.GameOver||gameManager.current.gameState!=gameManager.state.GAMEPLAY)
        {
            return;
        }
        textPanel.SetActive(true);
        endWave.gameObject.SetActive(true);
        StartCoroutine(delayedDeactivate(textPanel, endWave, notificationDuration));
    }
    IEnumerator delayedDeactivate(GameObject panel,TextMeshProUGUI text, float time)
    {
        yield return new WaitForSeconds(time);
        text.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
    }
}
