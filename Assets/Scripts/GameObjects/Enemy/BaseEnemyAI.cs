using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적의 AI 기본 통제 시스템.
/// 플레이어의 InputManager과 대응된다.
/// </summary>
public class BaseEnemyAI : MonoBehaviour
{
    public EnemyAIType BaseEnemyAIType = EnemyAIType.Melee;
    public EnemyState EnemyState = EnemyState.None;

    public float AIIdleTimemin = 3f;
    public float AIIdleTimemax = 5f;

    private GameObject PlayerObject;

    public EnemyAction EnemyAction;
    public EnemyAnimation EnemyAnimator;
    public DirectionController DirectionController;

    [Header("AI Settings")]
    public int IdleProb = 1;
    public int ChaseProb = 1;
    public int DetouringProb = 1;
    public int AttackProb = 1;

    [Header("Attack AI Settings")]
    public int Attack1Prob = 1;
    public int Attack2Prob = 1;
    public int ExtraAttackProb = 1;
    public int NotExtraAttackProb = 1;

    // Use this for initialization
    void Start()
    {
        if (!EnemyAction)
            EnemyAction = gameObject.GetComponent<EnemyAction>();
        if (!EnemyAnimator)
            EnemyAnimator = gameObject.GetComponentInChildren<EnemyAnimation>();
        if (!DirectionController)
            DirectionController = gameObject.GetComponentInChildren<DirectionController>();
    }

    void Update()
    {
        if (!PlayerObject)
        {
            PlayerObject = GameObject.FindGameObjectWithTag("Player");
            if (!PlayerObject) // 플레이어 오브젝트가 아직 스폰되지 않았거나 기타 이유로 찾지 못하는 경우
                return;
        }

        if (EnemyState == EnemyState.Dead)
            return;

        if (EnemyState == EnemyState.None)
        {
            StartCoroutine(StartAICoroutine());
        }

        float directionx = transform.position.x - PlayerObject.transform.position.x;
        if(EnemyAnimator.IsDirectionChangeAvailable())
        {
            DirectionController.SetDirection(directionx > 0 ? global::Direction.Left : global::Direction.Right);
        }
    }
    // 기본 AI 행동패턴
    // 플레이어를 공격할 수 없는 경우
    // 근접 AI : 플레이어를 향해 움직인다 OR 멍때린다.
    // 원거리 AI : 플레이어와의 거리를 벌리는 방향으로 Y축만 맞춘다. OR 멍때린다.
    // 플레이어를 공격할 수 있는 경우
    // 근접 AI : 공격한다. OR 멍때린다 OR 뒤를 잡는다.
    // 원거리 AI : 공격한다. OR 멍때린다.

    IEnumerator StartAICoroutine()
    {
        // 일단 행동 시간을 잡는다. 행동당 3~5초.
        float RandomTime = Random.Range(AIIdleTimemin, AIIdleTimemax);

        switch (BaseEnemyAIType)
        {
            case EnemyAIType.Melee:
                {
                    yield return StartMeleeAI(RandomTime);
                }
                break;
            case EnemyAIType.Range:
                {
                    yield return StartRangeAI(RandomTime);
                }
                break;
            case EnemyAIType.Hybrid:
                {
                    // 적AI 분포를 보고 행동결정을 해야한다.
                    // 일단 임시코드
                    yield return StartMeleeAI(RandomTime);
                }
                break;
        }
    }

    IEnumerator StartMeleeAI(float idleTime)
    {
        RandomGenerator<EnemyState> ActionDecider = new RandomGenerator<EnemyState>();
        if (IsPlayerWithinAttackRange(EnemyAIType.Melee))
        {
            ActionDecider.Push(EnemyState.Idle, IdleProb);
            ActionDecider.Push(EnemyState.Attacking, AttackProb);
            ActionDecider.Push(EnemyState.Detouring, DetouringProb);

            EnemyState DecidedAction = ActionDecider.GetRandom();
            switch (DecidedAction)
            {
                case EnemyState.Idle:
                    {
                        yield return DoIdle(idleTime);
                    }
                    break;
                case EnemyState.Attacking:
                    {
                        yield return DoMeleeAttack(idleTime);
                    }
                    break;
                case EnemyState.Detouring:
                    {
                        yield return DoDetour();
                    }
                    break;
            }
        }
        else
        {
            ActionDecider.Push(EnemyState.Idle, IdleProb);
            ActionDecider.Push(EnemyState.Chasing, ChaseProb);

            EnemyState DecidedAction = ActionDecider.GetRandom();
            switch (DecidedAction)
            {
                case EnemyState.Idle:
                    {
                        yield return DoIdle(idleTime);
                    }
                    break;
                case EnemyState.Chasing:
                    {
                        yield return DoChasing(idleTime);
                    }
                    break;
            }
        }
    }

