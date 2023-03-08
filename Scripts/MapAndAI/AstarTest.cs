using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarTest : MonoBehaviour
{
    [SerializeField] Vector2 start, end;
    [SerializeField] AIMoveType mode;
    Stack<Vector2> abc;
    private void Start()
    {
        Invoke("Test", 0.1f);
    }

    void Test()
    {

        Stack<Vector2> abc = new Stack<Vector2>();
        abc = AIPathFinding.Instance.AStar(start, end, mode);
        int s = 0;
        foreach(Vector2 i in abc)
        {
            //Vector2 path = abc.Pop();
            s++;
            Debug.Log("Step " + s + " go x:" + i.x + " y: " + i.y);
        }
     
    }
}
