using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Time.timeScale /Time.timeScale = 0;系統時間軸歸零  也就是所有跟時間軸有關的東西都不會動
		EnemySpawnController enemySpawnController= GameObject.FindGameObjectWithTag("SpawnPoints").GetComponent<EnemySpawnController>();
		enemySpawnController.SetGameStatus("pause");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
