using System.Collections;
using TMPro;
using UnityEngine;
public class questController : MonoBehaviour
{
  [SerializeField] private GameObject QuizPanel;
  [SerializeField] private TextMeshProUGUI text;
  void Start()
    {
      StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
      yield return new WaitForSeconds(1);
      text.text = "5";
      yield return new WaitForSeconds(1);
      text.text = "4";
      yield return new WaitForSeconds(1);
      text.text = "3";
      yield return new WaitForSeconds(1);
      text.text = "2";
      yield return new WaitForSeconds(1);
      text.text = "1";
      yield return new WaitForSeconds(1);
      QuizPanel.SetActive(true);
      Destroy(gameObject);
    }
}

