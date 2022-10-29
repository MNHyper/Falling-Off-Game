using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Move Variables")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform wallCheckPos;
    [SerializeField] private LayerMask wallLayer;

    [Header("Jump Variables")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float fullingSpeed;
    [SerializeField] private float jumpTimer;


    private bool mustPatrol;
    private bool mustTurn;
    private bool grounded;
    private bool jumping;
    private bool jumpPoint = false;


    // Start is called before the first frame update
    private void Start()
    {
        mustPatrol = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (mustPatrol)
        {
            Patrol();
        }

        JumpDown();
    }

    private void FixedUpdate()
    {
        IsGrounded();

        if (mustPatrol)
        {
            mustTurn = Physics2D.OverlapCircle(wallCheckPos.position, 0.1f, wallLayer);
        }
    }

    private void IsGrounded()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnJump(InputValue value)
    {
        jumping = value.isPressed;

        if (grounded && jumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpPoint = false;
            StartCoroutine(StopJump());
        }
    }

    private void JumpDown()
    {
        if (grounded && jumpPoint == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            jumpPoint = false;
        }
    }

    private void Patrol()
    {
        if (mustTurn)
        {
            Flip();
        }

        rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    private void Flip()
    {
        mustPatrol = false;
        mustTurn = !mustTurn;
        Vector3 PlayerScale = transform.localScale;
        PlayerScale.x *= -1;
        transform.localScale = PlayerScale;
        moveSpeed *= -1;
        mustPatrol = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckPos.position, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }

    private IEnumerator StopJump()
    {
        yield return new WaitForSeconds(jumpTimer);
        jumping = false;

        if (!grounded && !jumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, fullingSpeed);
            jumpPoint = true;
        }


    }

}
