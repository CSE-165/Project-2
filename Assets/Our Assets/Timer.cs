using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{

    public TMP_Text TimerText;
    private float countDown;
    private float timer; 
    private string display;
    private float roundedValue;
    // Start is called before the first frame update
    void Start()
    {
        countDown = 10.0f;
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(countDown <= 0)
        {
            timer = timer + Time.deltaTime;
            roundedValue = (float)Math.Round(timer, 2);
        } 
        else 
        {
            countDown = countDown - Time.deltaTime;
            roundedValue = (float)Math.Round(countDown, 2);
        }

        display = roundedValue.ToString();
        TimerText.text = display;
        
    }
}
