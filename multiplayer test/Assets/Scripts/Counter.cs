using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Counter : NetworkBehaviour {

    [SyncVar]
    public int counter = 0;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Counter").GetComponent<Text>().text = counter.ToString();



        if (Input.touchCount > 0 || Input.GetButton("Fire1"))
        {
            CmdCounter();

            
            

        }

        
    }

    [Command]
    void CmdCounter()
    {
        counter += 1;
    }
}
