using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CollisionBox : MonoBehaviour
{
    public GameObject Owner;

    public GameObject RootObject;

    public HitBoxType HitBoxType;

    protected ZController parentZController;
    protected BoxCollider2D attatchedCollider;

    [Tooltip("게임상의 y축 두께")]
    public float Thickness = 0.2f;

    // 이 박스의 중심좌표
    public Vector3 WorldXYZ
    {
        get
        {
            return new Vector3(attatchedCollider.bounds.center.x, RootObject.transform.position.y, attatchedCollider.bounds.center.y - RootObject.transform.position.y);
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // CollisionBox2D가 겹쳤을때 Thickness와 Z축을 고려한 재계산 함수
    public bool IsHit(CollisionBox other)
    {
        Bounds ownBound = new Bounds(WorldXYZ, new Vector3(attatchedCollider.bounds.size.x, Thickness, attatchedCollider.bounds.size.y));
        Bounds otherBound = new Bounds(other.WorldXYZ, new Vector3(other.attatchedCollider.bounds.size.x, other.Thickness, other.attatchedCollider.bounds.size.y));

        return ownBound.Intersects(otherBound);
    }

    protected void SetOwner(GameObject gameObject)
    {
        Owner = gameObject;
    }
}
