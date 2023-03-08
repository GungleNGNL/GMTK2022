using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : Singleton<HpUI>
{
    [SerializeField] int _hp = 5;
    int hp
    {
        get
        {
            return _hp;
        }
        set
        {
            if (value > maxHp || value < 0) return;
            if (value > _hp)
            {
                _hp = value;
                AddHp();
            }
            else
            {
                _hp = value;
                DeHp();
            }
        }
    }
    [SerializeField] int maxHp = 5;
    [SerializeField] Image[] holders;
    [SerializeField] Animation[] anims;
    RectTransform[] OrgPos;
    [SerializeField] Sprite heart, space;

    

    void OnPlayerHpChange(float currentHP, float maxHp, bool isDamage)
    {
        int curHp = (int)currentHP;
        SetUIHp(curHp);
    }

    private void Start()
    {
        var player = GameManager.Instance.GetPlayer();
        player.onHpChanged += OnPlayerHpChange;
        AddHp();
        StopAllAnimation();
    }
    void AddHp()
    {
        //hp++;
        for (int i = 0; i < hp; i++)
        {
            holders[i].sprite = heart;
            holders[i].color = Color.red;
        }
        if (hp == maxHp)
        {
            StopAllAnimation();
        }
        else
        {
            StartAnimation();
            StopAnimation();
        }      
    }

    void DeHp()
    {
        //hp--;
        for (int i = hp; i < maxHp; i++)
        {
            holders[hp].sprite = space;
            holders[hp].color = Color.white;
        }
        if (hp == maxHp - 1)
        {
            StartAnimation();
            return;
        }
        StopAnimation();
    }

    public void SetUIHp(int hp)
    {
        this.hp = hp;
    }

    void StartAnimation()
    {
        for(int i = 0; i < hp; i++)
        {
            if (i >= (maxHp - 1)) continue;
            anims[i].Play();
        }
    }

    void StopAnimation()
    {
        for (int i = hp; i < maxHp; i++)
        {
            if (i == (maxHp - 1)) return;
            anims[i].Stop();
            var rt = anims[i].GetComponent<Transform>();
            rt.localPosition = Vector3.zero;
        }
    }

    void StopAllAnimation()
    {
        for (int i = 0; i < hp - 1; i++)
        {
            anims[i].Stop();
            var rt = anims[i].GetComponent<Transform>();
            rt.localPosition = Vector3.zero;
        }
    }
}
