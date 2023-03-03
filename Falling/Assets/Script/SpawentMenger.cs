using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawentMenger : MonoBehaviour
{
    public GameObject[] spawners;
    public float spawneTimeMin;
    public float spawneTimeMax;
    private float spawneTime;

    // Update is called once per frame
    void Update()
    {
        spawneTime = Random.Range(spawneTimeMin, spawneTimeMax);
        if (spawneTime < Time.time)
        {
            ActiveSpawners(spawners);
            this.enabled = false;
        }
    }

    public void ActiveSpawners(GameObject[] arr)
    {
        foreach (var item in arr)
        {
            item.SetActive(true);
        }
    }

}
