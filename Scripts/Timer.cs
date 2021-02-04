using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Timer : MonoBehaviour
{
    public float timeRemaining = 120f;
    public bool timerActive = false;

    public Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        timerActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // shows the current time left
        timerText.text = Mathf.RoundToInt(timeRemaining).ToString();


        // subtracts a second from the timer if the timer is still running or not 0
        if (timeRemaining > 0 && timerActive == true)
        {
            timeRemaining -= Time.deltaTime;
        }
        // Disables timer once timer is less than 0
        else
        {
            SceneManager.LoadScene("MainMenu");
            timeRemaining = 0;
            timerActive = false;
        }
    }
}
