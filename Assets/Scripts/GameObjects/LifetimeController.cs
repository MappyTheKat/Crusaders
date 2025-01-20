using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LifetimeController : MonoBehaviour
{
    [Tooltip("Lifetime in Seconds")]
    public float Lifetime = 1f;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Lifetime -= Time.deltaTime;
        if (Lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
