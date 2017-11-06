using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StartGame : MonoBehaviour
{
    private Vector3 initPosition;
    // Use this for initialization
    void Start()
    {
        initPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
    }
    void Update()
    {

    }
    // Pubic method
    public void GameStart()
    {
        _setVisible(false);
        KillCount uiTextComponent = GameObject.FindGameObjectWithTag("UIcount").GetComponent<KillCount>();
        uiTextComponent.CountInit();
    }
    public void GameStop()
    {
        _setVisible(true);
        _stopSpawn();
        KillCount killCount = GameObject.FindGameObjectWithTag("UIcount").GetComponent<KillCount>();
        gun_script ak47 = GameObject.FindGameObjectWithTag("AK47").GetComponent<gun_script>();
        bestKillScript bestKill = GameObject.FindGameObjectWithTag("bestKill").GetComponent<bestKillScript>();
        ak47.Reloadbullet();
        bestKill.UpdateBestKill(killCount.GetCount());
        print("擊殺數:"+killCount.GetCount());
    }

    // Private method
    void _setVisible(bool input)
    {
        if (!input)
        {
            transform.localPosition = new Vector3(1000f, 1000f, 1000f);
        }
        else
        {
            transform.localPosition = initPosition;
        }
    }
    void _stopSpawn()
    {
        EnemySpawnController enemySpawnController = GameObject.FindGameObjectWithTag("SpawnPoints").GetComponent<EnemySpawnController>();
        enemySpawnController.SetGameStatus("pause");

        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("monster");
        foreach (GameObject enemy in enemyList)
        {
            Lean.LeanPool.Despawn(enemy);
        }
    }
}
