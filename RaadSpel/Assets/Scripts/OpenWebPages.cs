using UnityEngine;
using System.Collections;

public class OpenWebPages : MonoBehaviour {

	string registerURL = "http://mixed.multimediatechnology.be/register";
	string websiteURL = "http://mixed.multimediatechnology.be/";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void openRegister()
	{
		Application.OpenURL(registerURL);
	}

	public void openWebsite()
	{
		Application.OpenURL(websiteURL);
	}
}
