using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HitBox : CollisionBox
{
    public HitBoxRangeType HitBoxRangeType;

    [Header("Parameters")]
    public Vector2 AirborneVector;
    public int KnockBack;
    public int Smite;
    //경직
    public int Stiffness;
    //역경직
    public int RStiffness;
    public int MeleeDamage;
    public int RangeDamage;


    // Use this for initialization

    private void Awake()
    {
        if (!RootObject)
            RootObject = transform.root.gameObject;
        if (!attatchedCollider)
        {
            attatchedCollider = GetComponent<BoxCollider2D>();
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void HitBack()
    {
        // 히트백 처리.
        // 동작 애니메이션을 지연시킨다.
        switch (HitBoxRangeType)
        {
            case HitBoxRangeType.Laser:
                {
                    if (Owner)
                    {


                    }
                }break;
            case HitBoxRangeType.Bullet:
                {
                    // 히트백이 없고 맞으면 바로 오브젝트가 사라진다.
                    Destroy(RootObject);
                }break;
        }
    }
}

// 히트박스가 누구편인지
public enum HitBoxType
{
    Player,
    Enemy,
    Neutral
}

public enum HitBoxRangeType
{
    Bullet,
    Laser
}
