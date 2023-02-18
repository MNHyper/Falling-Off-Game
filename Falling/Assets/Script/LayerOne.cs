using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerOne : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;

    [Header("Day Night Variables")]
    public PlayerControl player;
    [SerializeField] GameObject dayTime;
    [SerializeField] GameObject nightTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= 15)
        {
            Destroy(gameObject);
        }

        transform.position += transform.up * speed * Time.deltaTime;
        rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        Cycle();
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
