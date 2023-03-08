using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 Movement => m_InputControl.Player.Move.ReadValue<Vector2>();
    public bool IsMovementInput => Movement != Vector2.zero;
    public bool IsForwardPressed => Movement.y > 0f;
    public Vector2 MosePosition => m_InputControl.Player.MousePosition.ReadValue<Vector2>();

    [HideInInspector]
    public bool IsAttackTriggered = false;
    [HideInInspector]
    public bool IsPickupTriggered = false;
    [HideInInspector]
    public bool IsRollDiceTriggered = false;
    [HideInInspector]
    public bool IsMovementPerformed = false;

    private InputControl m_InputControl;

    private void Awake()
    {
        m_InputControl = new InputControl();
    }

    void onDead_PI(Actor a)
    {
        m_InputControl.Disable();
    }


    private void Start()
    {
        Player player = GameManager.Instance.GetPlayer();
        player.onDie += onDead_PI;
        RegisterCallback();
    }

    private void OnEnable()
    {
        m_InputControl.Player.Enable();
    }

    private void OnDisable()
    {
        m_InputControl.Player.Disable();
    }


    private void RegisterCallback()
    {
        m_InputControl.Player.Attack.started += context => IsAttackTriggered = true;
        m_InputControl.Player.Attack.canceled += context => IsAttackTriggered = false;

        m_InputControl.Player.Pickup.started += context => IsPickupTriggered = true;
        m_InputControl.Player.Pickup.canceled += context => IsPickupTriggered = false;

        m_InputControl.Player.RollDice.started += context => IsRollDiceTriggered = true;
        m_InputControl.Player.RollDice.canceled += context => IsRollDiceTriggered = false;

        m_InputControl.Player.Move.started += context => IsMovementPerformed = false;
        m_InputControl.Player.Move.performed += context => IsMovementPerformed = true;
        m_InputControl.Player.Move.canceled += context => IsMovementPerformed = false;
    }
}
