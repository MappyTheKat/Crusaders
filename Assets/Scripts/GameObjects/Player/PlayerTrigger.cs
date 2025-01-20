using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public GameObject RuleManager;
	// Use this for initialization
	void Start ()
    {
        if (!RuleManager)
            RuleManager = GameObject.Find("RuleManager");
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        RuleManager.SendMessage(string.Format("{0}Triggered", name));
        Logger.DebugLog(string.Format("TriggerHit : {0}", name));
        Destroy(gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(string.Format("OntriggerEnter : {0}", other.name));
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log(string.Format("OnCollisionEnter : {0}", collision.gameObject.name));
    //}
}
