using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpenCloseUI : MonoBehaviour {

	public GameObject panelUI;

	// Use this for initialization
	void Start () {
        CloseUI();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OpenUI()
	{
		panelUI.gameObject.SetActive(true);
		
	}

	public void CloseUI()
	{
		panelUI.gameObject.SetActive(false);

	}
}
