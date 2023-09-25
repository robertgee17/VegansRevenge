using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BreedingPanelUI : MonoBehaviour
{
    public GameObject garrisonPanel;
    public BreedingButton breedingButton;
    public Transform panelTarget;
    private Garrison garrison;
    private Breeding breedingArea;

    private List<BreedingButton> buttons;
    Transform ui;
    Transform cam;
    // Start is called before the first frame update
    private void Start()
    {
        buttons = new List<BreedingButton>();
        cam = Camera.main.transform;
        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                breedingArea = GetComponent<Breeding>();
                garrison = GetComponent<Garrison>();

                ui = Instantiate(garrisonPanel, c.transform).transform;
                ui.gameObject.SetActive(true);

                List<gameManager.AnimalType> animalList;
                //if the valid garrisons is empty, that means all animals can garrison
                if (garrison.validGarrisons.Count != 0)
                {
                    animalList = garrison.validGarrisons;
                }
                else
                {
                    animalList = gameManager.current.GetAnimalTypes();
                }
                foreach (gameManager.AnimalType type in animalList)
                {
                    BreedingButton b = Instantiate(breedingButton, ui);
                    b.setup(breedingArea,garrison, type);
                    buttons.Add(b);
                }

                break;
            }
        }

        EventManager.current.onGarrisonEnter += onGarrisonEnter;
        EventManager.current.onGarrisonLeave += onGarrisonLeave;
    }


    private void onGarrisonEnter(GameObject obj, Animal a)
    {
        var buildingGarrison = obj.GetComponent<Garrison>();
        //if the garrison happening is not happening in the selected garrison, ignore it for UI purposes
        //or ignore it if it isn't a garrison
        if (buildingGarrison == null || buildingGarrison != garrison)
        {
            return;
        }
        //if the garrison happened in the selected garrison, update ui
        updateButtons(buildingGarrison);

    }
    private void onGarrisonLeave(GameObject obj)
    {
        var buildingGarrison = obj.GetComponent<Garrison>();
        //if the garrison happening is not happening in the selected garrison, ignore it for UI purposes
        //or ignore it if it isn't a garrison
        if (buildingGarrison == null || buildingGarrison != garrison)
        {
            return;
        }
        //if the garrison happened in the selected garrison, update ui
        updateButtons(buildingGarrison);
    }

    private void updateButtons(Garrison garrison)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            BreedingButton b = buttons[i];

            b.setUnitCount(garrison.numOfType[(int)b.getAnimalType()]);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            BreedingButton b = buttons[i];
            if (breedingArea.canBreed[(int)b.getAnimalType()])
            {
                b.setTextColor(Color.white);
            }
            else
            {
                b.setTextColor(Color.red);
            }
        }
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
            EventManager.current.onGarrisonEnter -= onGarrisonEnter;
            EventManager.current.onGarrisonLeave -= onGarrisonLeave;
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.Message);
        }
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
