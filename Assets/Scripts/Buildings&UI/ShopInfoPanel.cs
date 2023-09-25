using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopInfoPanel : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Cost;
    public Button buyButton;
    public Trader trader;
    public TMP_Dropdown animalTypeDropdown;

    private GameObject currentObject;

    public StatDisplay food;
    public StatDisplay startingAnimalAmount;
    public StatDisplay health;
    public StatDisplay damage;
    public StatDisplay attackSpeed;
    public StatDisplay attackRange;
    public StatDisplay visionRange;
    public StatDisplay attackType;
    public StatDisplay moveSpeed;
    public StatDisplay breedTime;
    public StatDisplay breedFood;
    public StatDisplay livestockValue;
    public StatDisplay slaughterTime;
    public StatDisplay slaughterValue;
    public StatDisplay garrisonStat;
    public StatDisplay garrisonSize;

    private void Awake()
    {
        animalTypeDropdown.onValueChanged.AddListener(delegate { onDropdownChange(animalTypeDropdown); });
    }
    private void Start()
    {
        trader = gameManager.current.getTrader();
    }
    public void show()
    {
        gameObject.SetActive(true);
    }
    public void hide()
    {
        gameObject.SetActive(false);
    }
    public void Reset()
    {
        itemImage.sprite = null;
        Title.text = "";
        Description.text = "";
        Cost.text = "";
    }
    public void setImage(Sprite sprite)
    {
        itemImage.sprite = sprite;
    }
    private void onDropdownChange(TMP_Dropdown dropdown)
    {
        Building b = currentObject.GetComponent<Building>();
        if (b != null)
        {
            populateBuildingInfo(b);
        }
    }
    public void populateFoodInfo(float cost, float amount)
    {
        animalTypeDropdown.gameObject.SetActive(false);
        currentObject = null;
        Title.text = "Food";
        resetStatDisplays();
        food.setValue(amount);
        Cost.text = "Cost: " + cost;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => trader.buyFood());
    }
    public void populateAnimalInfo(Animal a)
    {
        animalTypeDropdown.gameObject.SetActive(false);
        currentObject = a.gameObject;
        populateAnimalInfo(a.tag, a.stats, a.eStats,a.gStats);
        buyButton.onClick.RemoveAllListeners();
        if (gameManager.current.gameState != gameManager.state.GAMEPLAY)
        {
            buyButton.onClick.AddListener(() => trader.buyStartingAnimal(a.type));
        }
        else
        {
            buyButton.onClick.AddListener(() => trader.buyAnimal(a.type));
        }
            
    }
    private void populateAnimalInfo(string name,CombatStats cStats,EconStats eStats,GarrisonStats gStats)
    {
        Title.text = name;
        resetStatDisplays();
        if (gameManager.current.gameState != gameManager.state.GAMEPLAY)
        {
            startingAnimalAmount.setValue(eStats.numStartingOfThisType);
        }
        health.setValue(cStats.health);
        damage.setValue(cStats.damage);
        attackSpeed.setValue(cStats.attackRate);
        attackRange.setValue(cStats.range);
        visionRange.setValue(cStats.aggroRadius);
        attackType.setValue(attackTypeToString(cStats.attackType));
        moveSpeed.setValue(cStats.moveSpeed);
        breedTime.setValue(eStats.breedTime);
        breedFood.setValue(eStats.foodToBreed);
        livestockValue.setValue(eStats.livestockSellValue);
        slaughterTime.setValue(eStats.timeToSlaughter);
        slaughterValue.setValue(eStats.meatSellValue);
        setGarrisonStats(gStats);
        if (gameManager.current.gameState != gameManager.state.GAMEPLAY)
        {
            Cost.text = "FREE";
        }
        else
        {
            Cost.text = "Cost: " + eStats.baseBuyCost;
        }
        
    }
    private void setGarrisonStats(GarrisonStats gStats)
    {
        if (gStats.healthIncrease != 0)
        {
            garrisonStat.setValue("+" + gStats.healthIncrease + " Health " + garrisonStat.statName);
        }
        else if (gStats.attackIncrease != 0)
        {
            garrisonStat.setValue("+" + gStats.attackIncrease + " Damage " + garrisonStat.statName);
        }
        else if (gStats.attackRateIncrease != 0)
        {
            garrisonStat.setValue("+" + gStats.attackRateIncrease + " Attack Rate " + garrisonStat.statName);
        }
        else if (gStats.rangeIncrease != 0)
        {
            garrisonStat.setValue("+" + gStats.rangeIncrease + " Attack Range " + garrisonStat.statName);
        }
        else if (gStats.aoeIncrease != 0)
        {
            garrisonStat.setValue("+" + gStats.aoeIncrease + " AoE " + garrisonStat.statName);
        }
}
    public void populateBuildingInfo(Building b)
    {
        buyButton.onClick.RemoveAllListeners();
        currentObject = b.gameObject;
        string name = b.tag;
        if (b.gameObject.CompareTag("Breeding Area"))
        {
            animalTypeDropdown.gameObject.SetActive(true);
            animalTypeDropdown.options.Clear();
            if (gameManager.current.gameState != gameManager.state.GAMEPLAY)
            {
                Animal a = gameManager.current.getStartingAnimal();
                animalTypeDropdown.options.Add(new TMP_Dropdown.OptionData(a.gameObject.tag, a.GetSprite()));
                name += " (" + a.tag + ")";
            }
            else
            {
                foreach(Animal a in gameManager.current.animalPrefabs)
                {
                    animalTypeDropdown.options.Add(new TMP_Dropdown.OptionData(a.gameObject.tag, a.GetSprite()));
                }
                name += "\n(" + animalTypeDropdown.options[animalTypeDropdown.value].text + ")";
            }
            animalTypeDropdown.captionText.text = getSelectedDropdownAnimal();
            gameManager.AnimalType garrisonType = gameManager.current.tagToType(getSelectedDropdownAnimal());
            buyButton.onClick.AddListener(() => trader.buyBuilding(b.type, garrisonType));
        }
        else
        {
            animalTypeDropdown.gameObject.SetActive(false);
            buyButton.onClick.AddListener(() => trader.buyBuilding(b.type));
        }
        populateBuildingInfo(name, b.stats);
    }
    private void populateBuildingInfo(string name,BuildingStats bStats)
    {
        Title.text = name;
        resetStatDisplays();
        health.setValue(bStats.health);
        garrisonSize.setValue(bStats.maxGarrisonNum);
        if (name == "Tower"||name =="Farmhouse")
        {
            damage.setValue(bStats.damage);
            attackSpeed.setValue(bStats.attackRate);
            attackRange.setValue(bStats.range);
            attackType.setValue(attackTypeToString(bStats.attackType));
        }
        if (gameManager.current.gameState!=gameManager.state.GAMEPLAY)
        {
            Cost.text = "FREE";
        }
        else
        {
            Cost.text = "Cost: " + bStats.cost;
        }
    }
    private string attackTypeToString(Attack.types type)
    {
        if (type == Attack.types.SINGLE_TARGET)
        {
            return "Single Target Attack";
        }
        else if (type == Attack.types.AOE)
        {
            return "Area of Effect Attack";
        }
        else
        {
            return "UNKNOWN ATTACK TYPE";
        }
    }
    public string getSelectedDropdownAnimal()
    {
        return animalTypeDropdown.options[animalTypeDropdown.value].text;
    }
    private void resetStatDisplays()
    {
        food.gameObject.SetActive(false);
        startingAnimalAmount.gameObject.SetActive(false);
        health.gameObject.SetActive(false);
        damage.gameObject.SetActive(false);
        attackSpeed.gameObject.SetActive(false);
        attackRange.gameObject.SetActive(false);
        visionRange.gameObject.SetActive(false);
        attackType.gameObject.SetActive(false);
        moveSpeed.gameObject.SetActive(false);
        breedTime.gameObject.SetActive(false);
        breedFood.gameObject.SetActive(false);
        livestockValue.gameObject.SetActive(false);
        slaughterTime.gameObject.SetActive(false);
        slaughterValue.gameObject.SetActive(false);
        garrisonStat.gameObject.SetActive(false);
        garrisonSize.gameObject.SetActive(false);
    }
}
