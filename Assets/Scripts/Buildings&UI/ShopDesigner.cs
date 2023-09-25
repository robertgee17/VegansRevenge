using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopDesigner : MonoBehaviour
{
    public ButtonPrefab buttonPrefab;
    private gameManager gm;
    private BuildingPlacer bp;
    private List<Button> itemButtons;
    private List<gameManager.AnimalType> buyableAnimals;
    private List<gameManager.BuildingType> buyableBuildings;
    public ShopInfoPanel infoPanel;
    public Sprite foodSprite;

    private void Awake()
    {
        buyableAnimals = new List<gameManager.AnimalType>();
        buyableBuildings = new List<gameManager.BuildingType>();
        itemButtons = new List<Button>();
        bp = FindObjectOfType<BuildingPlacer>();
    }
    private void Start()
    {
        bp.setShop(this);
        gm = gameManager.current;
        setupShop();
        infoPanel.hide();
    }
    private void setupShop()
    {
        buyableAnimals = gm.getTrader().getBuyableAnimals();
        buyableBuildings = gm.getTrader().getBuyableBuildings();
        //make first object food
        if (gm.getTrader().isFoodBuyable())
        {
            string cost;
            if (gameManager.current.gameState != gameManager.state.GAMEPLAY)
            {
                cost = "FREE";
            }
            else
            {
                cost = gm.getTrader().foodBuyCost.ToString();
            }
            ButtonPrefab foodButton = Instantiate(buttonPrefab, transform);
            foodButton.GetComponentInChildren<TextMeshProUGUI>().text = "Food\n"+cost;
            foodButton.button.onClick.AddListener(() => infoPanel.gameObject.SetActive(true));
            foodButton.button.onClick.AddListener(() => infoPanel.populateFoodInfo(gm.getTrader().foodBuyCost, gm.getTrader().foodBuyAmt));
            foodButton.button.onClick.AddListener(() => infoPanel.setImage(foodSprite));
            foodButton.GetComponent<ButtonDoubleClickListener>().onDoubleClick.AddListener(() => gm.getTrader().buyFood());
            foodButton.icon.sprite = foodSprite;
            itemButtons.Add(foodButton.button);
        }
        for (int i = 0; i < buyableAnimals.Count; i++)
        {
            ButtonPrefab animalButton = Instantiate(buttonPrefab, transform);
            int index = i;
            Animal animal = gm.animalPrefabs[(int)buyableAnimals[index]];
            string cost;
            if (gameManager.current.gameState != gameManager.state.GAMEPLAY)
            {
                animalButton.GetComponent<ButtonDoubleClickListener>().onDoubleClick.AddListener(() => gm.getTrader().buyStartingAnimal(animal.type));
                cost = "FREE";
            }
            else
            {
                animalButton.GetComponent<ButtonDoubleClickListener>().onDoubleClick.AddListener(() => gm.getTrader().buyAnimal(animal.type));
                cost = animal.eStats.baseBuyCost.ToString();
            }
            animalButton.GetComponentInChildren<TextMeshProUGUI>().text = animal.tag+"\n"+ cost;
            animalButton.button.onClick.AddListener(() => infoPanel.gameObject.SetActive(true));
            animalButton.button.onClick.AddListener(() => infoPanel.populateAnimalInfo(animal));
            animalButton.button.onClick.AddListener(() => infoPanel.setImage(animal.eStats.icon));


            animalButton.icon.sprite = gm.animalPrefabs[(int)buyableAnimals[index]].eStats.icon;
            itemButtons.Add(animalButton.button);
        }
        //finally add buildings
        for (int i = 0; i < buyableBuildings.Count; i++)
        {
            ButtonPrefab buildingButton = Instantiate(buttonPrefab, transform);
            int index = i;
            Building building = gm.buildingPrefabs[(int)buyableBuildings[index]];
            string cost;
            if (gameManager.current.gameState != gameManager.state.GAMEPLAY)
            {
                cost = "FREE";
            }
            else
            {
                cost = building.stats.cost.ToString();
            }
            buildingButton.GetComponentInChildren<TextMeshProUGUI>().text = gm.buildingPrefabs[(int)buyableBuildings[index]].tag + "\n" + cost;
            buildingButton.button.onClick.AddListener(() => infoPanel.gameObject.SetActive(true));
            buildingButton.button.onClick.AddListener(() => infoPanel.populateBuildingInfo(building));
            buildingButton.button.onClick.AddListener(() => infoPanel.setImage(building.stats.icon));
            //don't allow double click buying for breeding areas since you need to select the type
            if(!building.CompareTag("Breeding Area"))
            {
                buildingButton.GetComponent<ButtonDoubleClickListener>().onDoubleClick.AddListener(() => gm.getTrader().buyBuilding(building.type));
            }
            buildingButton.icon.sprite = building.stats.icon;
            itemButtons.Add(buildingButton.button);
        }
    }
    private void OnEnable()
    {
        infoPanel.gameObject.SetActive(false);
        if (gm != null)
        {
            setupShop();
        }
    }
    private void OnDisable()
    {
        removeChildren();
    }
    private void removeChildren()
    {
        buyableAnimals.Clear();
        buyableBuildings.Clear();
        int i = 0;

        //Array to hold all child obj
        GameObject[] allChildren = new GameObject[transform.childCount];

        //Find all child obj and store to that array
        foreach (Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }

        //Now destroy them
        foreach (GameObject child in allChildren)
        {
            Destroy(child.gameObject);
        }

    }
}
