using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ComponentAttack : MonoBehaviour
{
    protected gameManager gm;
    private ComponentMove moveComponent;

    //targetting
    public ComponentHealth target;
    public LayerMask targetMask;
    private Attack.types attackType;

    private float damage, attackRate;
    private float attackCooldown = 0;
    private float range;
    private float aggroRadius;
    private float aoeRadius;

    //only used for ranged attacks
    private bool isRanged;
    private Projectile projectile;
              
    private bool inAttackAnim = false;

    private MoveAnimator anim;

    public Transform projectileSource;

    public AudioClip attackSound;
    private AudioSource soundSource;
    private SkinnedMeshRenderer renderer;
    public void setStats(CombatStats stats)
    {
        //damage
        damage = stats.damage;
        range = stats.range;
        aggroRadius = stats.aggroRadius;
        attackRate = stats.attackRate;
        aoeRadius = stats.aoeRadius;
        attackType = stats.attackType;
        isRanged = stats.isRanged;
        projectile = stats.projectile;
    }
    public void setStats(BuildingStats stats)
    {
        //damage
        damage = stats.damage;
        range = stats.range;
        aggroRadius = stats.range;
        attackRate = stats.attackRate;
        aoeRadius = stats.aoeRadius;
        isRanged = stats.isRanged;
        projectile = stats.projectile;
    }
    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
        moveComponent = GetComponent<ComponentMove>();
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        anim = GetComponent<MoveAnimator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gm = gameManager.current;
    }

    // Update is called once per frame
    void Update()
    {
        //don't do anything if the game is over
        if (gm.GameOver)
            return;
        attackCooldown -= Time.deltaTime;
        //make sure to not target dead enemies
        if (target != null && target.dead)
            target = null;

        //check if we should look for a new target
        if (lookForNewTarget())
        {
            findTarget();
        }

        //attack if target is in range
        if (targetInRange())
        {
            //stop moving
            if (moveComponent != null)
                moveComponent.stopMoving();
            attack();
        }
        //resume moving
        else if (moveComponent != null)
        {
            moveComponent.startMoving();
        }
    }
    protected virtual void attack()
    {
        if (attackCooldown > 0||inAttackAnim)
        {
            return;
        }
        if (anim != null)
        {
            if (!inAttackAnim)
            {
                inAttackAnim = true;
                anim.setTrigger("Attack");
                if (renderer.isVisible && attackSound!=null && soundSource!=null)
                {
                    soundSource.clip = attackSound;
                    soundSource.volume = AudioManager.current.calculateVolume(transform.position)*AudioManager.current.getSFXPercent();
                    soundSource.Play();
                }
            }
        }
        else
        {
            inAttackAnim = true;
            DoAttack();
        }
        Assert.IsTrue(attackRate > 0);
        attackCooldown = 1 / attackRate;
    }
    public void DoAttack()
    {
        if (!inAttackAnim)
            return;
        inAttackAnim = false;
        if (isRanged && target != null)
        {
            Instantiate(projectile, projectileSource.position, Quaternion.identity).setup(this,target);
        }
        else
        {
            DealDamage();
        }
    }
    public void DealDamage()
    {
        Attack.attack(this);
    }
    protected virtual bool targetInRange()
    {
        if (target == null) return false;
        //check if 2d distance to target is within attack range
        Vector3 thisPosNoY = new Vector3(transform.position.x, 0, transform.position.z);
        Collider targ = target.gameObject.GetComponent<Collider>();
        Vector3 closestPoint = targ.ClosestPointOnBounds(thisPosNoY);
        Vector3 vector3PointNoY = new Vector3(closestPoint.x, closestPoint.y, closestPoint.z);
        return Vector3.Magnitude(thisPosNoY - vector3PointNoY) <= range;
    }
    private bool findTarget()
    {
        ComponentHealth closestTarget = null;
        float closestDistance = float.MaxValue;
        foreach (Collider col in Physics.OverlapSphere(transform.position, aggroRadius, targetMask))
        {
            if (col.GetComponent<ComponentHealth>() != null&& !col.GetComponent<ComponentHealth>().dead)
            {
                if (closestTarget == null || get2dDistanceFrom(col.gameObject)< closestDistance)
                {
                    closestTarget= col.GetComponent<ComponentHealth>();
                    closestDistance = get2dDistanceFrom(col.gameObject);
                }
            }
        }
        if (closestTarget != null)
        {
            target = closestTarget;
            return true;
        }
        else
        {
            return false;
        }
    }
    private float get2dDistanceFrom(GameObject obj)
    {
        Vector2 thisPosNoY = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPosNoY = new Vector2(obj.transform.position.x, obj.transform.position.z);
        return Vector2.SqrMagnitude(thisPosNoY - targetPosNoY);
    }
    private bool lookForNewTarget()
    {
        return target == null;
    }
    public float getDamage()
    {
        return damage;
    }
    public float getAttackRate()
    {
        return attackRate;
    }
    public float getRange()
    {
        return range;
    }
    public float getAggroRadius()
    {
        return aggroRadius;
    }
    public float getAoeRadius()
    {
        return aoeRadius;
    }
    public void addDamage(float amt)
    {
        damage += amt;
    }
    public void decreaseDamage(float amt)
    {
        damage -= amt;
    }
    public void addAttackRate(float amt)
    {
        attackRate += amt;
    }
    public void decreaseAttackRate(float amt)
    {
        attackRate -= amt;
    }
    public void addRange(float amt)
    {
        range += amt;
        aggroRadius += amt;
    }
    public void decreaseRange(float amt)
    {
        range -= amt;
        aggroRadius -= amt;
    }
    public void addAoE(float amt)
    {
        if (attackType == Attack.types.SINGLE_TARGET)
        {
            attackType = Attack.types.AOE;
        }
        aoeRadius += amt;
    }
    public void decreaseAoE(float amt)
    {
        aoeRadius -= amt;
        if (aoeRadius<=0)
        {
            attackType = Attack.types.SINGLE_TARGET;
        }
    }
    public Attack.types getAttackType()
    {
        return attackType;
    }
}
