using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GarrisonButton : MonoBehaviour
{
    private Garrison garrison;// Start is called before the first frame update
    private Button button;
    public Button exitAllButton;
    private gameManager.AnimalType animalType;
    public Image image;
    public TextMeshProUGUI unitCount;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void setup(Garrison g,gameManager.AnimalType type)
    {
        garrison = g;
        animalType = type;
        image.sprite = gameManager.current.animalPrefabs[(int)animalType].GetSprite();
        button.onClick.AddListener(() => garrison.animalLeave(animalType));
        exitAllButton.onClick.AddListener(() => garrison.halfOfTypeLeave(animalType));
    }

    public gameManager.AnimalType getAnimalType()
    {
        return animalType;
    }
    public void setUnitCount(int num)
    {
        unitCount.text = num.ToString();
    }
}