    IEnumerator StartRangeAI(float idleTime)
    {
        RandomGenerator<EnemyState> ActionDecider = new RandomGenerator<EnemyState>();
        if (IsPlayerWithinAttackRange(EnemyAIType.Range))
        {
            ActionDecider.Push(EnemyState.Idle, IdleProb);
            ActionDecider.Push(EnemyState.Attacking, AttackProb);

            EnemyState DecidedAction = ActionDecider.GetRandom();
            switch (DecidedAction)
            {
                case EnemyState.Idle:
                    {
                        yield return DoIdle(idleTime);
                    }
                    break;
                case EnemyState.Attacking:
                    {
                        yield return DoRangeAttack(idleTime);
                    }
                    break;
            }
        }
        else
        {
            ActionDecider.Push(EnemyState.Idle, IdleProb);
            ActionDecider.Push(EnemyState.Negate, ChaseProb);

            EnemyState DecidedAction = ActionDecider.GetRandom();
            switch (DecidedAction)
            {
                case EnemyState.Idle:
                    {
                        yield return DoIdle(idleTime);
                    }
                    break;
                case EnemyState.Negate:
                    {
                        yield return DoKiting(idleTime);
                    }
                    break;
            }
        }
        yield return null;
    }

    IEnumerator DoMeleeAttack(float idleTime)
    {
        EnemyState = EnemyState.Attacking;

        // 일단 임시로 1,2라고 했지만 뭔가 변수화가 필요함.
        RandomGenerator<int> AttackDecider = new RandomGenerator<int>(2);
        AttackDecider.Push(1, Attack1Prob);
        AttackDecider.Push(2, Attack2Prob);

        int AttackDecide = AttackDecider.GetRandom();
        if (AttackDecide == 1)
        {
            EnemyAnimator.OnMeleeAttack1();
            yield return new WaitForSeconds(EnemyAnimator.GetAnimationDuration("MeleeAttack1"));

            RandomGenerator<bool> RandomDecider = new RandomGenerator<bool>(2);
            RandomDecider.Push(true, ExtraAttackProb);
            RandomDecider.Push(false, NotExtraAttackProb);

            if (RandomDecider.GetRandom())
            {
                EnemyAnimator.OnMeleeAttack2();
                yield return new WaitForSeconds(EnemyAnimator.GetAnimationDuration("MeleeAttack2"));
            }
        }
        else
        {
            EnemyAnimator.OnMeleeAttack2();
            yield return new WaitForSeconds(EnemyAnimator.GetAnimationDuration("MeleeAttack2"));
        }
        EnemyState = EnemyState.Idle;
        yield return new WaitForSeconds(idleTime);
        EnemyState = EnemyState.None;
    }

    IEnumerator DoRangeAttack(float idleTime)
    {
        EnemyState = EnemyState.Attacking;

        RandomGenerator<int> AttackDecider = new RandomGenerator<int>(2);
        AttackDecider.Push(1, Attack1Prob);
        AttackDecider.Push(2, Attack2Prob);

        int AttackDecide = AttackDecider.GetRandom();
        if (AttackDecide == 1)
        {
            EnemyAnimator.OnRangeAttack1();
            yield return new WaitForSeconds(EnemyAnimator.GetAnimationDuration("RangeAttack1"));
        }
        else
        {
            EnemyAnimator.OnRangeAttack2();
            yield return new WaitForSeconds(EnemyAnimator.GetAnimationDuration("RangeAttack2"));
        }
        EnemyState = EnemyState.Idle;
        yield return new WaitForSeconds(idleTime);
        EnemyState = EnemyState.None;
    }

    IEnumerator DoDetour()
    {
        EnemyState = EnemyState.Detouring;

        int decideUpDown = Random.Range(0, 2);

        Vector2 Direction = new Vector2(0, decideUpDown > 0 ? 1 : -1);

        float randomy = Random.Range(1f, 1.5f);

        yield return Move(Direction, randomy);

        float distancex = PlayerObject.transform.position.x - transform.position.x;
        Direction.y = 0;
        Direction.x = distancex > 0 ? 1 : -1;
        yield return Move(Direction, Mathf.Abs(distancex * 2));

        float distancey = PlayerObject.transform.position.y - transform.position.y;
        Direction.x = 0;
        Direction.y = distancey > 0 ? 1 : -1;

        yield return Move(Direction, Mathf.Abs(distancey));
        EnemyState = EnemyState.None;
    }

    IEnumerator DoIdle(float idleTime)
    {
        EnemyState = EnemyState.Idle;
        yield return new WaitForSeconds(idleTime);
        EnemyState = EnemyState.None;
    }

