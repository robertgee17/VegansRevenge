using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlaughterhouseUI : MonoBehaviour
{
    public FillBar redBar;
    private Slaughterhouse s;
    private Garrison g;
    private List<FillBar> bars;
    public int listSize = 100;
    public void setup()
    {
        bars = new List<FillBar>();
        for(int i = 0; i < listSize; i++)
        {
            bars.Add(Instantiate(redBar, transform));
            bars[i].gameObject.SetActive(false);
        }
        enabled = false;
    }
    public void setBuildings(GameObject obj)
    {
        s = obj.GetComponent<Slaughterhouse>();
        g = obj.GetComponent<Garrison>();
    }
    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i<listSize; i++)
        {
            if(i >= s.timeRemaining.Count || i >= g.getNumGarrisoned())
            {
                bars[i].gameObject.SetActive(false);
                continue;
            }
            Animal a = g.animalAt(i);
            bars[i].gameObject.SetActive(true);
            bars[i].name.text = a.tag;
            bars[i].setFillPercent(s.timeRemaining[i] / a.eStats.timeToSlaughter);
        }  
    }
    public void hide()
    {
        for (int i = 0; i < listSize; i++)
        {
            bars[i].gameObject.SetActive(false);
        }
    }
}
