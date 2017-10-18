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
    public void startGame()
    {
        _setVisible(false);
        KillCount uiTextComponent = GameObject.FindGameObjectWithTag("UIcount").GetComponent<KillCount>();
        uiTextComponent.CountInit();
    }
    public void stopGame()
    {
        _setVisible(true);
        _stopSpawn();
    }

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
    // Update is called once per frame
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
