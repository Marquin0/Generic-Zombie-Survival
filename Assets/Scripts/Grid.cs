using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public GameObject gridPrefab;
    private GameObject[,] gridObjects;
    public Camera cam;

	// Use this for initialization
	public void SetCamera (Camera camera) {
        cam = camera;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        gridObjects = new GameObject[(int)(width/0.3) + 6, (int)(height/0.3) + 6];
        for(int x = 0; x < gridObjects.GetLength(0); x++)
        {
            for(int y = 0; y < gridObjects.GetLength(1); y++)
            {
                Vector2 position = new Vector2(-gridObjects.GetLength(0) / 2 * 0.3f + 0.3f * x, -gridObjects.GetLength(1) / 2 * 0.3f + 0.3f * y);
                gridObjects[x, y] = Instantiate(gridPrefab, position, Quaternion.identity, transform);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (cam != null)
        {
            Vector2 position = new Vector2((int)(cam.transform.position.x / 0.3f) * 0.3f, (int)(cam.transform.position.y / 0.3f) * 0.3f);
            transform.position = position;
        }
	}
}
