using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private PlayerControl player;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    [SerializeField] private float minSpeedUp;
    [SerializeField] private float maxSpeedUp;
    [SerializeField] private GameObject spawner;
    public float minLimit;
    public float maxLimit;
    private float TimeToSpawn;
    private float timer;
    public List<GameObject> ObjectPrefabs = new List<GameObject>();

    private void Update()
    {
        TimeToSpawn = Random.Range(minSpawnTime, maxSpawnTime);
        if (minSpawnTime > minLimit)
        {
            minSpawnTime += minSpeedUp;
        }
        if (maxSpawnTime > maxLimit)
        {
            maxSpawnTime += maxSpeedUp;
        }


        if (Time.time > timer)
        {
            timer = Time.time + TimeToSpawn;

            int random = Random.Range(0, ObjectPrefabs.Count);

            var block = Instantiate(ObjectPrefabs[random], transform.position, transform.rotation);
            block.GetComponent<objectScript>().player = player;
        }

        if(player.dead == true)
        {
            spawner.SetActive(false);
        }

    }
}
