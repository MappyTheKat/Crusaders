using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZController : MonoBehaviour
{
    public static float Gravity = 0.01f;

    public float Z;

    #region For DependencyZControl (상위 오브젝트에 종속되어 따라다니는 물체들의 Z 컨트롤용)
    public bool IsDependentToParent = false;

    private float localZ;

    private ZController ParentZController;
    #endregion

    public bool IsKinetic = false;

    public bool Ground = true;

    public float JumpVelocity = 0.3f;

    public float ZVelocity = 0f;

    [Tooltip("Bounciness for Down : DownVelocity - bounciness")]
    public float bounciness = 0.2f;

    void Start()
    {
        if(IsDependentToParent)
        {
            // 그냥 GetComponentInParent를 호출하면 자기 자신이 리턴돼서 편법을 사용(..)
            ParentZController = transform.parent.GetComponentInParent<ZController>();
            if (!ParentZController)
                Logger.DebugLog(string.Format("Parent ZController of {0} not found", gameObject));
            localZ = Z;
        }
        
        if (IsKinetic)
        {
            Ground = false;
        }
        transform.localPosition = new Vector3(0, Z, transform.parent.position.y * (-0.1f));
    }

    void Update()
    {
        if(IsDependentToParent)
        {
            // DependentToParent일 경우 자신의 처음 세팅된 로컬 Z 좌표 + Parent의 ZController의 Z 좌표가 현재 자신의 Z 좌표가 된다.
            Z = ParentZController.Z + localZ;
            // transform.localPosition = new Vector3(0, localZ, 0);
        }
        else if (!IsKinetic)
        {
            if (Z > 0)
            {
                ZVelocity -= Gravity;
            }

            Z += ZVelocity;

            if (Z <= 0)
            {
                Z = 0;

                // 에어본 애니메이션을 분리해야할것 같다.
                // 뭔가 통통튀는 모양새가 되어버림
                var animator = GetComponent<Animator>();
                if (animator && animator.GetCurrentAnimatorStateInfo(0).IsName("Airborne"))
                {
                    var velocity = -(ZVelocity + bounciness);
                    if (velocity > 0)
                    {
                        ZVelocity = velocity;
                    }
                    else
                    {
                        Ground = true;
                        ZVelocity = 0;
                        transform.parent.SendMessage("ResetXForece");
                    }
                }
                else
                {
                    Ground = true;
                    ZVelocity = 0;
                    transform.parent.SendMessage("ResetXForece");
                }
            }
            else
            {
                Ground = false;
            }
            gameObject.SendMessage("SetGround", Ground);
            transform.localPosition = new Vector3(0, Z, 0);
        }
        else
        {
            Z += ZVelocity;
            transform.localPosition = new Vector3(0, Z, 0);
        }
    }

    public static void SetGravity(float value)
    {
        Gravity = value;
    }

    public void Jump()
    {
        AddZForce(JumpVelocity);
    }

    public void AddZForce(float value)
    {
        ZVelocity += value;
    }

    public void AddZ(float value)
    {
        Z += value;
    }
}
