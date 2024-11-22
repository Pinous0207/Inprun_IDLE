using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
public class ADS_Mng
{
    bool TestMode = true;
    public readonly string reward_Android_id = "ca-app-pub-9979325002286592/7716944818";
    public readonly string reward_Android_sample = "ca-app-pub-3940256099942544/5224354917";

    RewardedAd _rewardedAd; // 보상형 광고
    AdRequest _adRequest;
    Action _rewardedCallback;

    public void Init()
    {
        MobileAds.Initialize(initStatus => { });
        PrepareADS();
    }

    private void PrepareADS()
    {
        string reward;
        if(TestMode)
        {
            reward = reward_Android_sample;
        }
        else
        {
            reward = reward_Android_id;
        }

        _adRequest = new AdRequest();
        _adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(reward, _adRequest, OnAdRewardCallback);
    }

    private void OnAdRewardCallback(RewardedAd ad, LoadAdError error)
    {
        if(error != null || ad == null)
        {
            Debug.LogError("보상형 광고 준비에 실패하였습니다 : " + error);
            return;
        }

        Debug.Log("보상형 광고 준비에 성공하였습니다 : " + ad.GetResponseInfo());
        _rewardedAd = ad;
        RegisterEventHandlers(_rewardedAd); // 실행점X
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("보상형 광고가 닫혔습니다.");
            PrepareADS();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("보상형 광고 시청에 실패하였습니다.");
            PrepareADS();
        };

        ad.OnAdPaid += (AdValue adValue) =>
        {
            if (_rewardedCallback != null)
            {
                _rewardedCallback?.Invoke();
                _rewardedCallback = null;
            }
        };
    }

    public void ShowRewardedAds(Action rewardCallback)
    {
        _rewardedCallback = rewardCallback;
        
        Data_Mng.m_Data.ADS++;

        if(Data_Mng.m_Data.ADS_Remove)
        {
            if (_rewardedCallback != null)
            {
                _rewardedCallback?.Invoke();
                _rewardedCallback = null;
            }
            return;
        }

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(reward.Type + " : " + reward.Amount);

                if (_rewardedCallback != null)
                {
                    _rewardedCallback?.Invoke();
                    _rewardedCallback = null;
                }
            });
        }
        else
        {
            PrepareADS();
        }
    }
}
