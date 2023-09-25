using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ComponentMove : MonoBehaviour
{
    private gameManager gm;
    //movement
    private NavMeshAgent agent;
    private Motor motor;
    //attacking
    private ComponentAttack attackComponent;
    //destinations
    private Transform defaultDestination;

    //for debugging purposes
    protected bool foundTarget;
    protected float distanceToTarget = 0;

    private bool shouldMove = true;

    public void setStats(CombatStats stats)
    {
        agent.speed = stats.moveSpeed;
        agent.acceleration = stats.moveSpeed;
        agent.angularSpeed = 1000 * stats.moveSpeed;
        agent.baseOffset = 0;
        agent.autoTraverseOffMeshLink = false;
    }
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attackComponent = GetComponent<ComponentAttack>();
        motor = GetComponent<Motor>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gm = gameManager.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.GameOver)
            return;
        // if there is no target in range, move towards the default position
        // (should be starting point for animals, farmhouse for plants)
        if (attackComponent.target == null)
        {
            motor.setTarget(defaultDestination);
        }
        else
        {
            motor.setTarget(attackComponent.target.transform);
        }
        if (shouldMove)
        {
            motor.moveToTarget();
        }
        else
        {
            motor.stop();
        }
    }
    public void setDestination(Transform t)
    {
        defaultDestination = t;
    }
    public void clearDestination()
    {
        defaultDestination = null;
    }
    public Transform getDestination()
    {
        return defaultDestination;
    }
    public void startMoving()
    {
        shouldMove = true;
    }
    public void stopMoving()
    {
        shouldMove = false;
    }
    public Motor getMotor()
    {
        return motor;
    }
    public float getSpeed()
    {
        return agent.speed;
    }
}
