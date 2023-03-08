using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    float currentTime = 0f;
    public float lifetime = 20f;
    public GameObject onSpawnEffect;
    public GameObject onDieEffect;
    // Start is called before the first frame update
    void Start()
    {
        var pos2D = ChessBoard.Instance.GetChessLocation(transform.position);
        ChessBoard.Instance.itemMap[pos2D.x, pos2D.y] = ChessBoard.BoardItem.wall;
        currentTime = 0;
        if(onSpawnEffect != null)
        Instantiate(onSpawnEffect, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= lifetime)
        {
            if (onDieEffect != null)
                Instantiate(onDieEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
