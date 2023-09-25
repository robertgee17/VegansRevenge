using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    private Garrison garrison;

    private ComponentAttack attackComponent;

    protected override void Awake()
    {
        attackComponent = GetComponent<ComponentAttack>();
        attackComponent.setStats(stats);
        base.Awake();
    }
}
