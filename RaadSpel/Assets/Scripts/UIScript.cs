using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    string[] ThisQuestion;
    int CurrentQuestion;


    GameObject Canvas4;
    GameObject Canvas3;


	void Start () {
        Canvas3 = GameObject.Find("Canvas3Antwoord");
        Canvas4 = GameObject.Find("Canvas4Antwoord");
        Canvas3.SetActive(false);
        Canvas4.SetActive(false);

	
	}
	
	void FixedUpdate () {
	
	}

    void Update()
    {
        CurrentQuestion = GetComponent<QuestionData>().QuestionNr;
        ThisQuestion = GetComponent<QuestionData>().ThisQuestion;

        //check of vraag geladen is
        if (CurrentQuestion < 1)
        {
            GameObject.Find("Vraag").GetComponent<Text>().text = "Vraag aan het laden...";
            GameObject.Find("Titel").GetComponent<Text>().text = "Vraag X";
            Debug.Log("Question loading...");
            return;
            
        }


        //juiste canvas actief zetten
        if (int.Parse(ThisQuestion[1]) == 3)
        {
            Canvas3.SetActive(true);
        }
        else if (int.Parse(ThisQuestion[1]) == 4)
        {
            Canvas4.SetActive(true);
        }


        if (ThisQuestion[1] == null || ThisQuestion[1] == "")
        {

        }


        //vragen in labels zetten
        if (int.Parse(ThisQuestion[1]) >= 3)
        {
            GameObject.Find("Titel").GetComponent<Text>().text = "Vraag " + GetComponent<QuestionData>().QuestionNr.ToString() + ":";
            GameObject.Find("Vraag").GetComponent<Text>().text = ThisQuestion[0];
            GameObject.Find("Label1").GetComponent<Text>().text = ThisQuestion[2];
            GameObject.Find("Label2").GetComponent<Text>().text = ThisQuestion[3];
            GameObject.Find("Label3").GetComponent<Text>().text = ThisQuestion[4];

        }
        if (int.Parse(ThisQuestion[1]) == 4)
        {
            GameObject.Find("Label4").GetComponent<Text>().text = ThisQuestion[5];
        }

            
        //}
    }
}
