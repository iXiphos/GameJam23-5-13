using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    bool scoreActive = false;
    float currentTime;
    public TMP_Text currentTimeText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    { 
        if (scoreActive == true)
        {
            currentTime = currentTime + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = "You survived for " + time.Minutes.ToString() + ":" + time.Seconds.ToString();
    }

    public void StartScore()
    {
        scoreActive = true;
    }

    public void StopScore()
    {
        scoreActive = false;
    }
}
