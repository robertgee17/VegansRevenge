using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Animal : MonoBehaviour
{
    private gameManager gm;

    private ComponentAttack attackComponent;
    private ComponentHealth healthComponent;
    private ComponentMove moveComponent;
    public CombatStats stats;
    public GarrisonStats gStats;
    public EconStats eStats;
    public gameManager.AnimalType type;

    private GameObject toGarrison;
    private void Awake()
    {
        attackComponent = GetComponent<ComponentAttack>();
        healthComponent = GetComponent<ComponentHealth>();
        moveComponent = GetComponent<ComponentMove>();
        attackComponent.setStats(stats);
        healthComponent.setStats(stats);
        GetComponent<Collider>().isTrigger = true;
        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.isKinematic = true;

    }
    // Start is called before the first frame update
    void Start()
    {
        gm = gameManager.current;
        moveComponent.setStats(stats);
        EventManager.current.onStartDefense += onStartDefense;
        EventManager.current.onStartEcon += onStartEcon;
    }
    private void OnDestroy()
    {
        try
        {
            EventManager.current.onStartDefense -= onStartDefense;
            EventManager.current.onStartEcon -= onStartEcon;
        }catch(NullReferenceException e)
        {
            Debug.Log(e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void moveTo(Vector3 pos)
    {
        moveComponent.getMotor().move(pos);  
    }
    public void setDestination(Transform t)
    {
        moveComponent.setDestination(t);
    }
    public Transform getDestination()
    {
        return moveComponent.getDestination();
    }
    public void setStartPosition()
    {
        if (moveComponent.getDestination() != null)
            return;
        GameObject obj = new GameObject();
        obj.transform.position = transform.position;
        moveComponent.setDestination(obj.transform);
    }
    public void onStartDefense()
    {
        setStartPosition();
    }
    public void onStartEcon()
    {
        //moveComponent.setDestination(null);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var collided = other.gameObject;
        //if we aren't currently trying to garrison, if the collided isn't the garrison we want, or we're in defense phase, don't do anything
        if (toGarrison == null || collided != toGarrison || gm.phase == gameManager.Phase.DEFENSE)
            return;
        var garrison = collided.GetComponent<Garrison>();
        //if the collided is a garrison, garrison
        if (garrison!=null)
        {
            //if the building is a slaughterhouse, and this animal cannot be slaughtered, don't garrison
            if (!collided.CompareTag("Slaughterhouse") || !(eStats.timeToSlaughter < 0))
                garrison.garrisonAnimal(this);
        }
        //if we collided with the trader, sell this animal
        else if (collided.CompareTag("Trader"))
        {
            collided.GetComponent<Trader>().sell(this);
        }
    }
    public void stop()
    {
        try
        {
            moveComponent.getMotor().stop();
        }
        catch (Exception e) { }
        
        moveComponent.clearDestination();
    }
    public void setToGarrison(GameObject g)
    {
        toGarrison = g;
    }

    public Sprite GetSprite()
    {
        return eStats.icon;
    }
}
