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
	private int number = 0;
	private bool bigMonsterSpawned = true;
	private string gameStatus;
	// Use this for initialization
	void Start () {
		timeCount = 2f;
		
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
		if(killCount.GetCount()%10 == 0 && !bigMonsterSpawned)
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
		}else if(killCount.GetCount() % 10 == 1){
			bigMonsterSpawned = false;
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
					Lean.LeanPool.Spawn(enemy[1],
										spawnPosition,
										Quaternion.identity,
										transform);

					
					bigMonster(player.transform,offsetX,offsetZ);
					// Instantiate(enemy[Random.Range(0, enemy.Count)],spawnPosition,Quaternion.identity);
				}
				timeCount = interval;
			}
		}
	}
}
