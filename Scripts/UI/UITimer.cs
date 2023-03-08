using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITimer : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI timerText;
    int timer = 0;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        InvokeRepeating("Timer", 0, 1);
    }

    void Timer()
    {
        timer++;
        UpdateUITimer();
    }
    int mins = 0;
    int secs = 0;
    string minText = "";
    string secText = "";
    void UpdateUITimer()
    {
        mins = timer / 60;
        secs = timer % 60;
        if(mins < 10)
        {
            minText = 0 + "" + mins;
        }
        else
        {
            minText = "" + mins;
        }
        if (secs < 10)
        {
            secText = 0 + "" + secs;
        }
        else
        {
            secText = "" + secs;
        }
        timerText.text = minText + ":" + secText;
    }
}
