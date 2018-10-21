using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameBoard : NetworkBehaviour {
    
    public static List<GameObject> Buildings = new List<GameObject>();
    private GameObject gameArea;
    public GameObject invisWallPrefab;
    public static List<GameObject> Playerlist = new List<GameObject>();
    public GameObject gameAreaPrefab;

    // Use this for initialization
    public override void OnStartServer()
    {
        Playerlist = new List<GameObject>();
        gameArea = Instantiate(gameAreaPrefab);
        GameSettings.SetGameSettings(BoardSize.Medium, true);
        CreateGameArea();
        NetworkServer.Spawn(gameArea);
	}

    private void CreateGameArea()
    {
        var gameAreaSize = GameSettings.GetGameBoardSize();
        gameArea.transform.localScale = gameAreaSize*0.3f;

        var leftWall = Instantiate(invisWallPrefab);
        leftWall.transform.localScale = new Vector2(1, gameAreaSize.y / 3);
        leftWall.transform.position = new Vector2(gameAreaSize.x / -2 *0.3f -0.5f, 0);
        NetworkServer.Spawn(leftWall);

        var rightWall = Instantiate(invisWallPrefab);
        rightWall.transform.localScale = new Vector2(1, gameAreaSize.y / 3);
        rightWall.transform.position = new Vector2(gameAreaSize.x / 2 * 0.3f + 0.5f, 0);
        NetworkServer.Spawn(rightWall);

        var topWall = Instantiate(invisWallPrefab);
        topWall.transform.localScale = new Vector2(gameAreaSize.y / 3, 1);
        topWall.transform.position = new Vector2(0, gameAreaSize.x / 2 * 0.3f + 0.5f);
        NetworkServer.Spawn(topWall);

        var bottomWall = Instantiate(invisWallPrefab);
        bottomWall.transform.localScale = new Vector2(gameAreaSize.y / 3, 1);
        bottomWall.transform.position = new Vector2(0, gameAreaSize.x / -2 * 0.3f - 0.5f);
        NetworkServer.Spawn(bottomWall);
    }

    public static void UpdatePlayerGameStatusText(string text)
    {
        foreach(var player in Playerlist)
        {
            var p = player.GetComponent<PlayerController>();
            p.RpcUpdateGameStatusText(text);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
