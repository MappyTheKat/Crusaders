using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> PlayerCharacters;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnPlayer()
    {
        if (PlayerCharacters.Count > 0)
        {
            int index = (int)(GameCommon.Instance.SelectedCharacter);
            if (PlayerCharacters[index])
            {
                var playerObject = GameObject.Instantiate(PlayerCharacters[index], transform.position, Quaternion.identity);
                var ruleManager = GameObject.Find("RuleManager");
                if(ruleManager)
                {
                    ruleManager.SendMessage("SetPlayer", playerObject);
                }
                Camera.main.SendMessage("SetPlayer", playerObject);
            }
        }
    }
}
