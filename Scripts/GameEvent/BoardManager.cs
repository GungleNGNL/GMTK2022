using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    [SerializeField]Gameboard[] gameboards;
    int currentBoardNum;

    private void Start()
    {
        currentBoardNum = 1;
    }

    public void AddBoard()
    {
        if (currentBoardNum >= 3) return;
        currentBoardNum++;
        gameboards[currentBoardNum - 1].gameObject.SetActive(true);
        gameboards[currentBoardNum - 1].SetBoardId(currentBoardNum - 1);
    }

    public void DecreaseBoard(int id)
    {
        if (currentBoardNum <= 1) return;
            currentBoardNum--;
        gameboards[id - 1].gameObject.SetActive(false);
    }
}
