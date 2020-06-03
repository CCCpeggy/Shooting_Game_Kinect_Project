using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopArea : MonoBehaviour
{
    public static int level = 1;
    public GameObject spawner;
    public GameObject enemyPrefabs;
    public Vector3 enemyRotation;
    float startX, startZ, endX, endZ, Y;
    public bool isShooting;
    
    void Start()
    {
        Vector3 size = spawner.transform.rotation * Vector3.Scale(spawner.GetComponent<BoxCollider>().size, spawner.transform.localScale);
        size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
        Vector3 enemySize = Quaternion.Euler(enemyRotation) * Vector3.Scale(enemyPrefabs.GetComponent<BoxCollider>().size, enemyPrefabs.transform.localScale);
        enemySize = new Vector3(Mathf.Abs(enemySize.x), Mathf.Abs(enemySize.y), Mathf.Abs(enemySize.z));
        Debug.Log("size: "+ size);
        Debug.Log("enemySize: " + enemySize);
        Vector3 pos = spawner.transform.position;
        startX = pos.x - size.x / 2 + enemySize.x / 2;
        startZ = pos.z - size.z / 2 + enemySize.z / 2;
        Y = pos.y + enemySize.y / 2;
        endX = pos.x + size.x / 2 - enemySize.x / 2;
        endZ = pos.z + size.z / 2 - enemySize.z / 2;
        isShooting = false;
    }

    void Update()
    {
        if (transform.parent.transform.childCount <= 2)
        {
            isShooting = false;
        }
    }

    public void SpawnEnemy()
    {
        int enemyCount = level + Random.Range(0, level / 4);
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 newPos = new Vector3(Random.Range(startX, endX), Y, Random.Range(startZ, endZ));
            GameObject enemy = Instantiate(enemyPrefabs, newPos, Quaternion.Euler(enemyRotation));
            enemy.GetComponent<Enemy>().SetLevel(level);
            enemy.transform.SetParent(transform.parent.transform);
        }
        level++;
        isShooting = true;
        Debug.Log(level);
    }
}
