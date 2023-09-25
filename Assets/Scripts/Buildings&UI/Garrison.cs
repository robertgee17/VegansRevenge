using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Garrison : MonoBehaviour
{
    private gameManager gm;
    public List<Animal> garrison;
    public int[] numOfType;
    public Transform leaveLocation;

    private ComponentAttack attackComponent;
    private ComponentHealth healthComponent;

    private int maxCapacity = 10;

    //if this is empty, it means all animals can enter
    public List<gameManager.AnimalType> validGarrisons;
    public void Awake()
    {
        garrison = new List<Animal>();
        healthComponent = GetComponent<ComponentHealth>();
        attackComponent = GetComponent<ComponentAttack>();
    }
    public void Start()
    {
        gm = gameManager.current;
        numOfType = new int[gm.getNumAnimalTypes()];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void garrisonAnimal(Animal animal)
    {
        if (atMaxCapacity() || (validGarrisons.Count>0 && !validGarrisons.Contains(animal.type)) )
        {
            animal.setToGarrison(null);
            animal.stop();
            return;
        }

        //when we want to garrison an animal, we want to add it to the list, deselect it, then
        //deactivate all relevant parts
        garrison.Add(animal);
        numOfType[(int)animal.type]++;
        gm.deselect(animal.gameObject.GetComponent<Selectable>());
        animal.setToGarrison(null);
        animal.gameObject.SetActive(false);
        animal.gameObject.GetComponent<healthBar>().deactivate();
        //increase building's stats based off garrisoned animal
        healthComponent.addHealth(animal.gStats.healthIncrease);
        if (attackComponent != null)
        {
            attackComponent.addDamage(animal.gStats.attackIncrease);
            attackComponent.addAttackRate(animal.gStats.attackRateIncrease);
            attackComponent.addRange(animal.gStats.rangeIncrease);
            attackComponent.addAoE(animal.gStats.aoeIncrease);
        }
        EventManager.current.garrisonEnter(gameObject,animal);
    }

    public void halfOfTypeLeave(gameManager.AnimalType type)
    {
        List<Animal> toRemove = new List<Animal>();
        int numberToRemove = (int)Mathf.Ceil((float)numOfType[(int)type] / 2);
        for (int i = 0; i < numberToRemove; i++)
        {
            animalLeave(type);
        }
    }
    public void animalLeave(gameManager.AnimalType type)
    {
        for (int i = 0; i < getNumGarrisoned(); i++)
        {
            if (garrison[i].type == type)
            {
                animalLeave(i);
                return;
            }
        }
    }
    public void animalLeave(int index)
    {
        //make sure that the index isn't out of bounds
        if (index >= garrison.Count||gameObject.CompareTag("Slaughterhouse"))
            return;
        //activate animal
        var animal = garrison[index];
        animal.gameObject.SetActive(true);
        animal.gameObject.GetComponent<healthBar>().activate();
        //place it in a random location near the leave position
        float randomx = Random.Range(-1, 1);
        float randomz = Random.Range(-1, 1);
        Vector3 pos = new Vector3(leaveLocation.position.x + randomx, leaveLocation.position.y, leaveLocation.position.z + randomz);

        animal.gameObject.transform.position = pos;
        animal.gameObject.transform.rotation = leaveLocation.rotation;
        removeAnimal(animal,false);
    }
    public int getNumGarrisoned()
    {
        return garrison.Count;
    }
    public Animal animalAt(int index)
    {
        if (index >= garrison.Count)
        {
            return null;
        }
        return garrison[index];
    }
    public void removeAnimal(Animal animal,bool toSlaughter)
    {
        if (animal == null)
        {
            Debug.Log("ANIMAL IS NULL IN GARRISON REMOVE");
            return;
        }
        if (numOfType[(int)animal.type] <= 0)
        {
            return;
        }

        //decrease increased garrison stats
        healthComponent.decreaseHealth(animal.gStats.healthIncrease);
        if (attackComponent != null)
        {
            attackComponent.decreaseDamage(animal.gStats.attackIncrease);
            attackComponent.decreaseAttackRate(animal.gStats.attackRateIncrease);
            attackComponent.decreaseRange(animal.gStats.rangeIncrease);
            attackComponent.decreaseAoE(animal.gStats.aoeIncrease);
        }
        numOfType[(int)animal.type]--;
        garrison.Remove(animal);
        if (!toSlaughter)
        {
            animal.stop();
            animal.setToGarrison(null);
        }
        EventManager.current.garrisonLeave(gameObject);
    }
    public void setMaxCapacity(int capacity)
    {
        maxCapacity = capacity;
    }
    public bool atMaxCapacity()
    {
        return getNumGarrisoned() >= maxCapacity;
    }
    public int getMaxCapacity()
    {
        return maxCapacity;
    }
    public void setValidGarrison(gameManager.AnimalType type)
    {
        validGarrisons.Clear();
        validGarrisons.Add(type);
    }
    
}
