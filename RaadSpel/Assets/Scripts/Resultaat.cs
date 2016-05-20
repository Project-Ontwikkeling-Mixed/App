using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Resultaat : NetworkBehaviour {

    int juisteAntwoord;
    int hoogstePercent = -1;

    int clientAnswer;
    int serverAnswer;

    [SyncVar]
    bool clientBool;
    [SyncVar]
    bool serverBool;




    int vraagNummer;
    const string vragenPath = "gameData\\vragenLijst.mixed";
    string[,] vragenLijst;
    string[] antwoorden;
    int[] percenten;
    int aantalAntwoorden;


	// Use this for initialization
	void Start () {
        if (SceneManager.GetActiveScene().name != "Result")
        {
            return;
        }

        if (isServer)
        {
            readAnswers();
        }
        
	
	}
	
	// Update is called once per frame
	void Update () {

        if (SceneManager.GetActiveScene().name != "Result")
        {
            return;
        }


        if (isServer)
        {
            if (serverAnswer == juisteAntwoord)
            {
                serverBool = true;

            }
            else
            {
                serverBool = false;

            }

            if (clientAnswer == juisteAntwoord)
            {
                clientBool = true;
            }
            else
            {
                clientBool = false;
            }
        }
        




        if (isServer && serverBool)
        {
            if (clientBool)
            {
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt juist geraden!\nJe tegenstandeer heeft ook juist geraden!";
            }
            else
            {
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt juist geraden!\nJe tegenstandeer heeft fout geraden!";
            }
            
        }
        else if (isServer)
        {
            if (clientBool)
            {
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt fout geraden!\nJe tegenstander heeft juist geraden!";
            }
            else
            {
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt fout geraden!\nJe tegenstander heeft ook fout geraden!";
            }
            
        }

        if (!isServer && clientBool)
        {
            if (serverBool)
            {
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt juist geraden!\nJe tegenstandeer heeft ook juist geraden!";
            }
            else
            {
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt juist geraden!\nJe tegenstandeer heeft fout geraden!";
            }
        }
        else if (!isServer)
        {
            if (serverBool)
            {
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt fout geraden!\nJe tegenstander heeft juist geraden!";
            }
            else
            {
                GameObject.Find("Result").GetComponent<Text>().text = "Je hebt fout geraden!\nJe tegenstander heeft ook fout geraden!";
            }
        }
	
	}


    void readAnswers()
    {
        StreamReader inputStream = File.OpenText(vragenPath);
        vraagNummer = int.Parse(inputStream.ReadLine());
        int tempLength = int.Parse(inputStream.ReadLine());
        int tempHeight = int.Parse(inputStream.ReadLine());
        string[] tempVraagArr;
        vragenLijst = new string[tempLength, tempHeight];
        for (int i = 0; i < tempLength; i++)
        {
            tempVraagArr = inputStream.ReadLine().Split(' ');
            for (int j = 0; j < tempHeight; j++)
            {
                vragenLijst[i, j] = tempVraagArr[j];
            }
        }
        inputStream.Close();
        aantalAntwoorden = 3;


        antwoorden = new string[3];
        percenten = new int[3];
        //for (int i = 0; i < vragenLijst.GetLength(1); i++)
        //{
        //    Debug.Log("Vraag Debug: " + vragenLijst[vraagNummer - 1, i]);
        //}


        int antwI= 0;
        for (int i = 2 ; i < 2 + aantalAntwoorden; i++)
        {
            Debug.Log("VraagNummer= " + vraagNummer);
            antwoorden[antwI] = vragenLijst[vraagNummer - 2, i];
            antwI++;
        }
        int percI = 0;
        for (int i = 2 + aantalAntwoorden; i < 2 + aantalAntwoorden * 2; i++)
        {
            //Debug.Log("Vraag Debug: " + vragenLijst[vraagNummer - 1, i]);
            percenten[percI] = int.Parse(vragenLijst[vraagNummer - 2, i]);
            percI++;
        }

        for (int i = 0; i < percenten.Length; i++)
        {
            if (percenten[i] > hoogstePercent)
            {
                hoogstePercent = percenten[i];
                juisteAntwoord = i + 1;
            }
        }

        //for (int i = 0; i < percenten.Length; i++)
        //{
        //    Debug.Log("Percent: " + percenten[i]);
        //}


        GameObject.Find("Results").GetComponent<Text>().text = "Resultaten:\n";

        for (int i = 0; i < aantalAntwoorden; i++)
        {
            GameObject.Find("Results").GetComponent<Text>().text = GameObject.Find("Results").GetComponent<Text>().text + antwoorden[i] + ": " + percenten[i] + "%\n";
        }


        



        StreamReader reader = File.OpenText("gameData\\Antwoordgegevens.mixed");
        serverAnswer = int.Parse(reader.ReadLine());
        clientAnswer = int.Parse(reader.ReadLine());
        reader.Close();
    }
}
