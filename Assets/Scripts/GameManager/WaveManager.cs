using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<EnemySpawner> EnemySpawners;

    public GameObject RuleManager;

    // Use this for initialization
    void Start()
    {
        if (!RuleManager)
            RuleManager = GameObject.Find("RuleManager");

        // 사고방지용인데 너무 부하가 많이 들어간다(안돌아가게 하는 방법도 없고)
        for (int i = transform.childCount; i-- > 0;)
        {
            var spawner = transform.GetChild(i).GetComponent<EnemySpawner>();
            if (spawner && !EnemySpawners.Contains(spawner))
            {
                EnemySpawners.Add(spawner);
            }
        }

        if (EnemySpawners.Count == 0)
            Logger.DebugLog(string.Format("WaveManager {0}'s Spawner list is empty", this.name));
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = EnemySpawners.Count; i-- > 0;)
        {
            // 스폰이 다 끝나서 삭제된 스포너는 삭제한다.
            if (!EnemySpawners[i])
            {
                EnemySpawners.RemoveAt(i);
            }
        }
    }

    void DestroyAll()
    {
        for(int i = EnemySpawners.Count; i-->0;)
        {
            Destroy(EnemySpawners[i]);
            EnemySpawners.RemoveAt(i);
        }
    }

    IEnumerator WaveStart()
    {
        for (int i = EnemySpawners.Count; i-- > 0;)
        {
            EnemySpawners[i].SendMessage("WaveStart");
        }

        while (EnemySpawners.Count > 0)
        {
            yield return null;
        }

        RuleManager.SendMessage(string.Format("{0}Ended", name));
        Logger.DebugLog(string.Format("WaveEnd : {0}", this.name));
    }
}
