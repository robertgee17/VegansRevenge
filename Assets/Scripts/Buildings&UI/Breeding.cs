using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Breeding : MonoBehaviour
{
    private gameManager gm;
    private Garrison garrison;
    public float[] timeSpentBreeding;
    public bool[] canBreed;
    private bool isFarmhouse;
    private void Awake()
    {
        garrison = GetComponent<Garrison>();
        isFarmhouse = gameObject.CompareTag("Farmhouse");
    }
    // Start is called before the first frame update
    void Start()
    {
        gm = gameManager.current;
        timeSpentBreeding = new float[gm.getNumAnimalTypes()];
        canBreed = new bool[gm.getNumAnimalTypes()];
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < gm.getNumAnimalTypes(); i++)
        {
            int numAnimals;
            try
            {
                numAnimals = garrison.numOfType[i];
            }
            catch(Exception e)
            {
                Debug.Log("array len: " + garrison.numOfType.Length);
                Debug.Log("index: " + i);
                throw e;
            }
            Animal animal = gm.animalPrefabs[i];
            //if there are 1 or fewer animals of a type, reset the time spent breeding
            if (numAnimals <= 1 || garrison.atMaxCapacity()|| gm.food < animal.eStats.foodToBreed)
            {
                timeSpentBreeding[i] = 0;
                canBreed[i] = false;
                continue;
            }
            else
            {
                canBreed[i] = true;
            }
            //increment time spent breeding
            if (!isFarmhouse)
            {
                timeSpentBreeding[i] += Time.deltaTime * (numAnimals / 2);
            }
            else
            {
                timeSpentBreeding[i] += (Time.deltaTime * (numAnimals / 2))/2;
            }
            //if spent enough time breeding and have enough food, breed the animal
            if (timeSpentBreeding[i] >= animal.eStats.breedTime && gm.food >= animal.eStats.foodToBreed)
            {
                garrison.garrisonAnimal(gm.createAnimal((gameManager.AnimalType)i, Vector3.zero));
                gm.food -= animal.eStats.foodToBreed;
                timeSpentBreeding[i] = 0;
            }
        }
        
        
    }
}
