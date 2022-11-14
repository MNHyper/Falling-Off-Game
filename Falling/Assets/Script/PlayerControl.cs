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
    private bool mustPatrol;
    private bool mustTurn;

    [Header("Jump Variables")]
    [SerializeField] private float jumpPower;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    private bool jumping;
    private bool doubleJump;
    private bool grounded;

    [Header("Day Night Variables")]
    [SerializeField] private GameObject dayPlayer;
    [SerializeField] private GameObject nightPlayer;
    [SerializeField] private GameObject dayBackRound;
    [SerializeField] private GameObject nightBackRound;
    [SerializeField] private float cycleTime;
    private bool dayCycle;
    public bool day;
    public bool night;


    // Start is called before the first frame update
    private void Start()
    {
        mustPatrol = true;
        day = false;
        night = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (mustPatrol)
        {
            Patrol();
        }

    }

    private void FixedUpdate()
    {
        IsGrounded();

        if (mustPatrol)
        {
            mustTurn = Physics2D.OverlapCircle(wallCheckPos.position, 0.1f, wallLayer);
        }
    }

    private void OnDayNight(InputValue value)
    {
        dayCycle = value.isPressed;

        if (dayCycle && night == false)
        {
            dayPlayer.SetActive(true);
            dayBackRound.SetActive(true);
            nightPlayer.SetActive(false);
            nightBackRound.SetActive(false);

            StartCoroutine(DayTime());
        }

        if (dayCycle && day == false)
        {
            dayPlayer.SetActive(false);
            dayBackRound.SetActive(false);
            nightPlayer.SetActive(true);
            nightBackRound.SetActive(true);

            StartCoroutine(NightTime());
        }
    }

    private void IsGrounded()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnJump(InputValue value)
    {
        jumping = value.isPressed;

        if (grounded && !jumping)
        {
            doubleJump = false;
        }

        jumping = value.isPressed;

        if (jumping)
        {
            if(grounded || doubleJump)
            {
                jumpBufferCounter = jumpBufferTime;

                doubleJump = !doubleJump;
            }

            //animator.SetBool("Jumping", true);
            //FindObjectOfType<AudioManager>().Play("Jump");
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
            //animator.SetBool("Jumping", false);
        }


        if (jumpBufferCounter > 0f || doubleJump)
        {
            rb.velocity += new Vector2(0f, jumpPower);

            jumpBufferCounter = 0f;
        }

        if (!jumping && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
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

    private IEnumerator DayTime()
    {
        yield return new WaitForSeconds(cycleTime);
        day = false;
        night = true;
    }

    private IEnumerator NightTime()
    {
        yield return new WaitForSeconds(cycleTime);
        day = true;
        night = false;
    }

}
