using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorpent : MonoBehaviour
{
    public PlayerControl player;
    [SerializeField] GameObject dayTime;
    [SerializeField] GameObject nightTime;
    public float speed;

    private void Start()
    {

    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        Cycle();
        if (transform.position.y >= 8.55)
        {
            Destroy(gameObject);
        }
    }

    private void Cycle()
    {
        if (player.night == false)
        {
            dayTime.SetActive(true);
            nightTime.SetActive(false);
        }

        if (player.day == false)
        {
            dayTime.SetActive(false);
            nightTime.SetActive(true);
        }

    }
}

