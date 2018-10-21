using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    public float spawnDelay = 30f;
    private float nextSpawnIn;
    private int currentWave = 0;
    private bool waveSpawned = false;

    private float healthScaling = 0.5f;
    private int enemySpawnCount = 20;

    public GameObject enemyToSpawn = null;
    public static List<GameObject> Enemies = new List<GameObject>();
    
	// Use this for initialization
	public override void OnStartServer() {
        nextSpawnIn = spawnDelay;
    }

    // Update is called once per frame
    void Update () {
        if (!isServer)
            return;

        if (!waveSpawned)
        {
            nextSpawnIn -= Time.deltaTime;
            GameBoard.UpdatePlayerGameStatusText("Next Wave in:\n" + (int)nextSpawnIn);
        }
        else
        {
            GameBoard.UpdatePlayerGameStatusText("Remaining Enemies:\n" + Enemies.Count);
        }

        if(!waveSpawned && nextSpawnIn <= 0)
        {
            SpawnEnemies();
            waveSpawned = true;
        }
        if(waveSpawned && Enemies.Count == 0)
        {
            waveSpawned = false;
            nextSpawnIn = spawnDelay;
        }

        if(Input.GetMouseButton(1))
        {
            SpawnEnemies();
        }
    }

    [Server]
    public void SpawnEnemies()
    {

        var players = GameObject.FindGameObjectsWithTag("Player");
        float spawnDistance = GameSettings.GetGameBoardSize().x/3/2-5;

        for (int i = 0; i < enemySpawnCount; i++)
        {
            var randomPlayer = players[Random.Range(0, players.Length)];

            var angle = Random.Range(0, 360);

            var xPosition = spawnDistance * Mathf.Cos(angle);
            var yPosition = spawnDistance * Mathf.Sin(angle);

            var newEnemy = Instantiate(enemyToSpawn, new Vector3(xPosition, yPosition), Quaternion.identity);
            Enemies.Add(newEnemy);
            NetworkServer.Spawn(newEnemy);
        }
    }
}