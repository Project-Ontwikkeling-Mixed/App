using UnityEngine;
using System.Collections;

public class commandScript : MonoBehaviour {

    public bool sendAnswer = false;


	public void validateAnswer()
    {
        GameObject.Find("localObject").GetComponent<Antwoord>().sendAnswer = true;
    }


    void Update()
    {
        
    }
}
