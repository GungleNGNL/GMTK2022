using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Actor
{
    [SerializeField]
    private float maxShield;
    public float MaxShield => maxShield;
    [SerializeField]
    private float chargeShieldSpeed;
    [SerializeField]
    private float invincibleTime = 1f;
    [SerializeField]
    private GameObject invincibleEffect;
    [SerializeField]
    private float damageOnTrigger = 1f;
    [SerializeField]
    private GameObject onDamageEffect;

    public UnityAction<float> onSpChanged;

    [SerializeField] private float m_CurrentShield;
    private PlayerInput m_PlayerInput;
    private float m_LastInvincibleTime = 0f;
    private GameObject m_InvincibleInstance;

    protected override void Awake()
    {
        base.Awake();
        m_CurrentShield = maxShield;
    }

    void Start()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
        onDamage += OnDamage;
        onDie += OnDie;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDice();
        UpdateInvincible();

        if (Input.GetKeyDown(KeyCode.M))
        {
            var loc = ChessBoard.Instance.GetChessLocation(transform.position);
            Debug.Log(loc);
        }
            
    }

    public override void TakeDamage(Actor source, float damage)
    {
        if(m_CurrentShield > 0)
        {
            m_CurrentShield = Mathf.Clamp(m_CurrentShield - damage, 0, maxShield);
            onSpChanged?.Invoke(m_CurrentShield);
        }
        else
        {
            float v = Mathf.Clamp(damage, 1f, m_CurrentHp);
            Damage(v);
        }
    }

    public void RecoverSheild()
    {
        m_CurrentShield = maxShield;
        onSpChanged?.Invoke(m_CurrentShield);
    }

    private void UpdateDice()
    {
        if (m_PlayerInput.IsRollDiceTriggered)
        {
            DiceManager.Instance.Roll();
            m_PlayerInput.IsRollDiceTriggered = false;
        }    
    }

    private void UpdateInvincible()
    {
        if (isInvincible)
        {
            if (Time.time - m_LastInvincibleTime >= invincibleTime)
            {
                isInvincible = false;
                if (m_InvincibleInstance != null)
                    Destroy(m_InvincibleInstance);
            }    
        }
    }

    public void SetInvincible()
    {
        if (isInvincible)
            return;

        isInvincible = true;
        m_LastInvincibleTime = Time.time;

        if (invincibleEffect != null)
            m_InvincibleInstance = Instantiate(invincibleEffect, transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (isInvincible)
            return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null)
            return;

        enemy.TakeDamage(this, damageOnTrigger);
        SetInvincible();
    }

    private void OnDamage(Actor source, float damage)
    {
        Debug.Log($"Damage from {source.name}, Current HP: {m_CurrentHp}");

        if (onDamageEffect != null)
            Instantiate(onDamageEffect, transform);
    }

    private void OnDie(Actor source)
    {
        Debug.Log($"You are killed by {source.name}");
    }
}
