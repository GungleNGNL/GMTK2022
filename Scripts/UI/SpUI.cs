using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpUI : MonoBehaviour
{

    Player player;
    [SerializeField] int _sp = 5;
    int sp
    {
        get
        {
            return _sp;
        }
        set
        {
            if (value > maxSp || value < 0) return;
            if (value > _sp)
            {
                _sp = value;
                AddSp();
            }
            else
            {
                _sp = value;
                DeSp();
            }
        }
    }
    [SerializeField] int maxSp = 5;
    [SerializeField] Image[] holders;
    [SerializeField] Sprite shield, space;
    [SerializeField] Color sColor;



    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
        player.onSpChanged += OnPlayerSpChange;
        sp = (int)player.MaxShield;
        AddSp();
    }

    private void AddSp()
    {
        for (int i = 0; i < sp; i++)
        {
            holders[i].sprite = shield;
            holders[i].color = sColor;
        }
    }

    private void DeSp()
    {
        for (int i = sp; i < maxSp; i++)
        {
            holders[sp].sprite = space;
            holders[sp].color = Color.white;
        }
    }

    public void SetUISp(int sp)
    {
        this.sp = sp;
    }

    void OnPlayerSpChange(float currentSP)
    {
        int curSp = (int)currentSP;
        SetUISp(curSp);
    }
}
