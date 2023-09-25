using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Attack : MonoBehaviour
{
    public enum types
    {
        SINGLE_TARGET=0,
        AOE=1
    }
    public static void attack(ComponentAttack attacker)
    {
        Assert.IsTrue(attacker.getDamage() > 0);
        if (attacker.getAttackType() == types.SINGLE_TARGET)
        {
            singleTarget(attacker.target, attacker.getDamage());
        }
        else if(attacker.getAttackType() == types.AOE)
        {
            AoE(attacker.target, attacker.getDamage(), attacker.getAoeRadius(), attacker.targetMask);
        }
    }
    public static void singleTarget(ComponentHealth target,float damage)
    {
        if (target!=null && !target.dead)
            target.TakeDamage(damage);
    }
    public static void AoE(ComponentHealth target,float damage,float radius,LayerMask targets)
    {
        //Assert.IsTrue(radius > 0);
        if (target == null)
            return;
        foreach (Collider col in Physics.OverlapSphere(target.transform.position, radius, targets))
        {
            var toHit = col.gameObject.GetComponent<ComponentHealth>();
            if (toHit != null)
                toHit.TakeDamage(damage);
        }
    }
}
