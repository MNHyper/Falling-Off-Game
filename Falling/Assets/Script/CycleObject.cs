using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CycleObject : MonoBehaviour
{
    //[SerializeField] private PlayerControl control;
    [SerializeField] private GameObject dayBlockOn; //obj1
    [SerializeField] private GameObject dayBlockOff; //obj2

    [Header("Day Night Variables")]
    [SerializeField] private GameObject dayPlayer;
    [SerializeField] private GameObject nightPlayer;
    [SerializeField] private float cycleTime;
    private bool dayCycle;
    public bool day;
    public bool night;

    private void Start()
    {
        day = false;
        night = true;
    }

    private void OnDayNight(InputValue value)
    {
        dayCycle = value.isPressed;
        Debug.Log("Cycle");

        if (dayCycle && day == true)
        {
            DayCycle(dayBlockOff, dayBlockOn);

            StartCoroutine(DayTime());
        }

        if (dayCycle && night == true)
        {
            NightCycle(dayBlockOn, dayBlockOff);

            StartCoroutine(NightTime());
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

    /*
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "DayBlock")
            {
                Debug.Log("aaa");

            }
        }
    */
    private void NightCycle(GameObject obj1, GameObject obj2)
    {
        obj1.SetActive(false);
        obj2.SetActive(true);
    }

    private void DayCycle(GameObject obj1, GameObject obj2)
    {
        obj1.SetActive(false);
        obj2.SetActive(true);
    }

}
