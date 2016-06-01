using UnityEngine;
using System.Collections;

public class inlogtest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Login(string name, string password)
	{
		WWWForm form = new WWWForm();
		form.AddField("name", name);
		form.AddField("password", password);

		WWW www = new WWW("http://mixed.multimediatechnology.be/login.json", form);


		
	}

	public void testLogin()
	{
		Login("sacha.meertens@hotmaail.com", "mopmopmop");
	}
}
