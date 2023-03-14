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
    }

    // Update is called once per frame
    void Update()
    {
        spawneTime = Random.Range(spawneTimeMin, spawneTimeMax);
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
