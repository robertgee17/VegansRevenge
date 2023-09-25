using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialInputManager : MonoBehaviour
{
    public TutorialPanel[] PanelList;
    PlayerInput input;
    private int currentIndex=0;
    public GameObject TutorialCanvas;
    public TutorialState tState;
    public enum TutorialState
    {
        CUTSCENE=-1,
        FARMHOUSE=0,
        ANIMAL=1,
        BREEDING_AREA=2,
        FOOD=3,
        SLAUGHTERHOUSE=4,
        TOWER=5,
        COMBAT=6,
        END=7
    }
    void Start()
    {
        tState = TutorialState.CUTSCENE;
        foreach(TutorialPanel t in PanelList)
        {
            t.gameObject.SetActive(false);
        }
        PanelList[0].gameObject.SetActive(true);
        input = new PlayerInput();
        if (gameManager.current.isTutorial())
        {
            input.Tutorial.Enable();
        }
        input.Tutorial.Next.performed += Next_performed;
        input.Tutorial.Skip.performed += Skip_performed;
        EventManager.current.onEndDefense += showNext;
        EventManager.current.onStartEcon += nextState;
        if (gameManager.current.isTutorial())
        {
            TutorialCanvas.SetActive(true);
        }
        else
        {
            TutorialCanvas.SetActive(false);
        }
    }

    private void Skip_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        startGame();
    }

    void startGame()
    {
        GamemodeType.isTutorial = false;
        SceneManager.LoadScene("MainMap");
    }

    private void Next_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        showNext();
    }
    public void nextState()
    {
        tState++;
    }
    
    void showNext()
    {
        if (gameObject.activeInHierarchy == false)
        {
            return;
        }
        if (!PanelList[currentIndex].showNext())
        {
            if (tState < 0)
                tState = TutorialState.FARMHOUSE;
            if (PanelList[currentIndex].requiredState > tState)
            {
                return;
            }
            PanelList[currentIndex].gameObject.SetActive(false);
            currentIndex++;
            if (currentIndex == PanelList.Length - 2)
            {
                EventManager.current.startDefense();
                PanelList[currentIndex].gameObject.SetActive(true);
            }
            else if (currentIndex < PanelList.Length)
            {
                PanelList[currentIndex].gameObject.SetActive(true);
            }
            else
            {
                startGame();
            }
        }
    }
    private void OnDestroy()
    {
        input.Tutorial.Next.performed -= Next_performed;
        input.Tutorial.Skip.performed -= Skip_performed;
    }
}
