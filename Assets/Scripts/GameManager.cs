using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  
  [SerializeField] private float rightHipY, leftHipY;
  [SerializeField] private bool canSquat = true, canOveride = true;
  [SerializeField] private TextMeshProUGUI text;
  [SerializeField] private Animator duckAnimator, pumpAnimator;
  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private float squatPos;
  [SerializeField] private Slider slider;
  private static readonly int _NextStep = Animator.StringToHash("NextStep");
  private static readonly int SquatTrigger = Animator.StringToHash("SquatTrigger");
  private Timer _timer;
  private float timeRemaining = 3f;


  private void Start()
  {
    _timer = gameObject.GetComponent<Timer>();
  }

  public void SetHipPos(float yPos, string hip)
  {
    if (!_timer.GetTimerState()) return;
    switch (hip)
    {
      case "left":
        leftHipY = yPos;
        OverridePositions(leftHipY);
        CheckSquat(leftHipY);
        break;
      case "right":
        rightHipY = yPos;
        OverridePositions(rightHipY);
        CheckSquat(rightHipY);
        break;
    }
  }

  private void CheckSquat(float hip)
  {
    if (hip >= (squatPos * 2f) && canSquat)
    {
      slider.value = hip / (squatPos * 2f);
      duckAnimator.SetTrigger(SquatTrigger);
      pumpAnimator.SetTrigger(_NextStep);
      canSquat = false;
      text.text = "立って！";
      scoreText.text = (int.Parse(scoreText.text) + 1).ToString();
      if (int.Parse(scoreText.text) == 5)
        gameObject.GetComponent<AudioManager>().Play("target");
      timeRemaining = 3f;
    }
    else if (hip >= (squatPos * 1.75f) && hip <= (squatPos * 1.85f))
    {
      if (hip >= (squatPos * 1.75f))
        slider.value = hip / (squatPos * 3.5f);
      if (hip >= (squatPos * 1.76f))
        slider.value = hip / (squatPos * 3.2f);
      if (hip >= (squatPos * 1.78f))
        slider.value = hip / (squatPos * 2.8f);
      if (hip >= (squatPos * 1.8f))
        slider.value = hip / (squatPos * 2.4f);
      text.text = !canSquat ? "しゃがんで！" : "もっとしゃがんで！";
    }
    else if (hip >= (squatPos * 1.3f) && hip <= (squatPos * 1.75f))
    {
      if (hip >= (squatPos * 1.3f))
        slider.value = 0;
      if (hip >= (squatPos * 1.6f))
        slider.value = hip / (squatPos * 5.5f);
      if (hip >= (squatPos * 1.7f))
        slider.value = hip / (squatPos * 5f);
      if (hip >= (squatPos * 1.75f))
        slider.value = hip / (squatPos * 4.5f);
      canSquat = true;
      text.text = "しゃがんで！";
    }
  }

  private void OverridePositions(float hipPos)
  {
    if (text.text == "立って！" || !canOveride) return;
    squatPos = hipPos >= hipPos / 1.5f ? hipPos / 1.5f : hipPos;
    canOveride = false;
  }

  private void Update()
  {
    if (!_timer.GetTimerState()) return;
    if (!(timeRemaining > 0))
    {
      canOveride = true;
      timeRemaining = 3f;
    }
    else
    {
      timeRemaining -= Time.deltaTime;
    }
  }
}
