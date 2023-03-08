using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RollDice : MonoBehaviour
{
    Rigidbody rb;
    Transform parent;
    Vector3 force;
    Vector3 StartRollPoint;
    int lifeTime;
    float rollRate;
    [SerializeField] Transform[] diceSide;
    [SerializeField] Gameboard gameboard;
    bool isRolling;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        parent = GetComponentInParent<Transform>();
        //diceSide = new Transform[6];
        force = Vector3.zero;
        //StartRollPoint = new Vector3(0, 10, 0);
    }

    void onDead_RD(Actor a)
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        Player player = GameManager.Instance.GetPlayer();
        player.onDie += onDead_RD;
        rb = GetComponent<Rigidbody>();
        transform.position = StartRollPoint;
        rb.useGravity = false;
        isRolling = false;
        //InvokeRepeating("Roll", 1.0f, rollRate);
    }

    private void FixedUpdate()
    {
        if (!isRolling) return;
        if (rb.velocity.magnitude <= 0.1f)
            rb.velocity = Vector3.zero;
        if (rb.velocity.magnitude == 0.0f) {
            isRolling = false;
            int num = GetDiceNumber();
            gameboard.GetDiceNum(num);
            Debug.Log("Rolled " + num);
        }       
    }

    public void Roll()
    {
        if (isRolling) return;
        rb.useGravity = true;
        StartCoroutine(StartRollDelay());
        transform.position = StartRollPoint;
        
        rb.AddTorque(Random.Range(100, 200), Random.Range(0, 200), Random.Range(100, 200));
        
        //force = new Vector3(Random.Range(0, 5), 0, Random.Range(0, 5));
        //rb.AddForce(force, ForceMode.VelocityChange);
        //rb.angularVelocity = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
    }

    IEnumerator StartRollDelay()
    {
        yield return new WaitForFixedUpdate();
        isRolling = true;
    }

    public int GetDiceNumber()
    {
        if (!isRolling)
        {
            int diceNum = 0;
            float pos_y = 0;
            for(int i = 0; i < diceSide.Length; i++)
            {
                if(i == 0)
                {
                    pos_y = diceSide[i].position.y;
                    diceNum = i + 1;
                    continue;
                }
                if(pos_y < diceSide[i].position.y)
                {
                    pos_y = diceSide[i].position.y;
                    diceNum = i + 1;
                }
            }
            return diceNum;
        }return 0;
    }

    public void SetRB(Rigidbody rb)
    {
        this.rb = rb;
    }

    public void SetDiceLifeTime(int lifeTime)
    {
        this.lifeTime = lifeTime;
    }

    public void SetRollRate(float rate)
    {
        CancelInvoke("Roll");
        rollRate = rate;
        InvokeRepeating("Roll", 1.0f, rollRate);
    }

    public void SetDiceStartRollPos(Vector3 pos)
    {
        StartRollPoint = pos;
    }

    public void setGameBoard(Gameboard board)
    {
        gameboard = board;
    }

    public bool IsDiceRollling()
    {
        return isRolling;
    }

    void ResetDice()
    {
        if(lifeTime > 0)
        {
            lifeTime--;
            if(lifeTime <= 0)
            {
                lifeTime = 0;
                gameboard.ResetBoard();
                gameObject.SetActive(false);
                return;
            }
        }
        transform.eulerAngles = Vector3.zero;
        rb.useGravity = false;
        isRolling = false;
        transform.position = StartRollPoint;
    }

    private void OnDisable()
    {
        ResetDice();
    }


}


