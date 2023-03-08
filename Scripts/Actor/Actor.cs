using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    [SerializeField]
    protected float maxHp;
    [SerializeField]
    protected bool isInvincible;

    [SerializeField] protected float m_CurrentHp;
    protected bool m_IsAlive;

    public UnityAction<Actor, float> onDamage;
    public UnityAction<Actor> onDie;
    public UnityAction<float, float, bool> onHpChanged;

    public float GetMaxHp() => maxHp;
    public float GetCurrentHp() => m_CurrentHp;

    protected virtual void Awake()
    {
        m_CurrentHp = maxHp;
        m_IsAlive = true;
    }

    public virtual void TakeDamage(Actor source, float damage)
    {
        float v = Mathf.Clamp(damage, 1f, GetCurrentHp());
        Damage(v);
    }

    protected virtual void Damage(float damage)
    {
        if (isInvincible) return;
        float v = Mathf.Clamp(damage, 1f, m_CurrentHp);
        m_CurrentHp -= v;
        onDamage?.Invoke(this, v);
        onHpChanged?.Invoke(m_CurrentHp, maxHp, true);

        if (m_CurrentHp <= 0f)
        {
            m_IsAlive = false;
            onDie?.Invoke(this);
        }
    }
}
