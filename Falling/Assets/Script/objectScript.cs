using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectScript : MonoBehaviour
{
    public PlayerControl player;
    [SerializeField] GameObject dayTime;
    [SerializeField] GameObject nightTime;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSppeed;
    private float speed;

    private void Update()
    {
        speed = Random.Range(minSpeed, maxSppeed);
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

public enum SpeedBoost
{
    Easy,
    Normal,
    Hard,
    Extrime,
}