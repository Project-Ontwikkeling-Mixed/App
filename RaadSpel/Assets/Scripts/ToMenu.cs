using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ToMenu : MonoBehaviour {

    public NetworkManager netMan;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void changeSceneMenu()
    {
        netMan.ServerChangeScene("Menu ingelogd");
    }

    
}
