using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터 액션(화면 좌표상 이동, 리지드바디 통제) 등을 처리하는 스크립트.
/// 기본 타입으로 적들이나 플레이어가 별도 처리할 것을 가정하여 별도의 상속체계로 분리한다.
/// </summary>
public class CharacterAction : MonoBehaviour
{
    public string CharacterName;

    public float XSpeed = 0.2f;
    public float YSpeed = 0.1f;

    Rigidbody2D attatchedRigidBody;

    public float XForce = 0f;

    public Animator CharacterAnimator;

    public GameObject ChildObject;

    [Tooltip("Airborne vector when death")]
    public Vector2 DeathVector;

    public bool IsDead = false;

    void Awake()
    {
        if (!attatchedRigidBody)
            attatchedRigidBody = GetComponent<Rigidbody2D>();
        if (!ChildObject)
        {
            if (transform.childCount > 0)
            {
                ChildObject = transform.GetChild(0).gameObject;
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(XForce !=0)
        {
            transform.Translate(new Vector2(XForce, 0));
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, -transform.position.y);
	}

    public float Move(Vector2 Direction)
    {
        Direction.x = Direction.x * XSpeed;
        Direction.y = Direction.y * YSpeed;
        transform.Translate(Direction);

        return Direction.magnitude;
    }

    /// <summary>
    /// 오브젝트 전체에 (X,Z) 힘을 가함. X 포스만 여기서 처리하고 Z Force는 내부의 GameObject에서 별도로 처리.
    /// </summary>
    /// <param name="Vector"></param>
    public void AddForce(Vector2 Vector)
    {
        XForce = Vector.x;

        // Z Force 처리
        ChildObject.SendMessage("AddZForce", Vector.y);
    }

    public void ResetXForece()
    {
        XForce = 0;
    }

    public void Hit(HitBox hitBox)
    {
        Logger.DebugLog(string.Format("Hit ! Inflictor : {0}", hitBox.Owner.name));
        ChildObject.SendMessage("OnHit");
    }

    public void OnDeath()
    {
        // 일단 죽을땐 전부 떠서 죽는다
        if(XForce == 0)
        {
            AddForce(new Vector2(DeathVector.x * ChildObject.transform.localScale.x, DeathVector.y));
        }
        ChildObject.SendMessage("OnAirborne");
        ChildObject.SendMessage("OnDeath");
    }

    public void Airborne(Vector2 airborneVector)
    {
        AddForce(airborneVector);
        ChildObject.SendMessage("OnAirborne");
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
