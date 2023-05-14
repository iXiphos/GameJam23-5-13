using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMScript : MonoBehaviour
{
    public GameObject twigPrefab, stickPrefab, logPrefab;
    public int numberCollectablesAtGameStart = 15;
    public float objectSpawnRadius = 30f;
    public GameObject tilePrefab, grassPrefab;
    // Start is called before the first frame update
    void Start()
    {
        spawnScenery(25);
        spawnCollectables(numberCollectablesAtGameStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnScenery(int numSceneryToSpawn)
    {
        Vector3 startPos = new Vector2(-40, -40);
        for(int i = 0; i < 25; ++i)
        {
            for (int j = 0; j < 25; ++j)
            {
                GameObject tile = Instantiate(tilePrefab, startPos + new Vector3(i * 3.5f, j * 3.5f, 0), Quaternion.identity);
                tile.transform.parent = gameObject.transform;
            }
        }

        for (int i = 0; i < numSceneryToSpawn; ++i)
        {
            Vector3 newObjPos = new Vector3(Random.Range(-objectSpawnRadius, objectSpawnRadius), Random.Range(-objectSpawnRadius, objectSpawnRadius), 0);
            GameObject grass = Instantiate(grassPrefab, newObjPos, Quaternion.identity);
            grass.transform.parent = gameObject.transform;
        }
    }

    void spawnCollectables(int numCollectables)
    {
        for(int i = 0; i < numCollectables; ++i)
        {
            Vector3 newObjPos = new Vector3(Random.Range(-objectSpawnRadius, objectSpawnRadius), Random.Range(-objectSpawnRadius, objectSpawnRadius), 0);
            int rng = Random.Range(0, 11);
            if(rng == 10)
            {
                //spawn log 1 in 10 times
                Instantiate(logPrefab, transform.position + newObjPos, Quaternion.identity);
            }
            else if (rng > 5 && rng < 10)
            {
                //spawn stick 4 in 10 times
                Instantiate(stickPrefab, transform.position + newObjPos, Quaternion.identity);
            }
            else
            {
                //spawn twig 5 in 10 times
                Instantiate(twigPrefab, transform.position + newObjPos, Quaternion.identity);
            }
        }
    }
}
