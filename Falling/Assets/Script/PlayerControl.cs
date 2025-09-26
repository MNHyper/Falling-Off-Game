using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

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
    [SerializeField] private TMP_Text dayScoreText;
    [SerializeField] private TMP_Text nightScoreText;
    [SerializeField] private GameObject dayAudioText;
    [SerializeField] private GameObject nightAudioText;

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

    private float Score;
    [SerializeField] GameObject m_ObstaclesSpawner;
    private bool m_OnTutorial = true;
    private bool m_TutorialJump;
    private bool m_TutorialDayNight;

    [SerializeField] Animator m_DayTutorial;
    [SerializeField] Animator m_NightTutorial;
    [SerializeField] float StartSpeed;
    [SerializeField] float SpeedGrowOverTime;
    [SerializeField] float MaxSpeed;
    public static float Speed;

    private void Awake()
    {
        Speed = StartSpeed;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void UpdateScore()
    {
        if (m_OnTutorial) return;

        Score += Time.deltaTime * Speed;

        dayScoreText.text = $":{Mathf.RoundToInt(Score)}";
        nightScoreText.text = $":{Mathf.RoundToInt(Score)}";
    }

    // Start is called before the first frame update
    private void Start()
    {
        mustPatrol = true;
        day = false;
        night = true;

        m_OnTutorial = 0 == PlayerPrefs.GetInt("Tutorial", 0);

        if (!m_OnTutorial)
        {
            m_ObstaclesSpawner.SetActive(true);
            m_TutorialDayNight = true;
            m_TutorialJump = true;
            m_DayTutorial.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            dayScoreText.gameObject.SetActive(false);
            nightScoreText.gameObject.SetActive(false);

            m_NightTutorial.gameObject.SetActive(true);
        }
    }
    [ContextMenu("Reset Tutorial")]
    public void ResetTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 0);
    }
    [ContextMenu("Reset Score")]
    public void ResetScore()
    {
        PlayerPrefs.SetInt("Score", 0);
    }
    // Update is called once per frame
    private void Update()
    {
        if (m_OnTutorial)
        {
            if (m_TutorialDayNight && m_TutorialJump)
            {
                m_OnTutorial = false;
                m_ObstaclesSpawner.SetActive(true);

                dayScoreText.gameObject.SetActive(true);
                nightScoreText.gameObject.SetActive(true);
                m_DayTutorial.transform.parent.gameObject.SetActive(false);

                if(night)
                {
                    nightScoreText.gameObject.SetActive(false);
                    dayScoreText.gameObject.SetActive(true);
                }   
                else
                {
                    dayScoreText.gameObject.SetActive(false);
                    nightScoreText.gameObject.SetActive(true);
                }
                PlayerPrefs.SetInt("Tutorial", 1);
            }
        }
        else
        {
            if (Speed < MaxSpeed)
            {
                Speed += Time.deltaTime * SpeedGrowOverTime;
            }
            else
            {
                Speed = MaxSpeed;
            }
        }

            IsGrounded();
        if (mustPatrol)
        {
            Patrol();
        }
        dayAnimator.SetBool("Grounded", grounded);
        nightAnimator.SetBool("Grounded", grounded);

        if (dead) return;

        UpdateScore();
        clickTimer -= Time.deltaTime;
        slideTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(0))
        {
            clickTimer = TimeForTap;
            slideTimer = TimeForSlide;
            mouseClickPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetKeyUp(KeyCode.Q) || Input.GetMouseButtonUp(0))
        {
            var newMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (clickTimer > 0 && Vector2.Distance(mouseClickPosition, newMousePos) < DistanceForTap)
            {
                OnDayNight();
            }
            else if (slideTimer > 0 && Vector2.Distance(mouseClickPosition, newMousePos) > DistanceForSlide)
            {
                if (IsDirectionUpward(mouseClickPosition, newMousePos))
                    OnJump();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
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
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (night == false)
        {
            audioManager.PlaySFX(audioManager.day_night);

            if (!m_OnTutorial)
            {
                dayScoreText.gameObject.SetActive(true);
                nightScoreText.gameObject.SetActive(false);
            }
            dayAudioText.SetActive(true);
            dayPlayer.SetActive(true);
            dayBackRound.SetActive(true);
            nightPlayer.SetActive(false);
            nightBackRound.SetActive(false);
            nightAudioText.SetActive(false);

            m_DayTutorial.gameObject.SetActive(false);
            m_NightTutorial.gameObject.SetActive(true);


            StartCoroutine(DayTime());
        }

        if (day == false)
        {
            if (!m_OnTutorial)
            {
                dayScoreText.gameObject.SetActive(false);
                nightScoreText.gameObject.SetActive(true);
            }
            audioManager.PlaySFX(audioManager.day_night);
            dayPlayer.SetActive(false);
            dayBackRound.SetActive(false);
            nightAudioText.SetActive(true);
            nightPlayer.SetActive(true);
            nightBackRound.SetActive(true);
            dayAudioText.SetActive(false);
            m_DayTutorial.gameObject.SetActive(true);
            m_NightTutorial.gameObject.SetActive(false);

            StartCoroutine(NightTime());
        }

        m_TutorialDayNight = true;
        if (m_DayTutorial.gameObject.activeSelf)
        {
            m_DayTutorial.SetBool("Jump", true);
        }
        if (m_NightTutorial.gameObject.activeSelf)
        {
            m_NightTutorial.SetBool("Jump", true);
        }

    }

    private void IsGrounded()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private void OnJump()
    {
        if (!m_TutorialDayNight) return;

        if (grounded)
        {
            doubleJump = false;
        }
        if (doubleJump) return;

        audioManager.PlaySFX(audioManager.jump);
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
            m_TutorialJump = true;
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
            audioManager.StopMusic();
            var score = PlayerPrefs.GetInt("Score", 0);
            if (Score > score)
            {
                PlayerPrefs.SetInt("Score", Mathf.RoundToInt(Score));
            }
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
