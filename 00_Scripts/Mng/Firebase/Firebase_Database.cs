using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public partial class Firebase_Mng
{
    public void WriteData()
    {
        // JsonUtillity.ToJson -> 클래스, 구조체 -> json
        // JsonConvert.SerializeObject -> 딕셔너리 -> json

        #region DEFAULT_DATA
        Data data = new Data();
        if (Data_Mng.m_Data != null)
        {
            data = Data_Mng.m_Data;
            DateTime LastDate = DateTime.Parse(data.EndDate);
            data.EndDate = DateTime.Now.ToString();

            if(GetDateItem(LastDate, DateTime.Now))
            {
                data.Key[0] = 2;
                data.Key[1] = 2;
            }

            Debug.Log(data.EndDate + " : 종료 시간");
        }
        string Default_json = JsonUtility.ToJson(data);
        reference.Child("USER").Child(currentUser.UserId).Child("DATA").SetRawJsonValueAsync(Default_json).ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("데이터 쓰기 실패 : " + task.Exception.ToString());
            }
        });
        #endregion

        #region CHARCTER_DATA
        string Character_json = JsonConvert.SerializeObject(Base_Mng.Data.Character_Holder);
        reference.Child("USER").Child(currentUser.UserId).Child("CHARACTER").SetRawJsonValueAsync(Character_json).ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("데이터 쓰기 실패 : " + task.Exception.ToString());
            }
        });
        #endregion

        #region ITEM_DATA
        string Item_json = JsonConvert.SerializeObject(Base_Mng.Data.Item_Holder);
        reference.Child("USER").Child(currentUser.UserId).Child("ITEM").SetRawJsonValueAsync(Item_json).ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("데이터 쓰기 실패 : " + task.Exception.ToString());
            }
        });
        #endregion

        #region SMELT_DATA
        string smelt_json = JsonConvert.SerializeObject(Base_Mng.Data.m_Data_Smelt);
        reference.Child("USER").Child(currentUser.UserId).Child("SMELT").SetRawJsonValueAsync(smelt_json).ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("데이터 쓰기 실패 : " + task.Exception.ToString());
            }
        });
        #endregion
    }

    public void ReadData()
    {
        #region DEFAULT_DATA
        reference.Child("USER").Child(currentUser.UserId).Child("DATA").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result.Exists)
            {
                DataSnapshot snapshot = task.Result;

                var default_data = JsonUtility.FromJson<Data>(snapshot.GetRawJsonValue());
                Data data = new Data();
                if(default_data != null)
                {
                    data = default_data;
                }
                data.StartDate = DateTime.Now.ToString();
                Debug.Log(data.StartDate + " : 시작 시간");

                DateTime startDate = DateTime.Parse(data.StartDate);
                DateTime endDate = DateTime.Parse(data.EndDate);

                if(GetDateItem(startDate, endDate))
                {
                    data.Key[0] = 2;
                    data.Key[1] = 2;
                }

                Data_Mng.m_Data = data;
                LoadingScene.instance.LoadingMain();
            }
            else
            {
                Debug.LogError("데이터 읽기 실패: " + task.Exception.ToString());
            }
        });
        #endregion

        #region CHARACTER_DATA
        reference.Child("USER").Child(currentUser.UserId).Child("CHARACTER").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                var data = JsonConvert.DeserializeObject<Dictionary<string, Holder>>(snapshot.GetRawJsonValue());
                Base_Mng.Data.Character_Holder = data;

                Base_Mng.Data.Init();
            }
            else
            {
                Debug.LogError("데이터 읽기 실패: " + task.Exception.ToString());
            }
        });
        #endregion

        #region ITEM_DATA
        reference.Child("USER").Child(currentUser.UserId).Child("ITEM").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                var data = JsonConvert.DeserializeObject<Dictionary<string, Holder>>(snapshot.GetRawJsonValue());
                Base_Mng.Data.Item_Holder = data;

                Base_Mng.Data.Init();
            }
            else
            {
                Debug.LogError("데이터 읽기 실패: " + task.Exception.ToString());
            }
        });
        #endregion

        #region SMELT_DATA
        reference.Child("USER").Child(currentUser.UserId).Child("SMELT").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                var data = JsonConvert.DeserializeObject<List<Smelt_Holder>>(snapshot.GetRawJsonValue());
                Base_Mng.Data.m_Data_Smelt = data;

                foreach(var dd in data)
                {
                    Debug.Log(dd.Value);
                }
            }
            else
            {
                Debug.LogError("데이터 읽기 실패: " + task.Exception.ToString());
            }
        });
        #endregion

    }

    private bool GetDateItem(DateTime startTime, DateTime endTime)
    {
        if(startTime.Day != endTime.Day)
        {
            return true;
        }
        return false;
    }
}
