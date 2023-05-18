using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class VariablesList : MonoBehaviour
{
  public string baseApiURL = "";
  public string token = "";
  public static string MirrorID = Environment.MachineName;
  public static string UserID = Environment.UserName;
  public static string ConfigurationFolder = $"/home/{UserID}";
  public static string ConfigurationFile = "env.json";

  public class Config
  {
    public string API_TOKEN { get; set; }
    public string API_ROOT_ENDPOINT { get; set; }
    public string SOCKETIO_SERVER_URL { get; set; }
    public string PEOPLE_COUNTER_AUTH_CONTENT_TYPE { get; set; }
    public string MIRROR_API_URL { get; set; }
    public string AWS_ACCESS_TOKEN { get; set; }
    public string AWS_SECRET_KEY { get; set; }
    public string AWS_BUCKET_NAME { get; set; }
    public string AWS_REGIONAL { get; set; }
    public string AFLAC_FUJI_API_URL { get; set; }
    public string FACEPLUSPLUS_BASE_URL { get; set; }
    public string GOOGLECHART_BASE_URL { get; set; }
    public string SUKOYAKA_BASE_URL { get; set; }
    public string AGING_BASE_URL { get; set; }
    public string MIRROR_CONSOLE_API_HEADER_TOKEN { get; set; }
    public string MIRROR_CONSOLE_API_URL { get; set; }
  }

  public class Root
  {
    public string env { get; set; }
    public List<Config> config { get; set; }
  }
}
