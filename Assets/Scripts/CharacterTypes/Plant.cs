using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private GameObject farmhouse;
    private gameManager gm;

    private ComponentAttack attackComponent;
    private ComponentHealth healthComponent;
    private ComponentMove moveComponent;
    public CombatStats stats;

    private void Awake()
    {
        attackComponent = GetComponent<ComponentAttack>();
        healthComponent = GetComponent<ComponentHealth>();
        moveComponent = GetComponent<ComponentMove>();
        attackComponent.setStats(stats);
        healthComponent.setStats(stats);
    }
    // Start is called before the first frame update
    void Start()
    {
        farmhouse = GameObject.FindGameObjectWithTag("Farmhouse");
        gm = gameManager.current;
        setStartPosition();
        moveComponent.setStats(stats);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setStartPosition()
    {
        moveComponent.setDestination(farmhouse.transform);
    }
    private void OnDestroy()
    {
        gm.food += stats.foodOnDeath;
    }
}
