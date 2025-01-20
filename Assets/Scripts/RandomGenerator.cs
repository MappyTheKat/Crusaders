using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator<T>
{
    private List<RandomProbPair<T>> listRandom;
    private int Max = 0;

    public RandomGenerator()
    {
        listRandom = new List<RandomProbPair<T>>();
    }

    public RandomGenerator(int size)
    {
        listRandom = new List<RandomProbPair<T>>(size);
    }

    public void Push(RandomProbPair<T> value)
    {
        Max += value.Prob;
        listRandom.Add(value);
    }

    public void Push(T t, int prob)
    {
        RandomProbPair<T> pair = new RandomProbPair<T>(t, prob);
        Max += prob;
        listRandom.Add(pair);
    }

    /// <summary>
    /// 복원추출.
    /// </summary>
    /// <returns></returns>
    public T GetRandom()
    {
        if (listRandom.Count == 0)
        {
            throw new RandomEmptyException();
        }
        int RandomValue = Random.Range(0, Max) + 1; // 뚜껑이 exclusive 라서 +1을 해줌

        T output = default(T);

        int count = listRandom.Count;
        for (int idx = 0; idx < count; idx++)
        {
            if (listRandom[idx].Prob == 0)
                continue;
            if ((RandomValue -= listRandom[idx].Prob) <= 0)
            {
                output = listRandom[idx].Output;
                break;
            }
        }
        return output;
    }

    /// <summary>
    /// 비복원추출.
    /// </summary>
    /// <returns></returns>
    public T GetRandomEx()
    {
        if (listRandom.Count == 0)
        {
            throw new RandomEmptyException();
        }
        int RandomValue = Random.Range(0, Max) + 1; // 뚜껑이 exclusive 라서 +1을 해줌

        T output = default(T);

        int count = listRandom.Count;
        for (int idx = 0; idx < count; idx++)
        {
            if (listRandom[idx].Prob == 0)
                continue;
            if ((RandomValue -= listRandom[idx].Prob) <= 0)
            {
                output = listRandom[idx].Output;
                Max -= listRandom[idx].Prob;
                listRandom.RemoveAt(idx);
                break;
            }
        }
        return output;
    }

    public bool IsRandomListEmpty()
    {
        return listRandom.Count == 0;
    }

    public void Clear()
    {
        listRandom.Clear();
        Max = 0;
    }
}

public struct RandomProbPair<T>
{
    public T Output;
    public int Prob;

    public RandomProbPair(T output, int prob)
    {
        Output = output;
        Prob = prob;
    }

    public RandomProbPair(T output)
    {
        Output = output;
        Prob = 1;
    }
}

public class RandomEmptyException : System.Exception
{
    public override string Message
    {
        get
        {
            return "Random list is empty";
        }
    }

    public RandomEmptyException()
    {

    }
}
