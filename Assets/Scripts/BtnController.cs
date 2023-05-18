using UnityEngine;

public class BtnController : MonoBehaviour
{
  public void ExitGame()
  {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
  }
  public void NextPage(GameObject page)
  {
    page.SetActive(true);
  }
}
