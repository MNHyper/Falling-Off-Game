using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawentMenger : MonoBehaviour
{
    public GameObject[] spawners;
    public float spawneTime;

    // Update is called once per frame
    void Update()
    {
        if(spawneTime < Time.time)
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
