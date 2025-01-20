using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool IsRandom;
    [Tooltip("생성할 오브젝트가 떨어지면 자동으로 소멸하는지")]
    public bool IsInstant;
    [Tooltip("복원추출 여부")]
    public bool IsReserve;

    private bool WaveStarted = false;

    public List<GameObject> EnemyCharacters;

    private GameObject enemyObject;

    public Transform EnemiesRoot;
    // Use this for initialization
    void Start()
    {
        if (EnemiesRoot)
            EnemiesRoot = GameObject.Find("Enemies").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!WaveStarted)
            return;

        if(!enemyObject)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if(EnemyCharacters.Count>0)
        {
            int index = 0;
            // Random이 아닐 경우 0번부터 순차대로 스폰한다.
            if(IsRandom)
            {
                index = Random.Range(0, EnemyCharacters.Count);
            }

            if(EnemyCharacters[index])
            {
                var EnemyObject = GameObject.Instantiate(EnemyCharacters[index], transform.position, Quaternion.identity);
                enemyObject = EnemyObject;
            }

            if(!IsReserve)
            {
                EnemyCharacters.RemoveAt(index);
            }
        }
        else
        {
            if (IsInstant)
            {
                Destroy(gameObject);
            }
        }
    }

    public void WaveStart()
    {
        WaveStarted = true;
    }
}
