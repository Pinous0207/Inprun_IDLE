using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Timer_NTP : MonoBehaviour
{
    private static long diffTicks = 0L;
    private static DateTime syncTime = DateTime.Now;

    public static DateTime NowTime => DateTime.Now.AddTicks(diffTicks);

    public static void UpdateDateTime(string dataStr)
    {
        if(DateTime.TryParse(dataStr, out DateTime serverTime))
        {
            syncTime = serverTime;
            diffTicks = (syncTime - DateTime.Now).Ticks;
        }
    }
}

public class Time_Mng
{
    public float currentTime;
    public TimeSpan loginSpan;
    public Action<DateTime> OnTimeUpdated;
    private static bool isCheckIn = false;

    private const string serverUrl = "https://www.google.com/";

    public void Init()
    {
        Base_Mng.instance.StartCoroutine(CheckServerTime());
    }

    public IEnumerator CheckServerTime()
    {
        using UnityWebRequest request = UnityWebRequest.Get(serverUrl);
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Network Error");
        }
        else
        {
            string serverDate = request.GetResponseHeader("Date");
            if(DateTime.TryParse(serverDate, out DateTime serverTime))
            {
                Debug.Log("Server Time : " + serverTime);
                Timer_NTP.UpdateDateTime(serverDate);
            }
            else
            {
                Debug.LogError("Invalid server date Format.");
            }
        }
    }
}
