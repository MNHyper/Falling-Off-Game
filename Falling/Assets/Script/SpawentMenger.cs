using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawentMenger : MonoBehaviour
{
    [SerializeField] private PlayerControl player;
    public GameObject[] spawners;
    public float spawneTimeMin;
    public float spawneTimeMax;
    private float spawneTime;

    private void Start()
    {
        spawneTime = Random.Range(spawneTimeMin, spawneTimeMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawneTime < Time.timeSinceLevelLoad)
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

    public void UnactiveSpawners(GameObject[] arr)
    {
        foreach (var item in arr)
        {
            item.SetActive(false);
        }
    }

}
