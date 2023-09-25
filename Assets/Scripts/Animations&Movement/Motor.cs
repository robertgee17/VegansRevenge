using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Motor : MonoBehaviour
{
    public float RotationSpeed = .125f;
    private NavMeshAgent agent;
    private Transform target;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public void moveToTarget()
    {
        if (target == null)
            return;
        move(target.position);
        //Debug.Log("MOVING");
    }
    public void move(Vector3 pos)
    {
        //nullTarget();
        agent.isStopped = false;
        agent.SetDestination(pos);
    }
    public void stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        nullTarget();
    }
    public void nullTarget()
    {
        target = null;
    }
    public void setTarget(Transform t)
    {
        if (t!=null&&!gameManager.current.isBuilding(t.gameObject))
        {
            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(t.position, out myNavHit, 100, -1))
            {
                t.position = myNavHit.position;
            }
        }
        target = t;
    }
    public Transform getTarget()
    {
        return target;
    }
    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector2 targPos = new Vector2(target.position.x, target.position.z);
            Vector2 transPos = new Vector2(transform.position.x, transform.position.z);
            if ((targPos - transPos) != Vector2.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(target.position - transform.position);
                Quaternion lerp = Quaternion.Lerp(transform.rotation, targetRot, RotationSpeed);
                transform.rotation = lerp;
            }
        }
    }
}
