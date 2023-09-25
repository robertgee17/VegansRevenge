using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHealth : MonoBehaviour
{
    public event System.Action<float, float> OnHealthChanged;
    private gameManager gm;

    private float maxHealth, currentHealth;
    public bool dead = false;
    private float deathDelay = 0f;
    public Transform center;
    public void setStats(CombatStats stats)
    {
        //health
        maxHealth = stats.health;
        currentHealth = maxHealth;
    }
    public void setStats(BuildingStats stats)
    {
        //health
        maxHealth = stats.health;
        currentHealth = maxHealth;
    }
    // Start is called before the first frame update
    void Awake()
    {
    }

    private void Start()
    {
        gm = gameManager.current;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (OnHealthChanged != null)
        {
            OnHealthChanged(maxHealth, currentHealth);
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        dead = true;
        //call ondeath functions
        EventManager.current.objectDestroyed(gameObject);
        
        Destroy(gameObject, deathDelay);
    }
    public float getMaxHealth()
    {
        return maxHealth;
    }
    public float getCurrentHealth()
    {
        return currentHealth;
    }
    public void addHealth(float amt)
    {
        currentHealth += amt;
        maxHealth += amt;
    }
    public void decreaseHealth(float amt)
    {
        currentHealth = Mathf.Clamp(currentHealth-amt,1,maxHealth);
        maxHealth -= amt;
    }

    public void healToFull()
    {
        currentHealth = maxHealth;
        if (OnHealthChanged != null)
        {
            OnHealthChanged(maxHealth, currentHealth);
        }
    }
}
