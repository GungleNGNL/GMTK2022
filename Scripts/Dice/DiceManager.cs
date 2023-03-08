using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : Singleton<DiceManager>
{
    [SerializeField] int maxDicesNum;
    [SerializeField] RollDice[] dices;
    [SerializeField] GameObject[] diceObj;
    [SerializeField] int _activeDicesNum;
    int activeDicesNum {
        get
        {
            return _activeDicesNum;
        }
        set
        {
            if(value > maxDicesNum)
            {
                return;
            }
            else
            {
                _activeDicesNum = value;
            }
        }
    }
    [SerializeField] Transform[] diceSpawnPoint;

    [Header("DiceEvent")]
    [Range(3, 12)]
    [SerializeField] int diceCurrentLifeTime; // 1 = roll once
    [Range(4, 12)]
    [SerializeField] float diceCurrentRollRate;

    private void Awake()
    {
        if(diceCurrentLifeTime <= 0)
        {
            diceCurrentLifeTime = 3;
        }
        if(diceCurrentRollRate < 4)
        {
            diceCurrentRollRate = 8;
        }
        activeDicesNum = 1;
        for(int i = 0; i < maxDicesNum; i++)
        {
            SpawnDice(i);
        }
    }

    private void Start()
    {
        for (int i = 0; i < activeDicesNum; i++)
        {
            SetDice(i);
        }
    }

    void SpawnDice(int id)
    {
        GameObject dice = Instantiate(diceObj[id], diceSpawnPoint[id].position, Quaternion.identity);
        dices[id] = dice.GetComponent<RollDice>();
        dices[id].SetDiceStartRollPos(diceSpawnPoint[id].position);
        dices[id].gameObject.SetActive(false);
    }

    void SetDice(int id) //set and active dice
    {
        //get and set current CD/Roll speed
        //Rigidbody rb = dices[id].GetComponent<Rigidbody>();
        dices[id].SetRollRate(diceCurrentRollRate);
        //dices[id].SetRB(rb);
        if (id == 0)
        {
            dices[id].SetDiceLifeTime(0);
        }
        else
        {
            //get and set current life time
            dices[id].SetDiceLifeTime(diceCurrentLifeTime);
        }
        dices[id].gameObject.SetActive(true);
    }

    public void AddActiveDice()
    {
        if (activeDicesNum + 1 > maxDicesNum)
        {
            Debug.LogWarning("Tried to add dice than maximun dice number");
            return;
        }
        activeDicesNum++;
        SetDice(activeDicesNum - 1);
    }

    public void Roll()
    {
        if (dices.Length <= 0)
            return;

        var dice = dices[0];
        if (dice == null)
            return;

        dice.Roll();
    }

    public void SetDiceID(int id, Gameboard board)
    {
        dices[id].setGameBoard(board);
    }
}
