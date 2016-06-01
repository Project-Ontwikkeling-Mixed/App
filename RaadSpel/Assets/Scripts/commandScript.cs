using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class commandScript : MonoBehaviour {

    public bool sendAnswer = false;


	public void validateAnswer()
    {
        GameObject.Find("Result").GetComponent<Text>().text = "answer validated";
        GameObject.Find("localObject").GetComponent<GameManager>().sendAnswer = true;
       
        
    }


    void Update()
    {
        //try
        //{
        //    if (GameObject.Find("Volgende").activeSelf == true)
        //    {
        //        GameObject.Find("Result1").GetComponent<Text>().text = "running";
        //    }
        //    else
        //    {
        //        GameObject.Find("Result1").GetComponent<Text>().text = "not running";
        //    }
        //}
        //catch (System.Exception e)
        //{

        //    GameObject.Find("Result1").GetComponent<Text>().text = "not running";
        //}
        
        
    }
}
