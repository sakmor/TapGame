using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using System.Text;

public class GameDataSql : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(CoGetTop((d) =>
        {
            foreach (var item in d)
            {
                Debug.Log($"{item.Name} | {item.Score}");
            }
        }));
    }

    public void SendData(string name, double score)
    {
        StartCoroutine(CoSend(name, score));
    }

    IEnumerator CoSend(string name, double score)
    {
        var request = UnityWebRequest.Post(@$"http://mipipi.ddns.net/WebAPI/api/values/{name}|{score}", "");
        request.method = "POST";

        byte[] bodyRaw = Encoding.UTF8.GetBytes("GGC|9999");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        while (!request.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        if (!string.IsNullOrEmpty(request.error))
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Sned Done");
            var jsonContent = request.downloadHandler.text;
        }

        yield break;
    }

    public void GetTop5(Action<List<TapGameData>> OnGetDone)
    {
        StartCoroutine(CoGetTop(OnGetDone));
    }

    IEnumerator CoGetTop(Action<List<TapGameData>> OnGetDone)
    {
        var request = UnityWebRequest.Get("http://mipipi.ddns.net/WebAPI/api/values/");
        yield return request.SendWebRequest();

        while (!request.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        if (!string.IsNullOrEmpty(request.error))
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(MiniJSON.Json.Deserialize(request.downloadHandler.text));
            var jsonstr = MiniJSON.Json.Deserialize(request.downloadHandler.text).ToString();
            var jsonObj = MiniJSON.Json.Deserialize(jsonstr) as List<object>;
            List<TapGameData> datas = new List<TapGameData>();
            foreach (var item in jsonObj)
            {
                Dictionary<string, object> d = item as Dictionary<string, object>;

                datas.Add(new TapGameData()
                {
                    Name = d["Name"].ToString(),
                    Score = double.Parse(d["Score"].ToString())
                });

            }

            if (OnGetDone != null)
                OnGetDone(datas);
        }
    }

    [Serializable]
    public class TapGameData
    {
        public string Name { get; set; }
        public double Score { get; set; }
    }
}
