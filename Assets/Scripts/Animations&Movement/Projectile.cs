using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ComponentAttack source;
    private ComponentHealth target;
    private Vector3 targetLastPos;
    private Vector3 direction;
    public float speed;

    private bool firstUpdate = true;
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!firstUpdate && (target == null || target.dead))
        {
            Destroy(gameObject);
        }*/
        firstUpdate = false;
        //find target position
        Vector3 curPos = transform.position;
        if (target != null)
        {
            targetLastPos = target.center.position;
            direction = (targetLastPos - curPos).normalized;
        }
        //go towards target position
        Vector3 newPos = curPos+direction * speed * Time.deltaTime;
        transform.position = newPos;
        //if within radius 1 of last target pos and target is null, destroy
        if (target == null)
        {
            Destroy(gameObject,2f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(target == null || other.gameObject == target.gameObject)
        {
            source.DealDamage();
            Destroy(gameObject);
        }
    }

    public void setup(ComponentAttack s,ComponentHealth t)
    {
        source = s;
        target = t;
        targetLastPos = target.center.position;
    }

}
