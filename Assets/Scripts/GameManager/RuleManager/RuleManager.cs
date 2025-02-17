using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleManager : MonoBehaviour
{
    private GameObject PlayerSpawner;

    public GameObject EnemiesParent; // Enemy를 자식으로 가지고 있는 부모 오브젝트

    public GameObject PlayerCharacter;

    private int currentWave = 0;

    public List<WaveManager> Waves;
    public List<BoxCollider2D> Blockades;

    // 타겟 프레임레이트 : 임시로 일단 여기에 둠
    const int targetFrameRate = 60;

    private void Awake()
    {
        // 타겟 프레임레이트 설정 : 임시로 일단 여기에 둠
        Application.targetFrameRate = targetFrameRate;
    }

    // Use this for initialization
    IEnumerator Start ()
    {
        PlayerSpawner = GameObject.Find("PlayerSpawner");
        // 파일로 스크립트를 만들수 있도록 해야하는데 일단 여기서 쌩으로 처리한다.

        yield return new WaitForSeconds(0.5f);

        if(PlayerSpawner)
        {
            PlayerSpawner.SendMessage("SpawnPlayer");
        }

        if (Waves.Count > 0)
            Waves[0].SendMessage("WaveStart");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void SetPlayer(GameObject playerObject)
    {
        PlayerCharacter = playerObject;
    }

    void PlayerDead()
    {
        // 게임 오버를 처리한다.
        // 메인 화면으로 보낸다.
        Logger.DebugLog("Game Over");
    }
}
