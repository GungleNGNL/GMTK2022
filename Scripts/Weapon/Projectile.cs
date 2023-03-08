using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    protected float speed = 10f;
    [SerializeField]
    protected float damage = 1f;

    [Header("Collision Check")]
    [SerializeField]
    protected LayerMask sweepLayer;
    [SerializeField]
    protected float radius;
    [SerializeField]
    protected float minDistance;

    [Header("Effect")]
    [SerializeField]
    protected GameObject onHitEffect;

    protected Actor m_Owner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private GameObject SweepObjects(float distance)
    {
        RaycastHit[] hit = new RaycastHit[12];
        var count = Physics.SphereCastNonAlloc(transform.position, radius, transform.forward, hit, distance, sweepLayer, QueryTriggerInteraction.UseGlobal);
        if (count > 0)
            return hit[0].collider.gameObject;
        else
            return null;
    }

    private void Move()
    {
        float distance = speed * Time.deltaTime;

        var obj = SweepObjects(minDistance);
        if (obj == null)
            transform.position += transform.forward * distance;
        else
        {
            var actor = obj.GetComponent<Actor>();
            if (actor != null && m_Owner != null)
            {
                actor.TakeDamage(m_Owner, damage);
            }

            if (onHitEffect != null)
                Instantiate(onHitEffect, transform.position, Quaternion.identity);

            /*if (replaceOnHit && replaceObject != null)
                Instantiate(replaceObject, transform.position, Quaternion.LookRotation(-transform.forward));*/

            Destroy(this.gameObject);
        }
    }

    public void SetOwner(Actor owner) => m_Owner = owner;
}
