using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class RequestController : MonoBehaviour
{
  public TextMeshProUGUI scoreText;
  public GameObject ScoreField, ScorePanel;
  private int counter = 1;
  private bool isCurrent = false;
  [SerializeField]
  private VariablesList _variablesList;
  class Data
  {
    public string mirror_id;
    public int game_id;
    public int score;
    public string snap_name;
    public string snap_revision;
  }

  class ResponseSquatGET
  {
    public string description;
    public int score;
    public int date;
  }

  public class CustomCertificateHandler : CertificateHandler
  {
    protected override bool ValidateCertificate(byte[] certificateData)
    {
      return true;
    }
  }

  IEnumerator GetRequest(string uri, Action<string> callback = null)
  {
    System.Net.ServicePointManager.ServerCertificateValidationCallback +=
      (sender, certificate, chain, sslPolicyErrors) => true;
    UnityWebRequest uwr = UnityWebRequest.Get(uri);
    uwr.SetRequestHeader("Content-Type", "application/json");
    uwr.SetRequestHeader("Authorization", $"Bearer {_variablesList.token}");
    uwr.certificateHandler = new CustomCertificateHandler();
    yield return uwr.SendWebRequest();

    if (uwr.result == UnityWebRequest.Result.ConnectionError)
    {
      Debug.Log("Error While Sending: " + uwr.error);
    }
    else
    {
      callback?.Invoke(uwr.downloadHandler.text);
      yield return callback;
    }
  }

  IEnumerator PostRequest(string uri, Data data, Action<string> callback = null)
  {
    System.Net.ServicePointManager.ServerCertificateValidationCallback +=
      (sender, certificate, chain, sslPolicyErrors) => true;
    string parsedData = JsonConvert.SerializeObject(data);
    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(parsedData);

    UnityWebRequest uwr = UnityWebRequest.Post(uri, parsedData);
    uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
    uwr.downloadHandler = new DownloadHandlerBuffer();
    uwr.SetRequestHeader("Content-Type", "application/json");
    uwr.SetRequestHeader("Authorization", $"Bearer {_variablesList.token}");
    uwr.certificateHandler = new CustomCertificateHandler();
    yield return uwr.SendWebRequest();

    if (uwr.result == UnityWebRequest.Result.ConnectionError)
    {
      Debug.Log("Error While Sending: " + uwr.error);
    }
    else
    {
      callback?.Invoke(uwr.downloadHandler.text);
      yield return callback;
    }
  }

  private void CallBack<T>(out T value, string json)
  {
    value = JsonConvert.DeserializeObject<T>(json);
  }

  IEnumerator SendRequest()
  {
    List<ResponseSquatGET> responseList = new();

    bool response = false;

    int gameId = 1;

    yield return StartCoroutine(GetRequest($"{_variablesList.baseApiURL}ranking/results/{_variablesList.token}/{gameId}",
      json => CallBack(out responseList, json)));

    var current = responseList.Take(7);
    Debug.Log(current);
    var curRes = new ResponseSquatGET
    {
      score = int.Parse(scoreText.text),
      description = "test",
      date = 0
    };

    foreach (var curr in current)
    {
      var temp = Instantiate(ScoreField, ScorePanel.transform);
      temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = counter.ToString();
      if (!isCurrent && curRes.score >= curr.score)
      {
        temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = curRes.score.ToString();
        temp.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "あなた";
        isCurrent = true;
      }
      else
      {
        temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = curr.score.ToString();
        temp.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = curr.description;
      }

      counter++;
    }

    if (!isCurrent)
    {
      var temp = Instantiate(ScoreField, ScorePanel.transform);
      temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = counter.ToString();
      temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = curRes.score.ToString();
      temp.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "あなた";
      isCurrent = true;
    }

    Debug.Log($"Get completed");

    Data data = new();
    data.game_id = gameId;
    data.mirror_id = VariablesList.MirrorID;
    data.score = int.Parse(scoreText.text);
    data.snap_name = "mirror-x";
    data.snap_revision = "0";
    yield return StartCoroutine(PostRequest($"{_variablesList.baseApiURL}ranking/results/{_variablesList.token}", data));

    Debug.Log($"POST completed");
  }

  void Start()
  {
    if (Directory.Exists(VariablesList.ConfigurationFolder)&& File.Exists($"{VariablesList.ConfigurationFolder}/{VariablesList.ConfigurationFile}"))
    {
      StreamReader reader = new StreamReader($"{VariablesList.ConfigurationFolder}/{VariablesList.ConfigurationFile}");
      string jsonResult = reader.ReadToEnd();
      VariablesList.Root myDeserializedClass = JsonConvert.DeserializeObject<VariablesList.Root>(jsonResult);
      if (myDeserializedClass == null) return;
      _variablesList.baseApiURL = myDeserializedClass.config.First().MIRROR_CONSOLE_API_URL;
      _variablesList.token = myDeserializedClass.config.First().MIRROR_CONSOLE_API_HEADER_TOKEN;
      reader.Close();
    }
    StartCoroutine(SendRequest());
  }
}
