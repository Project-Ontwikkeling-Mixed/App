using UnityEngine;
using System.Collections;

public class commandScript : MonoBehaviour {

    public bool sendAnswer = false;


	public void validateAnswer()
    {
        GameObject.Find("localObject").GetComponent<GameManager>().sendAnswer = true;
    }


    void Update()
    {
        
    }
}
