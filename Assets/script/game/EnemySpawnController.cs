using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour {
	[Tooltip("產怪的等級")][SerializeField]private string _level = "1";
	[Tooltip("產怪的總量")][SerializeField]private int enemyCount = 60;
	[Tooltip("產怪的間隔")][SerializeField]private float interval = 2f;
	[Tooltip("BOSS 的間隔")][SerializeField]private int bossTiming ;

	private float timeCount = 0;
	private Transform spawnPoints;
	private List<GameObject> enemy = new List<GameObject>();
	private int number = 0;
	private bool bigMonsterSpawned = true;
	private string gameStatus;
	// Use this for initialization
	private int _boosTiming;
	void Start () {
		timeCount = 2f;
		_boosTiming = bossTiming;
		Transform spawnPoint = gameObject.transform.Find("SpawnPoints");

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
		gameStatus = input;
	}

	private int spawnTime = 0;
	void bigMonster(Transform player,int offsetX,int offsetZ){
        KillCount killCount = GameObject.FindGameObjectWithTag("UIcount").GetComponent<KillCount>();
		if(killCount.GetCount()%_boosTiming == 0 && !bigMonsterSpawned)
		{
			Vector3 spawnPosition = new Vector3(player.position.x + offsetX,
															player.position.y,
															player.position.z + offsetZ);
			GameObject spawnEnemy = Lean.LeanPool.Spawn(enemy[0],
								spawnPosition,
								Quaternion.identity,
								transform);

            MonsterController monsterController = spawnEnemy.GetComponent<MonsterController>();
			monsterController.OnInit();
			
			bigMonsterSpawned = true;
			print("Monster!!");
			if(_boosTiming >2) _boosTiming --;
		}else if(killCount.GetCount() % _boosTiming == 1){
			bigMonsterSpawned = false;
		}
	}
	void healerSpawn(Transform player, int offsetX, int offsetZ){
		if(Random.Range(0,100) <= 20){
			Vector3 spawnPosition = new Vector3(player.position.x + offsetX,
																player.position.y,
																player.position.z + offsetZ);
			GameObject spawnEnemy = Lean.LeanPool.Spawn(enemy[1],
								spawnPosition,
								Quaternion.identity,
								transform);

			Healer healer = spawnEnemy.GetComponent<Healer>();
			healer.OnInit();
		}
	}
	// Update is called once per frame
	void Update () {
		if(gameStatus.Equals("playing")){
			if(timeCount>0){
				timeCount -= Time.deltaTime;
			}else{
				int liveEnemyCount = transform.childCount - 2;
				if(liveEnemyCount < enemyCount){
					Transform spawn = spawnPoints.GetChild(Random.Range(0,spawnPoints.childCount-1));
					GameObject player = GameObject.FindGameObjectWithTag("Player");
					// Vector3 spawnPosition = new Vector3(spawn.position.x,
					// 									2f,
					// 									spawn.position.z);
					const int baseDis = 10,bestFar = 20;
					int offsetX = Random.Range(-bestFar, bestFar);
					int offsetZ = Random.Range(-bestFar, bestFar);
					if(offsetX > 0 && offsetX < baseDis){
						offsetX = baseDis;
					}else if(offsetX < 0 && offsetX > -baseDis){
						offsetX = -baseDis;
					}

					if (offsetZ > 0 && offsetZ < baseDis)
                    {
                        offsetZ = baseDis;
                    }else if (offsetZ < 0 && offsetZ > -baseDis)
                    {
                        offsetZ = -baseDis;
                    }
					Vector3 spawnPosition = new Vector3(player.transform.position.x + offsetX,
														player.transform.position.y,
														player.transform.position.z + offsetZ);
					GameObject zombie = Lean.LeanPool.Spawn(enemy[2],
										spawnPosition,
										Quaternion.identity,
										transform);
					EnemyController enemyController = zombie.GetComponent<EnemyController>();
					enemyController.OnInit();
					
					bigMonster(player.transform,offsetX,offsetZ);
					healerSpawn(player.transform, offsetX, offsetZ);
					// Instantiate(enemy[Random.Range(0, enemy.Count)],spawnPosition,Quaternion.identity);
				}
				timeCount = interval;
			}
		}else{
			_boosTiming = bossTiming;
			bigMonsterSpawned = true;
		}
	}
}
