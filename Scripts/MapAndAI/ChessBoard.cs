using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : Singleton<ChessBoard>
{
    [SerializeField]int boardHeight;
    [SerializeField]int boardWidth;
    [SerializeField]
    private LayerMask checkLayer;

    public enum BoardItem
    {
        space = 0,
        wall = 1,
        item = 2,
        enemy = 3,
        player = 4
    }

    public BoardItem[,] itemMap;

    private void Awake()
    {
        itemMap = new BoardItem[boardWidth, boardHeight];
    }

    private void Start()
    {
        AIPathFinding.Instance.GenerateAStarGrid(boardHeight, boardWidth);
    }

    private void Update()
    {
        /*for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                var pos = new Vector3(i * 0.5f - 4.75f, -1f, j * 0.5f - 4.75f);
                if (Physics.Raycast(pos, Vector3.up, out RaycastHit hitInfo, 100f, checkLayer, QueryTriggerInteraction.UseGlobal))
                {
                    var obj = hitInfo.collider.gameObject;
                    if (obj == null)
                        continue;

                    if (obj.layer == 6)
                        itemMap[i, j] = BoardItem.player;
                    else if (obj.layer == 7)
                        itemMap[i, j] = BoardItem.wall;
                    else if (obj.layer == 9)
                        itemMap[i, j] = BoardItem.enemy;
                }
                else
                    itemMap[i, j] = BoardItem.space;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (itemMap[i, j] == BoardItem.enemy)
                    {
                        Debug.Log($"x:{i}, y:{j} is enemy");
                    }
                    else if (itemMap[i, j] == BoardItem.player)
                    {
                        Debug.Log($"x:{i}, y:{j} is player");
                    }
                }
            }
        }*/
    }

    public Vector2Int GetChessLocation(Vector3 worldPosition)
    {
        return new Vector2Int((int)((worldPosition.x + 4.75f) * 2), (int)((worldPosition.z + 4.75f) * 2));
    }

    public Vector3 GetWorldPosition(Vector2Int chessPosition)
    {
        return new Vector3(chessPosition.x * 0.5f - 4.75f, 0.25f, chessPosition.y * 0.5f - 4.75f); 
    }

    public List<Node> GetOneToEightNeighbour(Vector2 current)
    {
        int curX = (int)current.x;
        int curY = (int)current.y;
        //Debug.Log("get " + curY + " " + curX + "neighbour");
        var neighbourList = new List<Node>();

        for (int y = -1; y < 2; y++) // around 8 slot
        {
            for (int x = -1; x < 2; x++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                var posX = curX + x;
                var posY = curY + y;

                if (posY < boardHeight && posX < boardWidth && posY >= 0 && posX >= 0)
                {
                    //Debug.Log("Check Nei " + posY + " " + posX);
                    if (itemMap[posX, posY] != BoardItem.wall && itemMap[posX, posY] != BoardItem.enemy)
                    {
                        neighbourList.Add(AIPathFinding.Instance.GetGridNode(posX, posY));
                    }
                }
            }
        }

        return neighbourList;
    }

    public List<Node> GetInfinityToEightNeighbour(Vector2 current)
    {
        int curX = (int)current.x;
        int curY = (int)current.y;
        var neighbourList = new List<Node>();

        for (int x = 1; (curX + x) < boardWidth; x++) // />
        {
            int y = x;
            var posX = curX + x;
            var posY = curY + y;
            if (posX >= boardWidth) break;
            if (posY >= boardHeight) break;
            if (itemMap[posX, posY] != BoardItem.wall && itemMap[posX, posY] != BoardItem.enemy)
            {
                neighbourList.Add(AIPathFinding.Instance.GetGridNode(posX, posY));
            }
            else
                break;
        }

        for (int y = 1; (curY + y) < boardHeight; y++) // ^
        {
            var posX = curX;
            var posY = curY + y;
            if (posY >= boardHeight) break;
            if (itemMap[posX, posY] != BoardItem.wall && itemMap[posX, posY] != BoardItem.enemy)
            {
                neighbourList.Add(AIPathFinding.Instance.GetGridNode(posX, posY));
            }
            else
                break;
        }

        for (int x = -1; (curX + x) > 0; x--) // <\
        {
            int y = -x;          
            var posX = curX + x;
            var posY = curY + y;
            if (posX < 0) break;
            if (posY >= boardHeight) break;
            if (itemMap[posX, posY] != BoardItem.wall && itemMap[posX, posY] != BoardItem.enemy)
            {
                neighbourList.Add(AIPathFinding.Instance.GetGridNode(posX, posY));
            }
            else
                break;
        }

        for (int x = -1; (curX + x) > 0; x--)   // <-
        {           
            var posX = curX + x;
            var posY = curY;
            if (posX < 0) break;
            if (itemMap[posX, posY] != BoardItem.wall && itemMap[posX, posY] != BoardItem.enemy)
            {
                neighbourList.Add(AIPathFinding.Instance.GetGridNode(posX, posY));
            }
            else
                break;
        }

        for (int x = -1; (curX + x) > 0; x--) // </
        {
            int y = x;
            var posX = curX + x;
            var posY = curY + y;
            if (posY < 0) break;
            if (itemMap[posX, posY] != BoardItem.wall && itemMap[posX, posY] != BoardItem.enemy)
            {
                neighbourList.Add(AIPathFinding.Instance.GetGridNode(posX, posY));
            }
            else
                break;
        }

        for (int y = -1; (curY + y) > 0 ; y--) // V
        {
            var posX = curX;
            var posY = curY + y;
            if (posY < 0) break;
            if (itemMap[posX, posY] != BoardItem.wall && itemMap[posX, posY] != BoardItem.enemy)
            {
                neighbourList.Add(AIPathFinding.Instance.GetGridNode(posX, posY));
            }
        }

        for (int x = 1; (curX + x) < boardWidth; x++) // \>
        {
            int y = -x;
            var posX = curX + x;
            var posY = curY + y;
            if (posX >= boardWidth) break;
            if (posY < 0) break;
            if (itemMap[posX, posY] != BoardItem.wall && itemMap[posX, posY] != BoardItem.enemy)
            {
                neighbourList.Add(AIPathFinding.Instance.GetGridNode(posX, posY));
            }
            else
                break;
        }

        for (int x = 1; (curX + x) < boardWidth; x++)   // ->
        {
            var posX = curX + x;
            var posY = curY;
            if (posX >= boardWidth) break;
            if (itemMap[posX, posY] != BoardItem.wall && itemMap[posX, posY] != BoardItem.enemy)
            {
                neighbourList.Add(AIPathFinding.Instance.GetGridNode(posX, posY));
            }
            else
                break;
        }

        return neighbourList;
    }
}
