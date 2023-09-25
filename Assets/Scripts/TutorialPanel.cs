using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialPanel : MonoBehaviour
{
    public GameObject[] topics;
    private int currentIndex = 0;
    public TutorialInputManager.TutorialState requiredState;
    public bool imagesDisappear;
    private void Awake()
    {
        foreach(GameObject obj in topics)
        {
            obj.SetActive(false);
        }
        showNext();
    }

    public bool showNext()
    {
        if (currentIndex >= topics.Length)
        {
            return false;
        }
        topics[currentIndex].gameObject.SetActive(true);
        
        if (currentIndex > 0)
        {
            if (!imagesDisappear)
            {
                disableChildren(currentIndex - 1);
                //foreach(TextMeshProUGUI t in getTextComponents(currentIndex - 1))
                //{
                //    t.gameObject.SetActive(false);
                //}
            }
            else
            {
                topics[currentIndex - 1].gameObject.SetActive(false);
            }
            
        }
        currentIndex++;
        return true;
    }

    TextMeshProUGUI[] getTextComponents(int index)
    {
        return topics[index].GetComponentsInChildren<TextMeshProUGUI>();
    }
    void disableChildren(int index)
    {
        foreach(Transform child in topics[index].transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
