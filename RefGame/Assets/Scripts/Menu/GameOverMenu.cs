﻿
#if HMS_BUILD
using HmsPlugin;
#endif
using HuaweiMobileServices.Ads;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverMenu : SimpleMenu<GameOverMenu>
{
    [SerializeField]
    private TextMeshProUGUI bestScoreText;
    [SerializeField]
    private GameObject continueButton;

    private int continueClickCount;

#if HMS_BUILD
    private RewardAdManager rewardAdManager;
#endif
    private bool rewarded = false;

    private void Start()
    {
        transform.SetAsFirstSibling();

#if HMS_BUILD
        rewardAdManager = RewardAdManager.GetInstance();
        rewardAdManager.OnRewarded = OnAddRewarded;
#endif
    }

    private void OnAddRewarded(Reward obj)
    {
        rewarded = true;
        Debug.Log("rewarded");
    }

    private void OnEnable()
    {
        bestScoreText.text = $"Best Score: {PlayerPrefs.GetInt(Const.PREF_BEST_SCORE)}";
        continueButton.SetActive(continueClickCount < 1);
    }

    public void OnContinueButtonClick()
    {
        continueClickCount++;

#if GMS_BUILD
        GoogleAdMobController.Instance.ShowRewardedAd();
        Hide();
#endif

#if HMS_BUILD
        rewardAdManager.ShowRewardedAd();
#endif
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("Pause : " + pause);
        if (!pause)
        {
            if (rewarded)
            {
                Debug.Log("Rewarding user");
                PlayerController.Instance.RestartPlayerFromContinue();
                Hide();
                rewarded = false;
            }
        }
    }

    public void OnTryAgainClick()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
