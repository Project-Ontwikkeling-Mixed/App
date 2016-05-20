using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    string[,] vragenLijst;
    float initTimer = 1;
    int huidigeVraag;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        initTimer -= Time.deltaTime;
	
	}

    void Update()
    {
        if (initTimer <= 0)
        {
            vragenLijst = GetComponent<AntwoordGegevens>().vragenLijst;
            huidigeVraag = GetComponent<AntwoordGegevens>().vraagNummer;
            GameObject.Find("Titel").GetComponent<Text>().text = "Vraag " + GetComponent<AntwoordGegevens>().vraagNummer.ToString() + ":";
            GameObject.Find("Vraag").GetComponent<Text>().text = vragenLijst[huidigeVraag-1,0];
            GameObject.Find("Label1").GetComponent<Text>().text = vragenLijst[huidigeVraag - 1, 2];
            GameObject.Find("Label2").GetComponent<Text>().text = vragenLijst[huidigeVraag - 1, 3];
            GameObject.Find("Label3").GetComponent<Text>().text = vragenLijst[huidigeVraag - 1, 4];
        }
    }
}
