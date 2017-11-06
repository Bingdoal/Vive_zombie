using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBox : MonoBehaviour {

	[Tooltip("補血量")][SerializeField] private float healamount;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void heal()
	{
		PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
	}
	void Update () {
		
	}
}
