using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BreedingButton : MonoBehaviour
{
    private Garrison garrison;// Start is called before the first frame update
    private Breeding breedingArea;
    private Button button;
    public Button exitAllButton;
    private gameManager.AnimalType animalType;
    public Image animalImage;
    public Image fillImage;
    public TextMeshProUGUI unitCount;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void setup(Breeding b,Garrison g, gameManager.AnimalType type)
    {
        breedingArea = b;
        garrison = g;
        animalType = type;
        animalImage.sprite = gameManager.current.animalPrefabs[(int)animalType].GetSprite();
        button.onClick.AddListener(() => garrison.animalLeave(animalType));
        exitAllButton.onClick.AddListener(() => garrison.halfOfTypeLeave(animalType));
    }

    private void Update()
    {
        fillImage.fillAmount = breedingArea.timeSpentBreeding[(int)animalType]/ gameManager.current.animalPrefabs[(int)animalType].eStats.breedTime;
    }

    public gameManager.AnimalType getAnimalType()
    {
        return animalType;
    }
    public void setUnitCount(int num)
    {
        unitCount.text = num.ToString();
    }
    public void setTextColor(Color color)
    {
        unitCount.color = color;
    }
}
