using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GarrisonUI : MonoBehaviour
{
    private gameManager gm;
    private List<Button> garrisonButtons;
    private List<TextMeshProUGUI> garrisonText;
    public FillBar breedBackground;
    public FillBar slaughterBackground;
    public Button buttonPrefab;
    public TextMeshProUGUI textPrefab;
    public SlaughterhouseUI sUI;
    private void Awake()
    {
        garrisonButtons = new List<Button>();
        garrisonText = new List<TextMeshProUGUI>();
        gm = FindObjectOfType<gameManager>();
        EventManager.current.onSelect += onSelect;
        EventManager.current.onGarrisonEnter += onGarrisonEnter;
        EventManager.current.onGarrisonLeave += onGarrisonLeave;
        for (int i = 0; i < gm.getNumAnimalTypes(); i++)
        {
            TextMeshProUGUI t = Instantiate(textPrefab, transform);
            Button b = Instantiate(buttonPrefab, transform);
            garrisonButtons.Add(b);
            garrisonText.Add(t);
            b.gameObject.SetActive(false);
            t.gameObject.SetActive(false);
        }
        sUI.setup();
        if (gm.selected != null)
        {
            onSelect(gm.selected.gameObject);
        }
    }
    private void onGarrisonEnter(GameObject obj,Animal a)
    {
        var garrison = obj.GetComponent<Garrison>();
        //if the garrison happening is not happening in the selected garrison, ignore it for UI purposes
        //or ignore it if it isn't a garrison
        if (gm.selected == null || obj != gm.selected.gameObject||garrison==null)
        {
            return;
        }
        //if the garrison happened in the selected garrison, update ui
        updateUI(obj);

    }
    private void onGarrisonLeave(GameObject obj)
    {
        var garrison = obj.GetComponent<Garrison>();
        //if the garrison happening is not happening in the selected garrison, ignore it for UI purposes
        //or ignore it if it isn't a garrison
        if (gm.selected == null || obj != gm.selected.gameObject || garrison == null)
        {
            return;
        }
        //if the garrison happened in the selected garrison, update ui
        updateUI(obj);
    }
    private void onSelect(GameObject obj)
    {
        var garrison = obj.GetComponent<Garrison>();
        //if the selected object is a garrison, update ui info
        if (garrison == null)
            return;
        
        updateUI(obj);
        updateButtonFuncs(garrison);
    }
    private void updateUI(GameObject obj)
    {
        if(!obj.CompareTag("Slaughterhouse"))
        {
            updateGarrisonUI(obj.GetComponent<Garrison>());
            sUI.hide();
            sUI.enabled = false;
        }
        else
        {
            hideGarrisonUI();
            sUI.enabled = true;
            sUI.setBuildings(obj);
        }
    }
    private void updateButtonFuncs(Garrison garrison)
    {
        for (int i = 0; i < gm.getNumAnimalTypes(); i++)
        {
            Button b = garrisonButtons[i];
            int index = i;
            b.onClick.RemoveAllListeners();
            //apparently this function pointer passes i by pointer, not reference, so make sure to save which
            //value is being passed thru, or else it will make update the parameter if you use i
            b.onClick.AddListener(() => garrison.animalLeave((gameManager.AnimalType)index));
        }
    }
    private void updateGarrisonUI(Garrison garrison)
    {
        for (int i = 0; i < gm.getNumAnimalTypes(); i++)
        {
            Button b = garrisonButtons[i];
            TextMeshProUGUI animalCount = garrisonText[i];
            //if there are animals of a given type in the 
            if (garrison.numOfType[i] > 0)
            {
                animalCount.text = "Number of " + gm.animalPrefabs[i].tag + "s: " + garrison.numOfType[i];
                b.GetComponentInChildren<TextMeshProUGUI>().text = "Remove a " + gm.animalPrefabs[i].tag;
                b.gameObject.SetActive(true);
                animalCount.gameObject.SetActive(true);
            }
            //if there aren't any animals of a type, decativate buttons regarding that animal
            else
            {
                b.gameObject.SetActive(false);
                animalCount.gameObject.SetActive(false);
            }
        }
    }
    void hideGarrisonUI()
    {
        for (int i = 0; i < gm.getNumAnimalTypes(); i++)
        {
            Button b = garrisonButtons[i];
            TextMeshProUGUI animalCount = garrisonText[i];
            //if there are animals of a given type in the 
            b.gameObject.SetActive(false);
            animalCount.gameObject.SetActive(false);
        }
    }

}
