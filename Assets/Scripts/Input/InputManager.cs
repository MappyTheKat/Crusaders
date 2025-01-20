using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("KeySettings")]
    public KeyCode Left = KeyCode.LeftArrow;
    public KeyCode Right = KeyCode.RightArrow;
    public KeyCode Up = KeyCode.UpArrow;
    public KeyCode Down = KeyCode.DownArrow;
    public KeyCode MeleeAttack = KeyCode.Z;
    public KeyCode RangeAttack = KeyCode.X;
    public KeyCode Jump = KeyCode.C;
    public KeyCode Special = KeyCode.Space;

    public CommandController CommandController;
    public PlayerAction PlayerAction;
    public PlayerAnimation PlayerAnimator;
    public DirectionController DirectionController;

    void Awake()
    {
        // 미리 인스펙터에서 할당해야하지만 사고방지용으로
        if (!CommandController)
            CommandController = gameObject.GetComponent<CommandController>();
        if (!PlayerAction)
            PlayerAction = gameObject.GetComponent<PlayerAction>();
        if (!PlayerAnimator)
            PlayerAnimator = gameObject.GetComponentInChildren<PlayerAnimation>();
        if (!DirectionController)
            DirectionController = gameObject.GetComponentInChildren<DirectionController>();
    }

    void Start()
    {

    }

    private Vector2 Direction = new Vector2();
    // Update is called once per frame
    void Update()
    {
        if (!CommandController || !PlayerAction)
            return;

        float x = 0f;
        float y = 0f;

        bool isWalk = false;
        if (Input.GetKeyDown(Up))
        {
            CommandController.InputArrowDown(Up);
        }
        if (Input.GetKeyDown(Down))
        {
            CommandController.InputArrowDown(Down);
        }
        if (Input.GetKeyDown(Left))
        {
            CommandController.InputArrowDown(Left);
        }
        if (Input.GetKeyDown(Right))
        {
            CommandController.InputArrowDown(Right);
            
        }
        if (Input.GetKeyDown(MeleeAttack))
        {
            CommandController.InputAttackButtonDown(MeleeAttack);
            PlayerAnimator.OnZKeyPressed();
        }
        if (Input.GetKeyDown(RangeAttack))
        {
            CommandController.InputAttackButtonDown(RangeAttack);
            PlayerAnimator.OnXKeyPressed();
        }
        if (Input.GetKeyDown(Jump))
        {
            CommandController.InputAttackButtonDown(Jump);
            PlayerAnimator.OnCKeyPressed();
        }
        if (Input.GetKeyDown(Special))
        {
            CommandController.InputAttackButtonDown(Special);
            PlayerAnimator.OnSpaceKeyPressed();
        }

        // 움직일 수 있는 상황일 때만 움직여야 한다.
        // 움직임이 가능한 State : Idle, Walk, Jump, JumpAttack
        // State는 내부 Animator에 들어있으므로 Animator의 State를 알아와야 함.

        if (Input.GetKey(Left)) x = -1f;
        if (Input.GetKey(Right)) x = 1f;
        if (Input.GetKey(Up)) y = 1f;
        if (Input.GetKey(Down)) y = -1f;

        Direction.x = x;
        Direction.y = y;

        isWalk = Direction != Vector2.zero;

        if(isWalk && CanWalk())
        {
            PlayerAction.Move(Direction);
        }

        if(x != 0  && CanChangeDirection())
        {
            DirectionController.SetDirection(x > 0 ? global::Direction.Right : global::Direction.Left);
        }

        PlayerAnimator.SetWalk(isWalk);
    }

    private bool CanWalk()
    {
        if (PlayerAnimator)
        {
            return PlayerAnimator.IsMoveAvailable();
        }
        return false;
    }

    private bool CanChangeDirection()
    {
        if(PlayerAnimator)
        {
            return PlayerAnimator.IsDirectionChangeAvailable();
        }
        return false;
    }
}