    // 아직 미구현
    IEnumerator DoKiting(float idleTime)
    {
        EnemyState = EnemyState.Negate;

        float directionx = transform.position.x - PlayerObject.transform.position.x;

        Vector2 Direction = new Vector2(directionx > 0 ? 1 : -1, 0);
        float randomx = Random.Range(2f, 2.5f);
        yield return Move(Direction, randomx);
        EnemyState = EnemyState.None;
    }

    IEnumerator DoChasing(float idleTime)
    {
        EnemyState = EnemyState.Chasing;

        while (!IsPlayerWithinAttackRange(EnemyAIType.Melee))
        {
            float xDistance = transform.position.x - PlayerObject.transform.position.x;
            float yDistance = transform.position.y - PlayerObject.transform.position.y;

            int x = 0;
            int y = 0;
            if (Mathf.Abs(xDistance) > 2)
            {
                x = xDistance > 0 ? -1 : 1;
            }

            if (Mathf.Abs(yDistance) > 0.3)
            {
                y = yDistance > 0 ? -1 : 1;
            }
            EnemyAnimator.SetWalk(true);
            EnemyAction.Move(new Vector2(x, y));
            yield return null;
        }
        EnemyAnimator.SetWalk(false);
        EnemyState = EnemyState.None;
    }

    IEnumerator Move(Vector2 direction, float distance)
    {
        EnemyAnimator.SetWalk(true);
        while (distance > 0)
        {
            EnemyAnimator.SetWalk(true);
            distance -= EnemyAction.Move(direction);
            yield return null;
        }
        EnemyAnimator.SetWalk(false);
    }

    public void Hit()
    {
        if(EnemyState != EnemyState.Down)
        {
            StopAllCoroutines();
            EnemyState = EnemyState.Hit;
            EnemyAnimator.SetWalk(false);
            StartCoroutine(DoHit());
        }
    }

    IEnumerator DoHit()
    {
        EnemyAnimator.OnHit();
        yield return new WaitForSeconds(EnemyAnimator.GetAnimationDuration("Hit"));
        EnemyState = EnemyState.None;
    }

    public void Airborne()
    {
        StopAllCoroutines();
        EnemyState = EnemyState.Down;
        EnemyAnimator.SetWalk(false);
        StartCoroutine(DoAirborne());
    }

    IEnumerator DoAirborne()
    {
        yield return null;
        while (!EnemyAnimator.IsMoveAvailable())
        {
            yield return null;
        }

        // 죽은 상태에서는 컨트롤을 더이상 하지 않는다.
        EnemyState = EnemyAction.IsDead ? EnemyState.Dead : EnemyState.None;
    }

    // 플레이어가 범위 안에 있는지 판단하는 함수.
    // 행동형이 정해진 다음이므로 정해진 행동형 AI를 파라미터로 받는다.
    bool IsPlayerWithinAttackRange(EnemyAIType AIType)
    {
        Vector2 curPos = transform.position;
        Vector2 playerPos = PlayerObject.transform.position;

        if (AIType == EnemyAIType.Melee)
        {
            // 일단 임시로 정하는 스펙
            // Y 차이가 0.3 이내이고 플레이어와의 거리가 1 이내이다.
            return Mathf.Abs(curPos.y - playerPos.y) < 0.3f && 1 < (curPos - playerPos).magnitude && (curPos - playerPos).magnitude < 2;
        }
        else
        {
            // 일단 임시로 정하는 스펙 2
            // Y 차이가 0.3 이내이다.
            return Mathf.Abs(curPos.y - playerPos.y) < 0.3f;
        }
    }
}

public enum EnemyAIType
{
    Melee,  //근거리 공격만을 수행하는 AI. 주로 적에게 접근하여 공격하는 기본형태의 AI와 추가로 적(플레이어의) 뒤를 잡는 행동을 취하려고 한다.
    Range,  //원거리 공격만을 수행하는 AI. 주로 적과의 Y축을 맞추되 적에게서 멀어지려고 하는 AI를 가진다.
    Hybrid  //둘다 수행할 수 있는 AI. 다른 적군들의 AI 분포를 보고 적은 쪽의 AI 역할을 수행하려고 한다. 단 수가 동일할 경우 랜덤으로 자신의 역할을 정한다.
}

public enum EnemyState
{
    None,       // 아무 행동 AI가 정해지지 않은 상태.
    Idle,       // 멍때리기.
    Chasing,    // 플레이어 추적하기.
    Attacking,  // 플레이어 공격하기.
    Detouring,  // 플레이어 우회하기 (뒤잡기).
    Negate,     // 플레이어와 멀어지기 (원거리 AI용).
    Hit,        // 피격 상태.
    Down,       // 다운 중.
    Dead,       // 죽음(죽음 애니메이션중)
}