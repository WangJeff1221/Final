using UnityEngine;
using System.Collections.Generic;

public class InfiniteBackground : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject tilePrefab;
    public int tileSize = 10;
    public int viewDistanceInTiles = 2;

    private Dictionary<Vector2Int, GameObject> tiles = new();
    private Vector2Int lastCamTile;

    void Start()
    {
        if (!mainCamera) mainCamera = Camera.main;
        UpdateTiles(); 
    }

    void Update()
    {
        Vector2 camPos = mainCamera.transform.position;
        Vector2Int camTile = new(
            Mathf.FloorToInt(camPos.x / tileSize),
            Mathf.FloorToInt(camPos.y / tileSize)
        );
        if (camTile != lastCamTile)
        {
            lastCamTile = camTile;
            UpdateTiles(); 
        }
    }

    void UpdateTiles()
    {
        var needed = new HashSet<Vector2Int>();
        for (int x = -viewDistanceInTiles; x <= viewDistanceInTiles; x++)
            for (int y = -viewDistanceInTiles; y <= viewDistanceInTiles; y++)
                needed.Add(new Vector2Int(lastCamTile.x + x, lastCamTile.y + y));

        foreach (var kv in new Dictionary<Vector2Int, GameObject>(tiles))
        {
            if (!needed.Contains(kv.Key))
            {
                Destroy(kv.Value);
                tiles.Remove(kv.Key);
            }
        }

        foreach (var coord in needed)
        {
            if (!tiles.ContainsKey(coord))
            {
                Vector3 worldPos = new Vector3(coord.x * tileSize, coord.y * tileSize, 10f);
                GameObject go = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);
                tiles[coord] = go;
            }
        }
    }
}
