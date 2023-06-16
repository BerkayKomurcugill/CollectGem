using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SpawnRate
{
    Low,
    Mid,
    High,
    VeryHigh
}
public class GridSystem : MonoBehaviour
{
    public SpawnRate spawnRate;
    public GameObject cubePrefab;
    public int x;
    public int z;
    public List<GameObject> cubeSpawnerList = new List<GameObject>();
    float timeLeft = 2;
    public Vector3 gridOrigin = Vector3.zero;
    void Start()
    {
        if (spawnRate == SpawnRate.Low)
            timeLeft = 2;
        if (spawnRate == SpawnRate.Mid)
            timeLeft = 1;
        if (spawnRate == SpawnRate.High)
            timeLeft = 0.5f;
        if (spawnRate == SpawnRate.VeryHigh)
            timeLeft = 0.1f;

        SpawnGrid();
    }
    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            if (cubeSpawnerList.Count > 0)
            {
                Transform tr = cubeSpawnerList[Random.Range(0, cubeSpawnerList.Count)].transform;
                tr.gameObject.GetComponent<GemSpawn>().Spawn();
                cubeSpawnerList.Remove(tr.gameObject);
                timeLeft = 2;
            }

        }
    }
    void SpawnGrid()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                Vector3 spawnPosition = new Vector3(i * cubePrefab.transform.localScale.x, 0, j * cubePrefab.transform.localScale.z) + gridOrigin;
                Spawner(spawnPosition);
            }
        }
    }
    void Spawner(Vector3 positionSpawn)
    {
        GameObject go = Instantiate(cubePrefab, positionSpawn, Quaternion.identity);
        go.transform.parent = this.transform;
        cubeSpawnerList.Add(go);
    }




}
