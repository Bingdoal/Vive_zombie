using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bestKillScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<Text>().text = PlayerPrefs.GetInt("bestKillNum").ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void UpdateBestKill(int killNum){
		if(killNum > PlayerPrefs.GetInt("bestKillNum"))
			PlayerPrefs.SetInt("bestKillNum",killNum);
		this.GetComponent<Text>().text = PlayerPrefs.GetInt("bestKillNum").ToString();
	}
}
