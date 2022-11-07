using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    SpriteRenderer playerSprite;
    bool Day = true;

    private void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(Day)
            {
                playerSprite.color = Color.black;
                Day = !Day;
            }
            else
            {
                playerSprite.color = Color.white;
                Day = !Day;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Day)
        {
            if (collision.gameObject.CompareTag("Black"))
            {
                Time.timeScale = 0;
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("White"))
            {
                Time.timeScale = 0;
            }
        }
    }
}
