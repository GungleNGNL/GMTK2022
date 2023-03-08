using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameboard : MonoBehaviour
{
    [SerializeField]int boardID;
    [SerializeField] Sprite batteryIcon, addDiceIcon, bombIcon, mobSpawnIcon, machineGunIcon, wallIcon;
    [SerializeField] GameObject batteryItem, enemyPawn, enemyQueen, wall, explosionIndicator, mGunItem; //prefab
    [SerializeField] Image IconSlot;

    //GameboardEvent gameEvent;
    
    private void Start()
    {
        DiceManager.Instance.SetDiceID(boardID, this);
    }

    private void OnEnable()
    {
        IconSlot.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        IconSlot.sprite = null;
        IconSlot.gameObject.SetActive(false);
    }

    public void GetDiceNum(int num)
    {
        SpawnEvent((GameboardEvent)num - 1);
    }

    void SpawnEvent(GameboardEvent gEvent)
    {
        switch (gEvent)
        {
            case GameboardEvent.battery:                                                           //can get but cant heal
                IconSlot.sprite = batteryIcon;
                GameObject b = Instantiate(batteryItem, RandomSpawning(0), Quaternion.identity);
                b.transform.position += new Vector3(0, 0.4f, 0);
                break;

            case GameboardEvent.addDice:
                BoardManager.Instance.AddBoard();
                IconSlot.sprite = addDiceIcon;
                DiceManager.Instance.AddActiveDice();
                break;

            case GameboardEvent.bomb:                                                                          //No time, changed to mob. Change back later
                //IconSlot.sprite = bombIcon;
                //Instantiate(explosionIndicator, RandomSpawning(0), Quaternion.identity);
                IconSlot.sprite = mobSpawnIcon;
                int random = Random.Range(0, 9);
                if (random < 10)
                {
                    Instantiate(enemyPawn, RandomSpawning(3), Quaternion.identity);
                }
                else
                
                    Instantiate(enemyQueen, RandomSpawning(3), Quaternion.identity);
                
                break;

            case GameboardEvent.mobSpawn:
                IconSlot.sprite = mobSpawnIcon;
                int random2 = Random.Range(0, 9);
                if (random2 < 10)
                {
                    Instantiate(enemyPawn, RandomSpawning(3), Quaternion.identity);
                    
                }
                else
                    Instantiate(enemyQueen, RandomSpawning(3), Quaternion.identity);

                break;

            case GameboardEvent.MachineGun:
                IconSlot.sprite = machineGunIcon;
                GameObject m = Instantiate(mGunItem, RandomSpawning(4), Quaternion.identity);
                m.transform.position += new Vector3(0, 0.25f, 0);
                break;

            case GameboardEvent.Wall:
                IconSlot.sprite = wallIcon;
                GameObject h = Instantiate(wall, RandomSpawning(1), Quaternion.identity);
                h.transform.position += new Vector3(0, 0.5f, 0);
                break;
        }
    }

    Vector3 RandomSpawning(int type)
    {
        int x = Random.Range(0, 19);
        int y = Random.Range(0, 19);
        /*do
        {
            x = Random.Range(0, 19);
            y = Random.Range(0, 19);
        } while (ChessBoard.Instance.itemMap[x, y] == ChessBoard.BoardItem.space);*/
        ChessBoard.Instance.itemMap[x, y] = (ChessBoard.BoardItem)type;
        Vector2Int pos = new Vector2Int(x, y);
        Vector3 wPos = new Vector3();
        //wPos = ChessBoard.Instance.GetWorldPosition(pos);
        wPos = new Vector3(x * 0.5f - 4.75f, 0, y * 0.5f - 4.75f);
        //wPos.y = 0;
        return wPos;
    }

    public void ResetBoard()
    {
        BoardManager.Instance.DecreaseBoard(boardID);
    }

    public void SetBoardId(int id)
    {
        boardID = id;
    }
}


enum GameboardEvent
{
    battery = 0,
    addDice = 1,
    bomb = 2,
    mobSpawn = 3,
    MachineGun= 4,
    Wall = 5,
}
