using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [Header("Projectile")]
    [SerializeField]
    protected GameObject projectile;
    [SerializeField]
    protected Transform tip;

    [Header("Aim")]
    [SerializeField]
    protected LayerMask groundLayer;
    [SerializeField]
    protected bool ignoreHeight;

    protected override void OnUpdate()
    {
        Vector3 position = GetMousePosition();
        if (position.sqrMagnitude <= 0f)
            return;

        Vector3 direction = position - transform.position;
        if (ignoreHeight)
            direction.y = 0f;

        transform.forward = direction;
    }

    protected override void OnAttack()
    {
        if (m_RootObject == null)
            return;

        var obj =  Instantiate(projectile, tip.position, transform.rotation);
        if (obj == null)
            return;

        var instance = obj.GetComponent<Projectile>();
        if (instance == null)
            return;

        instance.SetOwner(m_RootObject);
    }

    protected Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(m_PlayerInput.MosePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundLayer))
            return hitInfo.point;
        else
            return Vector3.zero;
    }
}
