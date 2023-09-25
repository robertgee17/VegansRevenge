using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    protected gameManager gm;

    protected ComponentHealth healthComponent;
    public BuildingStats stats;
    public gameManager.BuildingType type;
    public MeshRenderer[] renderers;
    
    protected virtual void Awake()
    {
        healthComponent = GetComponent<ComponentHealth>();
        healthComponent.setStats(stats);
    }
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<gameManager>();
        if (GetComponent<Garrison>() != null)
        {
            GetComponent<Garrison>().setMaxCapacity(stats.maxGarrisonNum);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void repairButton()
    {
        if (canAffordRepair())
        {
            gameManager.current.coin -= getRepairCost();
            healthComponent.healToFull();
        }
    }
    public int getRepairCost()
    {
        return (int)Mathf.Ceil((healthComponent.getMaxHealth() - healthComponent.getCurrentHealth())/2);
    }
    public bool canAffordRepair()
    {
        return gameManager.current.coin >= getRepairCost();
    }
    public bool atFullHP()
    {
        return healthComponent.getCurrentHealth() >= healthComponent.getMaxHealth();
    }
}
