using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Singleton<Weapon>
{
    [SerializeField]
    protected float cooldown = 0.5f;

    protected Actor m_RootObject;
    protected PlayerInput m_PlayerInput;
    protected float m_LastAttackTime;
    
    protected virtual void Initialize() { }
    protected virtual void OnUpdate() { }
    protected abstract void OnAttack();

    protected virtual void Start()
    {
        m_RootObject = GetComponentInParent<Actor>();
        m_PlayerInput = GetComponentInParent<PlayerInput>();

        Initialize();
    }

    private void Update()
    {
        UpdateAttackState();
        OnUpdate();
    }

    private void UpdateAttackState()
    {
        if (m_PlayerInput == null)
            return;

        if (Time.time - m_LastAttackTime <= cooldown)
            return;

        if (m_PlayerInput.IsAttackTriggered)
        {
            OnAttack();//spawn
            m_LastAttackTime = Time.time;
            //m_PlayerInput.IsAttackTriggered = false;
        }        
    }
    
    public void upG()
    {
        cooldown = 0.15f;
        Invoke("normal", 10f);
    }

    void normal()
    {
        cooldown = 0.5f;
    }
}
