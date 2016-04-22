using UnityEngine;
using System.Collections;

public class fireBall : MonoBehaviour {
    float velocity = 100f;
    float timeToLive = 3f;
    float tempTime = 0;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        tempTime += Time.deltaTime;
        if(tempTime > timeToLive)
        {
            Destroy(gameObject);
        }
	}

    void OnCollisonEnter(Collision2D collision)
    {
        GameObject hit = collision.gameObject;
        bool hitPlayer = hit.GetComponent<PlayerMove>();
        if(hitPlayer != null)
        {
            Destroy(gameObject);
        }
    }
}
