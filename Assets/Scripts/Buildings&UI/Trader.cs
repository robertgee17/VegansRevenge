using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour
{
    private gameManager gm;
    public Transform spawnPoint;
    private BuildingPlacer bp;
    public int foodBuyCost = 50;
    public int foodBuyAmt = 100;
    private void Awake()
    {
        bp = FindObjectOfType<BuildingPlacer>();
    }
    private void Start()
    {
        gm = gameManager.current;
    }
    public void sell(Animal a)
    {
        gm.coin+=a.eStats.livestockSellValue;
        gm.deselect(a.GetComponent<Selectable>());
        Destroy(a.gameObject);
    }
    public void buyFood()
    {
        if (gm.coin < foodBuyCost)
        {
            return;
        }
        //reduce coins of user by cost
        gm.coin -= foodBuyCost;
        gm.food += foodBuyAmt;
        if (gm.isTutorial()&&gameManager.current.GetTutorialState() == TutorialInputManager.TutorialState.FOOD)
        {
            gameManager.current.nextState();
        }
        buySFX();
    }
    public void buyStartingAnimal(gameManager.AnimalType type)
    {
        Animal a = gm.animalPrefabs[(int)type];
        for(int i = 0; i < a.eStats.numStartingOfThisType; i++)
        {
            float randomx = Random.Range(-1, 1);
            float randomz = Random.Range(-1, 1);
            Vector3 pos = new Vector3(spawnPoint.position.x + randomx, spawnPoint.position.y, spawnPoint.position.z + randomz);

            Animal newAnimal = gm.createAnimal(type, pos);
            newAnimal.gameObject.transform.rotation = spawnPoint.rotation;
        }
        gameManager.current.startingAnimal = type;
        gm.nextState();
        EventManager.current.deselect();
        if (!gameManager.current.isTutorial())
        {
            gm.selectTrader();
        }
        buySFX();
    }
    public void buyAnimal(gameManager.AnimalType type)
    {
        Animal a = gm.animalPrefabs[(int)type];
        //if you dont have enough coins, dont allow buy
        if (gm.coin < a.eStats.baseBuyCost)
        {
            return;
        }
        //reduce coins of user by cost
        gm.coin -= a.eStats.baseBuyCost;
        //set spawn point
        float randomx = Random.Range(-1, 1);
        float randomz = Random.Range(-1, 1);
        Vector3 pos = new Vector3(spawnPoint.position.x + randomx, spawnPoint.position.y, spawnPoint.position.z + randomz);
        Animal newAnimal = gm.createAnimal(type,pos);
        newAnimal.gameObject.transform.rotation = spawnPoint.rotation;
        buySFX();
    }
    public void buyBuilding(gameManager.BuildingType type)
    {
        Building b = gm.buildingPrefabs[(int)type];
        float cost;
        if (gameManager.current.gameState != gameManager.state.GAMEPLAY || gm.isTutorial())
        {
            cost = 0;
        }
        else
        {
            cost = b.stats.cost;
        }
            //if you dont have enough coins, dont allow buy
        if (gm.coin < cost)
        {
            return;
        }
        //reduce coins of user by cost
        bp.enabled = true;
        bp.selectBuilding(type,cost);
        EventManager.current.deselect();
    }
    public void buyBuilding(gameManager.BuildingType type,gameManager.AnimalType garrisonType)
    {
        Building b = gm.buildingPrefabs[(int)type];
        float cost;
        if (gameManager.current.gameState != gameManager.state.GAMEPLAY || gm.isTutorial())
        {
            cost = 0;
        }
        else
        {
            cost = b.stats.cost;
        }
        //if you dont have enough coins, dont allow buy
        if (gm.coin < cost)
        {
            return;
        }
        //reduce coins of user by cost
        bp.enabled = true;
        bp.selectBuilding(type,garrisonType, cost);
        EventManager.current.deselect();
    }
    public List<gameManager.AnimalType> getBuyableAnimals()
    {
        List<gameManager.AnimalType> list = new List<gameManager.AnimalType>();
        if (gm.isTutorial())
        {
            if (gm.GetTutorialState() == TutorialInputManager.TutorialState.ANIMAL)
            {
                list.Add(gameManager.AnimalType.CHICKEN);
            }
        }
        else
        {
            if (gm.gameState == gameManager.state.FARMHOUSE)
            {

            }
            else if (gm.gameState == gameManager.state.ANIMAL)
            {
                for (int i = 0; i < gm.startingAnimals.Length; i++)
                {
                    list.Add(gm.startingAnimals[i]);
                }
            }
            else if (gm.gameState == gameManager.state.BREEDING_AREA)
            {

            }
            else
            {
                for (int i = 0; i < gm.getNumAnimalTypes(); i++)
                {
                    list.Add((gameManager.AnimalType)i);
                }
            }
        }
        return list;
    }
    public List<gameManager.BuildingType> getBuyableBuildings()
    {
        List<gameManager.BuildingType> list = new List<gameManager.BuildingType>();
        if (gm.isTutorial())
        {
            if (gm.GetTutorialState() == TutorialInputManager.TutorialState.FARMHOUSE)
            {
                list.Add(gameManager.BuildingType.FARMHOUSE);
            }
            else if (gm.GetTutorialState() == TutorialInputManager.TutorialState.BREEDING_AREA)
            {
                list.Add(gameManager.BuildingType.BREEDINGAREA);
            }
            else if (gm.GetTutorialState() == TutorialInputManager.TutorialState.SLAUGHTERHOUSE)
            {
                list.Add(gameManager.BuildingType.SLAUGHTERHOUSE);
            }
            else if (gm.GetTutorialState() == TutorialInputManager.TutorialState.TOWER)
            {
                list.Add(gameManager.BuildingType.TOWER);
            }
        }
        else
        {
            if (gm.gameState == gameManager.state.FARMHOUSE)
            {
                list.Add(gameManager.BuildingType.FARMHOUSE);
            }
            else if (gm.gameState == gameManager.state.ANIMAL)
            {

            }
            else if (gm.gameState == gameManager.state.BREEDING_AREA)
            {
                list.Add(gameManager.BuildingType.BREEDINGAREA);
            }
            else
            {
                //make all buildings buyable except for the farmhouse(which is index 0)
                for (int i = 1; i < gm.getNumBuildingTypes(); i++)
                {
                    list.Add((gameManager.BuildingType)i);
                }
            }
        }
        
        return list;
    }

    public bool isFoodBuyable()
    {
        return gm.gameState == gameManager.state.GAMEPLAY||(gm.isTutorial()&&gm.GetTutorialState()==TutorialInputManager.TutorialState.FOOD);
    }
    public void buySFX()
    {
        AudioSource s = GetComponent<AudioSource>();
        s.volume = AudioManager.current.getSFXPercent();
        s.Play();
    }
}
