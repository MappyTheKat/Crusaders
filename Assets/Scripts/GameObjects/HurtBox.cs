using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HurtBox : CollisionBox
{
    private List<HitBox> listContactHitbox;
    public int HitBoxListSize = 8;

    // 월드 위치에서의 Boundary를 재구성해야하므로...
    // X Y 위치를 얻어온다.
    // 이 박스의 Z 위치 정보를 얻어온다
    // 박스와(X,Z) Z 위치, 두께로 유사 3차원 박스를 재현한다.

    // 박스는 항상 정사각형이지만 컬라이더가 오프셋으로 이동하거나 중심이 왜곡되는 현상도 있으므로 중심을 다시 계산해야한다.
    // 어차피 Z 컨트롤러 때문에 재계산을 해야하므로 이쪽이 좀 더 편하다.
    // 월드상의 좌표 (가상 Z축을 포함하는)
    // (X = 컬라이더의 센터의 X, Y = 오너의 Y, Z = 센터의 Y - 오너의 Y)

    [Header("Parameters")]
    public int MeleeArmor;
    public int RangeArmor;
    public int AirborneResist;
    public float KnockbackResist;
    public int SmiteResist;
    public int HitRecovery;
    public int SuperArmor;

    private void Awake()
    {
        if (!RootObject)
            RootObject = transform.root.gameObject;
        listContactHitbox = new List<HitBox>(HitBoxListSize);
        if (!attatchedCollider)
        {
            attatchedCollider = GetComponent<BoxCollider2D>();
        }
        if (!parentZController)
        {
            if (transform.parent)
            {
                parentZController = transform.parent.GetComponent<ZController>();
            }
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = listContactHitbox.Count; i --> 0;)
        {
            if (!listContactHitbox[i])
            {
                listContactHitbox.RemoveAt(i);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        HitBox hitBoxObject = other.gameObject.GetComponent<HitBox>();
        if (hitBoxObject)
        {
            if (!listContactHitbox.Contains(hitBoxObject))
            {
                if (hitBoxObject.HitBoxType != this.HitBoxType)
                {
                    if (IsHit(hitBoxObject))
                    {
                        if (listContactHitbox.Count >= HitBoxListSize) // 히트박스는 8개 이상 저장하지 않는다.
                        {
                            listContactHitbox.RemoveAt(0);
                        }
                        listContactHitbox.Add(hitBoxObject);
                        // 허트박스 소유 오브젝트 히트처리
                        Hit(hitBoxObject);
                        // 히트박스 소유 오브젝트 히트처리
                        hitBoxObject.SendMessage("HitBack");
                    }
                }
            }
        }
    }

    // 정보를 받아와서 플레이어오브젝트에게 전할 정보를 넘겨받는다.
    // 데미지
    // 히트타입
    // 띄우는힘
    // 넉백수치
    // 강타수치
    // 경직도
    // 역경직도
    // 아머파괴도
    private void Hit(HitBox hitBox)
    {
        int dmg = 0;
        dmg += Mathf.Max(0, hitBox.MeleeDamage - MeleeArmor);
        dmg += Mathf.Max(0, hitBox.RangeDamage - RangeArmor);

        Vector2 AirborneVector = hitBox.AirborneVector;
        AirborneVector.y -= AirborneResist;
        if (AirborneVector.y > 0)
        {
            RootObject.SendMessage("Airborne", AirborneVector);
        }
        // 데미지 계산, 히트 박스와 허트박스의 상성체크는 여기서 하므로 Hit에서는 
        RootObject.SendMessage("AddHP", -dmg);
        RootObject.SendMessage("Hit", hitBox);
    }
}
