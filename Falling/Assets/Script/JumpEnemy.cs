using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEnemy : MonoBehaviour
{
    [Header("Move Variables")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float onWallTime;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform wallCheckPos;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float topSpeed;
    [SerializeField] private float slowSpeed;
    private bool mustPatrol;
    private bool mustTurn;
    private Vector3 enemyScale;
    private float speed;

    [Header("Day Night Variables")]
    public PlayerControl player;
    [SerializeField] GameObject dayTime;
    [SerializeField] GameObject nightTime;

    // Start is called before the first frame update
    void Start()
    {
        mustPatrol = true;
        speed = Random.Range(topSpeed, slowSpeed);

    }

    // Update is called once per frame
    void Update()
    {     
        Cycle();
        if (transform.position.y >= 8.55)
        {
            Destroy(gameObject);
        }

        if (mustPatrol == true)
        {
            Patrol();
        }

    }

    private void FixedUpdate()
    {
        if (mustPatrol == true)
        {
            mustTurn = Physics2D.OverlapCircle(wallCheckPos.position, 0.1f, wallLayer);
        }
    }

    private void Patrol()
    {
        if (mustTurn)
        {
            Flip();

        }

        if (mustPatrol == true)
        {
            transform.position += transform.up * speed * Time.deltaTime;
            rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

    }

    private void Flip()
    {
        mustPatrol = false;
        mustTurn = !mustTurn;
        enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
        moveSpeed *= -1;
        StartCoroutine(StopMove());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckPos.position, 0.1f);
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

    private IEnumerator StopMove()
    {
        yield return new WaitForSeconds(onWallTime);
        mustPatrol = true;
    }

}
