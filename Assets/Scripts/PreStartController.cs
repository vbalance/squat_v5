using System.Collections;
using TMPro;
using UnityEngine;

public class PreStartController : MonoBehaviour
{
  [SerializeField] private GameObject textPanel,countDownPanel;
  [SerializeField] private TextMeshProUGUI text;
  [SerializeField] private Timer timer;
    void Start()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
      yield return new WaitForSeconds(2);
      textPanel.SetActive(false);
      countDownPanel.SetActive(true);
      text.text = "3";
      yield return new WaitForSeconds(1);
      text.text = "2";
      yield return new WaitForSeconds(1);
      text.text = "1";
      yield return new WaitForSeconds(1);
      text.text = "GO!";
      yield return new WaitForSeconds(1);
      timer.EnableTimer();
      Destroy(gameObject);
    }
}
