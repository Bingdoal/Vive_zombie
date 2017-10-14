using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour {
	private string textMeshContain;
	private TextMesh textMesh;
	private float healthPoint;
	private int bullet;
	// Use this for initialization
	void Start () {
		textMesh = this.GetComponent<TextMesh>();
		_setText();
		textMeshContain = textMesh.text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setBullet(int bulletNum){
		bullet = bulletNum;
		_setText();
	}

	public void setHP(float hp){
		healthPoint = hp;
		_setText();
	}

	void _setText(){
		textMesh.text = "血量:"+ healthPoint + "\n子彈:" + bullet;
	}

}
