using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectPool
{
    private GameObject prefab;
    private Transform poolParentTransform;
    private int batchSize;
    private List<GameObject> pool;
    private List<GameObject> objectsInUse;

    public GameObjectPool(GameObject prefab, Transform poolParentTransform, int batchSize = 100)
    {
        this.prefab = prefab;
        this.poolParentTransform = poolParentTransform;
        this.batchSize = batchSize;
        pool = new List<GameObject>();
        objectsInUse = new List<GameObject>();
        InitializeNewBatch();
    }

    public GameObject[] GetObjects(int count)
    {
        while (pool.Count < count)
            InitializeNewBatch();
        var result = pool.Take(count).ToArray();
        foreach (var gameObject in result)
        {
            objectsInUse.Add(gameObject);
            pool.Remove(gameObject);
        }
        return result;
    }

    public void ReleaseObjects(IEnumerable<GameObject> gameObjects)
    {
        foreach (var gameObject in gameObjects.Where(gameObject => objectsInUse.Contains(gameObject)))
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(poolParentTransform);
            objectsInUse.Remove(gameObject);
            pool.Add(gameObject);
        }
    }

    private void InitializeNewBatch()
    {
        for (var i = 0; i < batchSize; ++i)
        {
            var gameObject = GameObject.Instantiate(prefab, poolParentTransform);
            gameObject.SetActive(false);
            pool.Add(gameObject);
        }
    }
}