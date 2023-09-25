using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveList : MonoBehaviour
{
    public static WaveList current;
    public int numWaves = 10;
    public List<gameManager.PlantType>[] waves;
    public List<int> waveTimer;

    public float waveSizeRange = .2f;
    public int peaPlantStartWave = 4;
    public int mushroomStartWave = 8;
    private void Awake()
    {
        if (current != this && current != null)
        {
            Destroy(gameObject);
        }
        else
        {
            current = this;
        }

        waves = new List<gameManager.PlantType>[numWaves];
        waveTimer = new List<int>();

        for (int i = 0; i < numWaves; i++)
        {
            waves[i] = new List<gameManager.PlantType>();
        }
        //Wave 1
        //10 tank plants
        addNumOfTypeToWave(0, gameManager.PlantType.TANK, 10);
        addNumOfTypeToWave(1, gameManager.PlantType.PEA, 0);
        waveTimer.Add(120);

        //Wave 2
        addNumOfTypeToWave(1, gameManager.PlantType.TANK, 15);
        addNumOfTypeToWave(1, gameManager.PlantType.PEA, 5);
        waveTimer.Add(120);

        //Wave 3
        addNumOfTypeToWave(2, gameManager.PlantType.TANK, 5);
        addNumOfTypeToWave(2, gameManager.PlantType.PEA, 20);
        waveTimer.Add(90);

        //Wave 4
        addNumOfTypeToWave(3, gameManager.PlantType.TANK, 30);
        addNumOfTypeToWave(3, gameManager.PlantType.PEA, 0);
        waveTimer.Add(90);

        //Wave 5
        addNumOfTypeToWave(4, gameManager.PlantType.TANK, 5);
        addNumOfTypeToWave(4, gameManager.PlantType.PEA, 30);
        waveTimer.Add(90);

        //Wave 6
        addNumOfTypeToWave(5, gameManager.PlantType.TANK, 25);
        addNumOfTypeToWave(5, gameManager.PlantType.PEA, 15);
        waveTimer.Add(90);

        //Wave 7
        addNumOfTypeToWave(6, gameManager.PlantType.TANK, 20);
        addNumOfTypeToWave(6, gameManager.PlantType.PEA, 25);
        waveTimer.Add(90);
        //Wave 8
        addNumOfTypeToWave(7, gameManager.PlantType.TANK, 0);
        addNumOfTypeToWave(7, gameManager.PlantType.PEA, 50);
        addNumOfTypeToWave(7, gameManager.PlantType.MUSHROOOM, 1);
        waveTimer.Add(90);

        //Wave 9
        addNumOfTypeToWave(8, gameManager.PlantType.TANK, 35);
        addNumOfTypeToWave(8, gameManager.PlantType.PEA, 20);
        addNumOfTypeToWave(8, gameManager.PlantType.MUSHROOOM, 2);
        waveTimer.Add(90);

        //Wave 10
        addNumOfTypeToWave(9, gameManager.PlantType.TANK, 30);
        addNumOfTypeToWave(9, gameManager.PlantType.PEA, 30);
        addNumOfTypeToWave(9, gameManager.PlantType.MUSHROOOM, 3);
        waveTimer.Add(90);
    }
    private void addNumOfTypeToWave(int waveIndex, gameManager.PlantType type, int amt)
    {
        for(int i = 0; i < amt; i++)
        {
            waves[waveIndex].Add(type);
        }
    }

    public int getNumWaves()
    {
        if (GamemodeType.isInfinite)
        {
            return -1;
        }
        else
        {
            return numWaves;
        }
    }
    public List<gameManager.PlantType> getWave(int waveIndex)
    {

        int waveNum = waveIndex + 1;
        int numToSpawn;
        if (GamemodeType.isInfinite)
        {
            Debug.Log("giving infinite wave");
            numToSpawn = Mathf.CeilToInt(10 + waveNum * Mathf.Log(waveNum, 1.5f));
            numToSpawn = randomizeNumSpawned(numToSpawn);
            List<gameManager.PlantType> list = new List<gameManager.PlantType>();
            
            for(int i=0;i< numToSpawn; i++)
            {
                list.Add(gameManager.PlantType.TANK);
            }
            if (waveNum > peaPlantStartWave)
            {
                int num = peaPlantStartWave - waveNum;
                numToSpawn = Mathf.CeilToInt(5 + num * Mathf.Log(num, 1.5f));
                numToSpawn = randomizeNumSpawned(numToSpawn);
                for (int i = 0; i < numToSpawn; i++)
                {
                    list.Add(gameManager.PlantType.PEA);
                }
            }
            if (waveNum > mushroomStartWave)
            {
                int num = mushroomStartWave - waveNum;
                numToSpawn = num;
                numToSpawn = randomizeNumSpawned(numToSpawn);
                for (int i = 0; i < numToSpawn; i++)
                {
                    list.Add(gameManager.PlantType.MUSHROOOM);
                }
            }
            return list;
        }
        else
        {
            return waves[waveIndex];
        }
    }
    int randomizeNumSpawned(int numToSpawn)
    {
        float randomize = Random.Range(1-waveSizeRange, 1+ waveSizeRange);
        return Mathf.CeilToInt(numToSpawn * randomize);
    }
    public int getWaveTime(int i)
    {
        if (GamemodeType.isInfinite)
        {
            return 75;
        }
        else
        {
            return waveTimer[i];
        }
    }
}
