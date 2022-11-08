using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CycleObject : MonoBehaviour
{
    //[SerializeField] private PlayerControl control;
    [SerializeField] private GameObject dayBlockOn; //obj1
    [SerializeField] private GameObject dayBlockOff; //obj2
    [SerializeField] private PlayerControl player;

    private void OnTriggerStay2D(Collider2D collision)
    {
         if (collision.tag == "DayBlock")
         {
             Debug.Log("aaa");

         }
    }

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
