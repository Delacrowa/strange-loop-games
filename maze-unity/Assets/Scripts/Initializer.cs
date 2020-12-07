using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Initializer : MonoBehaviour
{
    private const string WallPrefabName = "WallPrefab";
    [SerializeField] private MazeSpawner _mazeSpawner;

    private void Start()
    {
        GenerateMaze();
        GeneratePathGraph();
        CreateEntrances();
        Destroy(gameObject);
    }

    private void CreateEntrances()
    {
        var walls = _mazeSpawner.GetComponentsInChildren<Transform>()
            .Where(w => w.gameObject.name.Contains(WallPrefabName) && IsBoundaryWall(w.position))
            .ToList();
        CreateEntrance(walls, IsStartWall);
        CreateEntrance(walls, IsEndWall);
    }

    private static bool IsEndWall(Vector3 position) => !IsStartWall(position);

    private static void CreateEntrance(List<Transform> walls, Func<Vector3, bool> match)
    {
        var availableWalls = walls.FindAll(w => match.Invoke(w.position));
        var target = availableWalls[Random.Range(0, availableWalls.Count)];
        Destroy(target.gameObject);
    }

    private static bool IsBoundaryWall(Vector3 position) => Equals(position.x, 38)
                                                            || Equals(position.z, 38)
                                                            || Equals(position.x, -2)
                                                            || Equals(position.z, -2);

    private static bool IsStartWall(Vector3 position) =>
        Equals(position.x, -2) && Equals(position.y, 0) && Equals(position.z, 0)
        || Equals(position.z, -2) && Equals(position.x, 0) && Equals(position.y, 0);

    private static void GeneratePathGraph() => AstarPath.active.Scan();

    private void GenerateMaze() => _mazeSpawner.GenerateMaze();

    private static bool Equals(float a, float b) => Mathf.Approximately(a, b);
}