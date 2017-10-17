using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	public void startGame(){
		SetVisible(false);
		KillCount uiTextComponent = GameObject.FindGameObjectWithTag("UIcount").GetComponent<KillCount>();
		uiTextComponent.CountInit();
	}
	public void stopGame(){
		SetVisible(true);
		
	}

	void SetVisible(bool input){
		if(!input){
			transform.localPosition = new Vector3(1000f,1000f,1000f);	
		}else{
			transform.localPosition = new Vector3(-6f,4f,20f);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
