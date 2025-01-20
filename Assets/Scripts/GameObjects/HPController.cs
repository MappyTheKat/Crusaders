using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPController : MonoBehaviour
{
    public int MaxHP;

    public int CurrentHP;

    // 애니메이션 용. CurrentHP를 향해 매프레임마다 1씩 이동해서 HP 증감 애니메이션을 처리할때 사용한다.
    public int HpIndicator;

    public bool IsDead;

    public bool Invelnerable = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHP != HpIndicator)
        {
            HpIndicator += (CurrentHP > HpIndicator ? 1 : -1);
        }

        if (IsDead)
            return;

        if (Invelnerable)
            return;

        if (CurrentHP <= 0)
        {
            SendMessage("OnDeath");
            IsDead = true;
        }
    }

    public void AddHP(int hp)
    {
        if(Invelnerable) // 무적이어도 일단 힐은 받는다
        {
            if (hp < 0)
                return;
        }

        CurrentHP = Mathf.Min(CurrentHP + hp, MaxHP);
    }
}
