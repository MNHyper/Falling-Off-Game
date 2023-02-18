using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBR : MonoBehaviour
{
    [SerializeField] private PlayerControl player;
    [SerializeField] private float TimeToSpawn;
    float timer;
    public List<GameObject> ObjectPrefabs = new List<GameObject>();
    void Update()
    {
        if (Time.time > timer)
        {
            timer = Time.time + TimeToSpawn;

            int random = Random.Range(0, ObjectPrefabs.Count);

            var block = Instantiate(ObjectPrefabs[random], transform.position, transform.rotation);
            block.GetComponent<LayerOne>().player = player;
        }
    }
}

