using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;

    [Header("Timer Setting")]
    public float currentTime;
    public bool countDown;

    [Header("Format Setting")]
    public bool hasFotnat;
    public TimerFormate format;
    private Dictionary<TimerFormate, string> timeFormats = new Dictionary<TimerFormate, string>();

    // Start is called before the first frame update
    void Start()
    {
        timeFormats.Add(TimerFormate.Whole, "0");
        timeFormats.Add(TimerFormate.TenthDecimal, "0.0");
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;

        timerText.text = currentTime.ToString(timeFormats[format]+"M");
    }
}

public enum TimerFormate
{
    Whole,
    TenthDecimal,
}