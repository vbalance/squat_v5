using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    /// <summary>
    /// How many time is remaining.
    /// </summary>
    [Tooltip("Set remaining time in sec")] [SerializeField]
    private float timeRemaining;
    
    /// <summary>
    /// How many time.
    /// </summary>
    [Tooltip("Set time in sec")] [SerializeField]
    private float time;

    /// <summary>
    /// Timer state.
    /// </summary>
    [Tooltip("Play/Pause timer")] [SerializeField]
    private bool timerIsRunning;

    /// <summary>
    /// Timer text.
    /// </summary>
    [Tooltip("UI timer text")] [SerializeField]
    private TextMeshProUGUI timeText;
    /// <summary>
    /// EndScreen.
    /// </summary>
    [SerializeField]
    private GameObject EndScreen;

    /// <summary>
    /// This function add arg value to current time.
    /// Call 'AddTime' function when you want add some time to current.
    /// </summary>
    public void AddTime(float value)
    {
        timeRemaining += value;
        DisplayTime(timeRemaining);
    }
    /// <summary>
    /// This function enables arg value to current time.
    /// Call 'EnableTimer' function when you want start timer.
    /// </summary>
    public void EnableTimer()
    {
        timerIsRunning = true;
        DisplayTime(timeRemaining);
    }
    
    public void DisableTimer()
    {
        timerIsRunning = false;
        timeRemaining = time;
        DisplayTime(timeRemaining);
    }
    public bool GetTimerState()
    {
      return timerIsRunning;
    }
    
    // Update
    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeText.color = timeRemaining < 5 ? Color.red : Color.black;
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                EndScreen.SetActive(true);
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    /// <summary>
    /// This function display current time in UI.
    /// </summary>
    private void DisplayTime(float timeToDisplay)
    {
        // timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{00}", seconds);
    }
}
