using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager current;

    private gameManager gm;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI selectedText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI waveCount;
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;
    public GameObject shopWindow;

    public GameObject gameEndPanel;
    public TextMeshProUGUI gameEndText;
    public GameObject pauseMenuPanel;
    public GameObject tacticalPausePanel;
    public GameObject audioSettingsPanel;

    public GameObject endCutscene;
    public GameObject victoryDialogue;
    public GameObject defeatDialogue;

    private void Awake()
    {
        if (current != this && current != null)
        {
            Destroy(gameObject);
        }
        else
        {
            current = this;
        }
    }
    private void Start()
    {
        gm = gameManager.current;
        EventManager.current.onFarmhouseDestroyed += defeatCutscene;
        EventManager.current.onVictory += victoryCutscene;
        if (!gm.isTutorial())
        {
            tutorialPanel.SetActive(true);
        }
        else
        {
            tutorialPanel.SetActive(false);
        }
    }
    private void Update()
    {
        setStatsText();
        waveCount.text = "Wave: " + (gm.getCurrentWave() + 1);
        if (!GamemodeType.isInfinite && !gameManager.current.isTutorial())
        {
            waveCount.text += "/10";
        }
        //waveCount.text = "Wave: 9";
    }
    private void victoryCutscene()
    {
        endCutscene.SetActive(true);
        victoryDialogue.SetActive(true);
        defeatDialogue.SetActive(false);
    }
    private void defeatCutscene()
    {
        endCutscene.SetActive(true);
        victoryDialogue.SetActive(false);
        defeatDialogue.SetActive(true);
    }
    public void toggleTacticalPause()
    {
        if (tacticalPausePanel.activeInHierarchy)
        {
            tacticalPausePanel.SetActive(false);
            unpause();
            return;
        }
        Time.timeScale = 0f;
        tacticalPausePanel.SetActive(true);
    }
    public void showShop()
    {
        shopWindow.SetActive(true);
    }
    public void hideShop()
    {
        shopWindow.SetActive(false);
    }
    public void onGameOver()
    {
        gameEndPanel.SetActive(true);
        gameEndText.text = "Defeat";
    }
    public void onVictory()
    {
        gameEndPanel.SetActive(true);
        gameEndText.text = "Victory!";
    }
    public void onRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void pause()
    {
        if (pauseMenuPanel.activeInHierarchy)
        {
            resume();
            return;
        }
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
    }
    public void resume()
    {
        pauseMenuPanel.SetActive(false);
        unpause();
    }
    private void unpause()
    {
        if (!pauseMenuPanel.activeInHierarchy && !tacticalPausePanel.activeInHierarchy)
        {
            Time.timeScale = 1f;
        }
    }
    public void goToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
    public void goToCredits()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-1);
    }
    public void exitGame()
    {
        Application.Quit(0);
    }
    public void setStatsText()
    {
        if (gm.selected == null)
        {
            statsText.text = "";
            return;
        }
        GameObject obj = gm.selected.gameObject;
        var health = obj.GetComponent<ComponentHealth>();
        if (health != null)
        {
            float maxHealth = health.getMaxHealth();
            float currentHealth = health.getCurrentHealth();
            statsText.text = "Health: " + currentHealth + "/" + maxHealth + "\n";
        }
        var attack = obj.GetComponent<ComponentAttack>();
        if (attack != null)
        {
            float damage = attack.getDamage(); ;
            float attackRate = attack.getAttackRate();
            float range = attack.getRange();
            float aggroRadius = attack.getAggroRadius();
            statsText.text += "Damage: " + damage + "\n";
            statsText.text += "Attack Rate: " + attackRate + "\n";
            statsText.text += "Range: " + range + "\n";
            statsText.text += "Aggro Radius: " + aggroRadius + "\n";

        }

        var move = obj.GetComponent<ComponentMove>();
        if (move != null)
        {
            float speed = move.getSpeed();
            statsText.text += "Speed: " + speed + "\n";
        }

        var garrison = obj.GetComponent<Garrison>();
        if (garrison != null)
        {
            int capacity = garrison.getMaxCapacity();
            statsText.text += "Max Capacity: " + capacity;
        }
        var animal = obj.GetComponent<Animal>();
        if (animal != null)
        {
            statsText.text += "Breeding Cost: " + animal.eStats.foodToBreed + " Food";
        }

    }
    public void setIntroText(string text)
    {
        tutorialText.text = text;
    }
    public void showAudioSettings()
    {
        audioSettingsPanel.SetActive(true);
    }
}
