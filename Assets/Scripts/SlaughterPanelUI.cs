using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SlaughterPanelUI : MonoBehaviour
{
    public GameObject slaughterPanel;
    public SlaughterIcon slaughterIcon;
    public Transform panelTarget;
    private Garrison garrison;
    private Slaughterhouse slaughterhouse;

    private List<SlaughterIcon> icons;
    Transform ui;
    Transform cam;
    // Start is called before the first frame update
    private void Start()
    {
        icons = new List<SlaughterIcon>();
        cam = Camera.main.transform;
        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                garrison = GetComponent<Garrison>();
                slaughterhouse = GetComponent<Slaughterhouse>();
                ui = Instantiate(slaughterPanel, c.transform).transform;
                ui.gameObject.SetActive(true);

                break;
            }
        }
        for(int i = 0; i < slaughterhouse.numKillableAtATime; i++)
        {
            SlaughterIcon s = Instantiate(slaughterIcon, ui);
            s.gameObject.SetActive(false);
            icons.Add(s);
        }

    }
    private void Update()
    {
        for(int i = 0; i < icons.Count; i++)
        {
            if (i >= garrison.getNumGarrisoned())
            {
                icons[i].gameObject.SetActive(false);
                continue;
            }
            Animal a = garrison.animalAt(i);
            icons[i].gameObject.SetActive(true);
            icons[i].updateStatus(a.type, slaughterhouse.timeRemaining[i] / a.eStats.timeToSlaughter);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (panelTarget == null)
        {
            Destroy(ui.gameObject);
            ui.gameObject.SetActive(false);
        }
        if (ui != null)
        {
            ui.position = panelTarget.position;
            ui.forward = cam.forward;
        }
    }
    private void OnDestroy()
    {
        try
        {
            Destroy(ui.gameObject);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

}
