using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField]
    private GameObject spawnEffect;
    [SerializeField]
    private GameObject onDamageEffect;
    [SerializeField]
    private GameObject onDieEffect;
    [SerializeField]
    private float destroyDelay;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float movementDelay = 0.5f;
    [SerializeField]
    private float overlapDelay = 1.5f;
    [SerializeField]
    private AIMoveType moveType;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float attackInterval = 2f;

    [Header("Projectile Spawn Points")]
    [SerializeField]
    private Transform[] spawnPoints;

    private bool m_IsMoving = false;
    private Vector3 m_Destination;
    private float m_LastMovementTime = 0.0f;
    private Vector2Int m_PreviousPosition2D;
    private bool m_IsOverlapped = false;
    private float m_LastOverlappedTime = 0.0f;
    private float m_LastAttackTime = 0.0f;

    private void Start()
    {
        onDamage += OnDamage;
        onDie += OnDie;

        var pos2D = ChessBoard.Instance.GetChessLocation(transform.position);
        ChessBoard.Instance.itemMap[pos2D.x, pos2D.y] = ChessBoard.BoardItem.player;
        m_PreviousPosition2D = pos2D;

        m_LastAttackTime = Time.time;

        if (spawnEffect != null)
            Instantiate(spawnEffect, transform);
    }

    private void Update()
    {
        UpdatePath();
        UpdateMovement();
        UpdateAttack();
    }

    private void UpdatePath()
    {
        if (m_IsMoving || Time.time - m_LastMovementTime < movementDelay)
            return;

        var player = GameManager.Instance.GetPlayer();
        if (player == null)
            return;

        if (Vector3.Distance(transform.position, player.transform.position) <= 0.1f)
        {
            m_IsOverlapped = true;
            return;
        }
        else
        {
            if (m_IsOverlapped)
            {
                m_LastOverlappedTime = Time.time;
                m_IsOverlapped = false;
            }

            if (Time.time - m_LastOverlappedTime < overlapDelay)
                return;
        }

        var chessPosition = ChessBoard.Instance.GetChessLocation(new Vector3(transform.position.x,0,transform.position.z));
        var playerChessPosition = ChessBoard.Instance.GetChessLocation(player.transform.position);
        var paths = AIPathFinding.Instance.AStar(new Vector2(chessPosition.x, chessPosition.y), new Vector2(playerChessPosition.x, playerChessPosition.y), moveType);
        if (paths.Count <= 0)
            return;

        var path = paths.Pop();
        var destination2D = path;
        m_Destination = ChessBoard.Instance.GetWorldPosition(new Vector2Int((int)destination2D.x, (int)destination2D.y));

        ChessBoard.Instance.itemMap[(int)destination2D.x, (int)destination2D.y] = ChessBoard.BoardItem.enemy;
        ChessBoard.Instance.itemMap[m_PreviousPosition2D.x, m_PreviousPosition2D.y] = ChessBoard.BoardItem.space;
        m_PreviousPosition2D = new Vector2Int((int)destination2D.x, (int)destination2D.y);

        m_IsMoving = true;
    }

    private void UpdateMovement()
    {
        if (m_Destination.sqrMagnitude <= 0f || !m_IsMoving)
            return;

        if (Vector3.Distance(transform.position, m_Destination) <= 0.01f)
        {
            m_IsMoving = false;
            transform.position = m_Destination;
            m_LastMovementTime = Time.time;
            m_Destination = Vector3.zero;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, m_Destination, speed * Time.deltaTime);
    }

    private void UpdateAttack()
    {
        if (Time.time - m_LastAttackTime < attackInterval)
            return;

        if (projectile == null || spawnPoints.Length <= 0)
            return;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            var spawnPoint = spawnPoints[i];
            var obj = Instantiate(projectile, spawnPoint.transform.position, spawnPoint.rotation);
            if (obj == null)
                continue;

            var instance = obj.GetComponent<Projectile>();
            if (instance == null)
                continue;

            instance.SetOwner(this);
        }

        m_LastAttackTime = Time.time;
    }

    private void OnDamage(Actor source, float damage)
    {
        if (onDamageEffect != null)
            Instantiate(onDamageEffect, transform);
    }

    private void OnDie(Actor killer)
    {
        if (onDieEffect != null)
            Instantiate(onDieEffect, transform.position, Quaternion.identity);

        StartCoroutine(DelayedDestroy(destroyDelay));
    }

    private IEnumerator DelayedDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
