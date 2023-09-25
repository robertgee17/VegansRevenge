using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Slaughterhouse : MonoBehaviour
{
    private gameManager gm;
    private Garrison garrison;
    private int slaughterIndex = 0;
    public int numKillableAtATime = 2;
    public List<float> timeRemaining;

    private void Awake()
    {
        timeRemaining = new List<float>();
        garrison = GetComponent<Garrison>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gm = gameManager.current;
        EventManager.current.onGarrisonEnter += addAnimal;
    }
    private void addAnimal(GameObject obj,Animal a)
    {
        if (obj == gameObject)
        {
            timeRemaining.Add(a.eStats.timeToSlaughter);
        }
    }

    private void Update()
    {
        for(int i = 0; i < numKillableAtATime && i<timeRemaining.Count; i++)
        {
            timeRemaining[i] -= Time.deltaTime;
            if (timeRemaining[i] <= 0)
            {
                
                Animal a = garrison.animalAt(i);
                if (a == null)
                {
                    Debug.Log("ANIMAL NULL");
                    continue;
                }
                Debug.Log("ANIMAL SLAUGHTERED");
                garrison.removeAnimal(a,true);
                gm.coin += a.eStats.meatSellValue;
                Destroy(a.gameObject);
            }
        }
        for(int i = 0; i < timeRemaining.Count; i++)
        {
            if (timeRemaining[i] <= 0)
            {
                timeRemaining.RemoveAt(i);
            }
        }
    }
}
