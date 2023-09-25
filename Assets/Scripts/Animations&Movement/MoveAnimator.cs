using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAnimator : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.keepAnimatorControllerStateOnDisable=true;
        anim.SetFloat("AttackRate", GetComponent<ComponentAttack>().getAttackRate());
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
    }
    public void setTrigger(string t)
    {
        anim.SetTrigger(t);
    }
}
