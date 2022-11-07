using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectScript : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        if(transform.position.y >= 8.55)
        {
             Destroy(gameObject);
        }
    }
}
