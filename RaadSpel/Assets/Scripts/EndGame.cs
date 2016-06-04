using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EndGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int mijnScore = PlayerPrefs.GetInt("mijnScore");
        int tegenstanderScore = PlayerPrefs.GetInt("tegenstanderScore");
        GameObject.Find("Punten").GetComponent<Text>().text = "Jouw punten: " + mijnScore + "\n\nTegenstander score: " + tegenstanderScore;
        if (mijnScore > tegenstanderScore)
        {
            GameObject.Find("tekst").GetComponent<Text>().text = "Proficiat!Je hebt gewonnen.";
            GameObject.Find("gewonnen").GetComponent<Text>().text = "Gewonnen";
        }
        else if (tegenstanderScore > mijnScore)
        {
            GameObject.Find("tekst").GetComponent<Text>().text = "Jammer!Je hebt verloren.";
            GameObject.Find("gewonnen").GetComponent<Text>().text = "Verloren";
        }
        else if (tegenstanderScore == mijnScore)
        {
            GameObject.Find("tekst").GetComponent<Text>().text = "Je hebt gelijkgespeeld met je tegenstander!";
            GameObject.Find("gewonnen").GetComponent<Text>().text = "Gelijkspel";
        }

	
	}
	
	public void returnToHome()
    {
        if (PlayerPrefs.GetString("username") == null || PlayerPrefs.GetString("username") == "")
        {
            SceneManager.LoadScene("Menu uitgelogd");
        }
        else
        {
            SceneManager.LoadScene("Menu ingelogd");
        }
        
    }
}
