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
	void Start () {
		controller = controllerLeft.GetComponent<SteamVR_TrackedController>();
		controller.Gripped+= reload;
		trackedObject = controllerLeft.GetComponent<SteamVR_TrackedObject>();
	}
	void reload(object sender, ClickedEventArgs e){
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
