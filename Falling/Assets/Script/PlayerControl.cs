using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    AudioManager audioManager;
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
    [SerializeField] private float forwordPower;
    private float jumpBufferTime = 0.3f;
    private float jumpBufferCounter;
    private bool jumping;
    private bool doubleJump;
    private bool grounded;

    [Header("Day Night Variables")]
    [SerializeField] private GameObject dayPlayer;
    [SerializeField] private GameObject nightPlayer;
    [SerializeField] private GameObject dayBackRound;
    [SerializeField] private GameObject nightBackRound;
    [SerializeField] private GameObject dayTimer;
    [SerializeField] private GameObject NightTimer;
    [SerializeField] private GameObject dayJumpB;
    [SerializeField] private GameObject NightJumpB;
    [SerializeField] private GameObject dayCycleB;
    [SerializeField] private GameObject NightCycleB;
    [SerializeField] private float cycleTime;
    private bool dayCycle;
    public bool day;
    public bool night;


    [Header("Animator")]
    [SerializeField] private Animator dayAnimator;
    [SerializeField] private Animator nightAnimator;

    [Header("Dead")]
    [SerializeField] Shake shake;
    [SerializeField] float deaingTime;
    [SerializeField] private GameObject deathScroomDay;
    [SerializeField] private GameObject deathScroomNight;
    public bool dead = false;
    private Vector2 mouseClickPosition;
    public float TimeForTap;
    public float TimeForSlide;
    public float DistanceForTap;
    public float DistanceForSlide;
    private float clickTimer;
    private float slideTimer;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

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
        IsGrounded();
        if (mustPatrol)
        {
            Patrol();
        }
        dayAnimator.SetBool("Grounded", grounded);
        nightAnimator.SetBool("Grounded", grounded);

        if (dead) return;

        clickTimer -= Time.deltaTime;
        slideTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(0))
        {
            clickTimer = TimeForTap;
            slideTimer = TimeForSlide;
            mouseClickPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if(Input.GetKeyUp(KeyCode.Q) || Input.GetMouseButtonUp(0))
        {
            var newMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (clickTimer > 0 && Vector2.Distance(mouseClickPosition, newMousePos) < DistanceForTap)
            {
                OnDayNight();
            }
            else if (slideTimer > 0 && Vector2.Distance(mouseClickPosition, newMousePos) > DistanceForSlide)
            {
                if(IsDirectionUpward(mouseClickPosition, newMousePos))
                OnJump();
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnJump();
        }
    }

    private void FixedUpdate()
    {
        if (mustPatrol)
        {
            mustTurn = Physics2D.OverlapCircle(wallCheckPos.position, 0.1f, wallLayer);
        }
    }

    private void OnDayNight()
    {
        if (night == false)
        {
            audioManager.PlaySFX(audioManager.day_night);
            dayPlayer.SetActive(true);
            dayBackRound.SetActive(true);
            dayTimer.SetActive(true);
            dayJumpB.SetActive(true);
            dayCycleB.SetActive(true);
            nightPlayer.SetActive(false);
            nightBackRound.SetActive(false);
            NightTimer.SetActive(false);
            NightJumpB.SetActive(false);
            NightCycleB.SetActive(false);

            StartCoroutine(DayTime());
        }

        if (day == false)
        {
            audioManager.PlaySFX(audioManager.day_night);
            dayPlayer.SetActive(false);
            dayBackRound.SetActive(false);
            dayTimer.SetActive(false);
            dayJumpB.SetActive(false);
            dayCycleB.SetActive(false);
            nightPlayer.SetActive(true);
            nightBackRound.SetActive(true);
            NightTimer.SetActive(true);
            NightJumpB.SetActive(true);
            NightCycleB.SetActive(true);

            StartCoroutine(NightTime());
        }
    }

    private void IsGrounded()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private void OnJump()
    {
        audioManager.PlaySFX(audioManager.jump);
        if (grounded)
        {
            doubleJump = false;
        }
        if (doubleJump) return;

        if (grounded)
        {
            dayAnimator.SetTrigger("Jump");
            nightAnimator.SetTrigger("Jump");
        }
        else
        {
            dayAnimator.Play("DC Doble Jump");
            nightAnimator.Play("NC Doble Jump");
            doubleJump = true;
        }


        rb.velocity += new Vector2(forwordPower, jumpPower);


        //animator.SetBool("Jumping", true);
        //FindObjectOfType<AudioManager>().Play("Jump");


        /*   jumpBufferCounter -= Time.deltaTime;



           if (jumpBufferCounter > 0f || doubleJump)
           {
               rb.velocity += new Vector2(forwordPower, jumpPower);

               jumpBufferCounter = 0f;
           }

           if (!jumping && rb.velocity.y > 0f)
           {
               rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
           }*/

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Dead")
        {
            Time.timeScale = deaingTime;
            shake.start = true;
            dayAnimator.SetBool("Deaing", true);
            nightAnimator.SetBool("Deaing", true);
            moveSpeed = 0f;
            dayCycle = false;
            jumping = false;
            dead = true;
            StartCoroutine(DeaingTimer());
            audioManager.PlaySFX(audioManager.death);
        }
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

    private IEnumerator DeaingTimer()
    {
        yield return new WaitForSeconds(deaingTime);
        if (night == false)
        {
            deathScroomDay.SetActive(true);
            dayAnimator.SetBool("Deaing", false);
            nightAnimator.SetBool("Deaing", false);
        }

        if (day == false)
        {
            deathScroomNight.SetActive(true);
            dayAnimator.SetBool("Deaing", false);
            nightAnimator.SetBool("Deaing", false);
        }
    }
    public bool IsDirectionUpward(Vector2 from, Vector2 to, float offsetAngle = 45f)
    {
        Vector2 direction = (to - from).normalized;
        float angleToUp = Vector2.Angle(direction, Vector2.up);

        return angleToUp <= offsetAngle;
    }

}
