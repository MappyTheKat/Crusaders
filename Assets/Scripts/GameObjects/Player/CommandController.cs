using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour
{
    public readonly int QueueSize = 4;

    // 큐가 내부 조회가 안돼서 바꿔야할듯
    public Queue<InputKeyTimePair> InputQueue;
    // 항상 4개의 인풋까지만 저장함

    void Awake()
    {
        InputQueue = new Queue<InputKeyTimePair>(QueueSize);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int currentQueueSize = InputQueue.Count;
        //for (int i =0; i< currentQueueSize; i++ )
        //{
        //    InputQueue[i].
        //}
    }

    public void InputAttackButtonDown(KeyCode keyCode)
    {
        // 인풋 큐랑 매칭해서 맞는 커맨드를 실행한다.
    }

    public void InputArrowDown(KeyCode keyCode)
    {
        InputKeyTimePair pair = new InputKeyTimePair(keyCode);
        if(InputQueue.Count >= QueueSize)
        {
            InputQueue.Dequeue();
        }
        InputQueue.Enqueue(pair);
    }
}

// KeyDown된 인풋(방향키)를 저장하는 스트럭트
public struct InputKeyTimePair
{
    KeyCode KeyCode;
    int ValidFrames;

    public InputKeyTimePair(KeyCode keyCode)
    {
        this.KeyCode = keyCode;
        ValidFrames = 30; // 30 프레임이 지나면 자동으로 큐에서 빠져나간다.
    }
}