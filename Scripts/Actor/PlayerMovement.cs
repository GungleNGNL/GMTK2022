using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float rotationSpeed = 4f;
    [SerializeField]
    private float movementDelay = 0.5f;
    [SerializeField]
    private Vector3 moveDistance = new Vector3(0.5f, 0.0f, 0.5f);
    [SerializeField]
    private LayerMask blockLayer;

    private PlayerInput m_PlayerInput;
    private CharacterController m_CharacterController;
    private Vector3 m_MoveDirection = Vector3.zero;
    private bool m_IsMoving = false;
    private Vector3 m_Destination = Vector3.zero;
    private float m_LastMovementTime = 0.0f;
    private Vector2Int m_PreviousPosition2D;


    void onDead_PM(Actor a)
    {
        m_PlayerInput.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Player player = GameManager.Instance.GetPlayer();
        player.onDie += onDead_PM;
        m_PlayerInput = GetComponent<PlayerInput>();
        m_CharacterController = GetComponent<CharacterController>();

        var pos2D = ChessBoard.Instance.GetChessLocation(transform.position);
        ChessBoard.Instance.itemMap[pos2D.x, pos2D.y] = ChessBoard.BoardItem.player;
        m_PreviousPosition2D = pos2D;
    }

    // Update is called once per frame
    void Update()
    {
        BeforeCharacterUpdate();
        UpdatePosition();
        //UpdateRotation();
        AfterCharacterUpdate();
    }

    private void BeforeCharacterUpdate()
    {
        UpdateMovementDirection();
    }

    private void AfterCharacterUpdate()
    {
        //m_MoveDirection = Vector3.zero;
    }

    private void UpdateMovementDirection()
    {
        //m_MoveDirection += Vector3.forward * m_PlayerInput.Movement.y;
        //m_MoveDirection += Vector3.right * m_PlayerInput.Movement.x;
        if (m_PlayerInput.IsMovementPerformed && !m_IsMoving && Time.time - m_LastMovementTime >= movementDelay)
        {
            m_Destination = transform.position;

            if (m_PlayerInput.Movement.x != 0f && m_PlayerInput.Movement.y != 0f)
            {
                m_Destination.x += m_PlayerInput.Movement.x > 0f ? moveDistance.x : -moveDistance.x;
                m_Destination.z += m_PlayerInput.Movement.y > 0f ? moveDistance.z : -moveDistance.z;
            }
            else
            {
                m_Destination.x += (moveDistance.x * m_PlayerInput.Movement.x);
                m_Destination.z += (moveDistance.z * m_PlayerInput.Movement.y);
            }

            var checkPos = m_Destination;
            checkPos.y = -1f;

            if (Physics.Raycast(checkPos, Vector3.up, out RaycastHit hitInfo, 100f, blockLayer, QueryTriggerInteraction.UseGlobal))
            {
                m_Destination = Vector3.zero;
                return;
            }

            var pos2D = ChessBoard.Instance.GetChessLocation(m_Destination);
            ChessBoard.Instance.itemMap[pos2D.x, pos2D.y] = ChessBoard.BoardItem.player;
            ChessBoard.Instance.itemMap[m_PreviousPosition2D.x, m_PreviousPosition2D.y] = ChessBoard.BoardItem.space;
            m_PreviousPosition2D = pos2D;

            m_MoveDirection = (m_Destination - transform.position).normalized;

            m_IsMoving = true;
        }
    }

    private void UpdatePosition()
    {
        //m_CharacterController.Move(m_MoveDirection * speed * Time.deltaTime);
        if (m_IsMoving)
        {
            if (Vector3.Distance(transform.position, m_Destination) <= 0.01f)
            {
                m_IsMoving = false;
                transform.position = m_Destination;
                m_LastMovementTime = Time.time;
                m_MoveDirection = Vector3.zero;
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, m_Destination, speed * Time.deltaTime);
        }
    }

    private void UpdateRotation()
    {
        Vector3 targetForward = Vector3.RotateTowards(transform.forward, m_MoveDirection, rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(targetForward);
    }
}
