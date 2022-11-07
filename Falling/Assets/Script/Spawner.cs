using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public float TimeToSpawn;
    float timer;
    public List<GameObject> ObjectPrefabs = new List<GameObject>();
    void Update()
    {
        if(Time.time > timer)
        {
            timer = Time.time + TimeToSpawn;

            int random = Random.Range(0, ObjectPrefabs.Count);

            Instantiate(ObjectPrefabs[random], transform.position, transform.rotation);
        }
    }
}
