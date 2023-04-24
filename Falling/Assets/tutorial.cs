using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour
{

    //[SerializeField] private Animator animator;
    [SerializeField] private PlayerControl player;
    private bool one = false;
    private bool two = false;
    private bool three = false;
    private bool end = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            Debug.Log("one");

        }
    }
}
