using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagReload : MonoBehaviour
{

    // Use this for initialization
    public GameObject controllerGunHand;
    public GameObject magHand;
    private SteamVR_Controller.Device device;
    private SteamVR_TrackedObject trackedObject;
	private SteamVR_TrackedController gun;
	private SteamVR_TrackedController mag;

    private bool collision;
    void Start()
    {
         gun = controllerGunHand.GetComponent<SteamVR_TrackedController>();
        gun.Gripped += _reload;

		 mag = magHand.GetComponent<SteamVR_TrackedController>();
		mag.TriggerClicked+=getMag;
        
        trackedObject = magHand.GetComponent<SteamVR_TrackedObject>();

    }
    void test(object sender, ClickedEventArgs e){

    }
    void _reload(object sender, ClickedEventArgs e)
    {
        if (collision)
        {
            gun_script gun = GameObject.FindGameObjectWithTag("AK47").GetComponent<gun_script>();
            gun.Reloadbullet();
            collision = false;
			device = SteamVR_Controller.Input((int)trackedObject.index);
			device.TriggerHapticPulse(3999);
			this.gameObject.SetActive(false);
        }
    }
	void getMag(object sender, ClickedEventArgs e){
		this.gameObject.SetActive(true);
	}
	private void OnTriggerStay(Collider other) {
		collision = true;
	}
    void OnTriggerEnter(Collider other){
		 collision = true;
	}
	void OnTriggerExit(Collider other){
		collision = false;
	}
	// Update is called once per frame
    void Update()
    {
        
    }
}
