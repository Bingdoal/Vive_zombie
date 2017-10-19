using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour {
	[Tooltip("產怪的等級")][SerializeField]private string _level = "1";
	[Tooltip("產怪的總量")][SerializeField]private int enemyCount = 60;
	[Tooltip("產怪的間隔")][SerializeField]private float interval = 2f;
	private float timeCount = 0;
	private Transform spawnPoints;
	private List<GameObject> enemy = new List<GameObject>();
	// private Vector3 fixPosition = new Vector3(125.6f,16.8f,34.3f);
	private Vector3 fixPosition = new Vector3(0f,0f,0f);
	private int number = 0;
	private string gameStauts;
	// Use this for initialization
	void Start () {
		timeCount = 2f;
		
		Transform spawnPoint = gameObject.transform.Find("SpawnPoints");
		// Transform SpawnPoints = GameObject.FindGameObjectWithTag ("SpawnPoints").transform;

		spawnPoints = spawnPoint.GetComponentInChildren<Transform>();
		StartCoroutine (Loaditem ("monster/level1")); 
	}
	IEnumerator Loaditem(string path){
		Object[] obj = Resources.LoadAll(path,typeof(GameObject));
			for(int i = 0;i<obj.Length;i++){
				enemy.Add((GameObject)obj [i]);
				yield return 0;
			}
			yield return 0;
	}
	public void SetGameStatus(string input){
		gameStauts = input;
	}
	// Update is called once per frame
	void Update () {
		if(gameStauts.Equals("playing")){
			if(timeCount>0){
				timeCount -= Time.deltaTime;
			}else{
				int liveEnemyCount = transform.childCount - 2;
				if(liveEnemyCount < enemyCount){
					Transform spawn = spawnPoints.GetChild(Random.Range(0,spawnPoints.childCount-1));
					// spawn.SetPositionAndRotation(new Vector3(0,0,0),new Quaternion());
					Vector3 spawnPosition = new Vector3(spawn.position.x - fixPosition.x,
														spawn.position.y - fixPosition.y,
														spawn.position.z - fixPosition.z);
					Lean.LeanPool.Spawn(enemy[Random.Range(0,enemy.Count)],
										spawnPosition,
										Quaternion.identity,
										transform);
				}
				timeCount = interval;
			}
		}else{
			// print("game status now is"+gameStauts);
		}
	}
}
