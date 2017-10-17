using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagReload : MonoBehaviour {

	// Use this for initialization
	public GameObject controllerLeft;
	private SteamVR_Controller.Device device;
	private SteamVR_TrackedObject trackedObject;
	private SteamVR_TrackedController controller;
	private bool collision ;
	private bool systemPause;
	void Start () {
		controller = controllerLeft.GetComponent<SteamVR_TrackedController>();
		controller.Gripped+= reload;
		//controller.TriggerClicked += systemtest;
		trackedObject = controllerLeft.GetComponent<SteamVR_TrackedObject>();
		systemPause = false;
	}
	void systemtest(){
		if(systemPause){
			Time.timeScale = 1.0f;
			systemPause = false;
		}else{
			Time.timeScale = 0f;
			systemPause = true;
		}
	}
	void reload(object sender, ClickedEventArgs e){
		//systemtest();
		if(collision){
			gun_script reload = GameObject.FindGameObjectWithTag("AK47").GetComponent<gun_script>();
			reload.reloadbullet();
			collision = false;
		}
	}
	void OnTriggerEnter(Collider other)
	{
		collision = true;
	}
	void OnTriggerExit(Collider other)
	{
		collision = false;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
