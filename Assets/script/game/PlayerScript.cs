using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	[Tooltip("血量")][SerializeField] private float healthPoint;
	[Tooltip("被打音效")][SerializeField] private AudioClip attackedAC ;
	[Tooltip("死亡音效")][SerializeField] private AudioClip dieAC ;

	private AudioSource audioSource;
	public string status = "alive";
	
	// Use this for initialization
	void Start () {
		_changeUI();
		audioSource = this.GetComponent<AudioSource> ();
		if (audioSource == null) {
			audioSource = this.gameObject.AddComponent<AudioSource> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ApplyDamage(float _damage){
		float t = healthPoint - _damage;
		PlaySound(attackedAC);
		if (t > 0) {
			healthPoint = t;
		} else {
			status = "dead";
			print(status);
			healthPoint = 10;
			_stopSpawn();
			_playerDead();
			// menu.SetActive(true);
		}
		_changeUI();
	}
	void PlaySound(AudioClip audioClip){
		audioSource.Stop();
		audioSource.clip = audioClip;
		if(!audioSource.isPlaying){
			audioSource.Play();
		}
		
	}

	void _changeUI(){
		UIScript uIScript = GameObject.FindGameObjectWithTag("UI").GetComponent<UIScript>();
		uIScript.setHP(healthPoint);
	}

	void _stopSpawn(){
		EnemySpawnController enemySpawnController= GameObject.FindGameObjectWithTag("SpawnPoints").GetComponent<EnemySpawnController>();
		enemySpawnController.SetGameStatus("pause");

		GameObject[] enemyList = GameObject.FindGameObjectsWithTag("monster");
			foreach(GameObject enemy in enemyList){
				Lean.LeanPool.Despawn(enemy);
		}
	}
	void _playerDead(){
		StartGame menu = GameObject.FindGameObjectWithTag("menu").GetComponent<StartGame>();
		menu.stopGame();
		PlaySound(dieAC);
	}
}
