using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : SingletonManager<PlayerManager>
{
    //private Player player;
    private bool handlingPlayerHit;

    protected override void Awake()
    {
        
        base.Awake();
        handlingPlayerHit = false;
    }

    public void HandlePlayerHit(Player player)
    {
        if (!handlingPlayerHit)
        {
            handlingPlayerHit = true;
            StartCoroutine(HandlePlayerHitCoroutine(player));
        }
    }

    private IEnumerator HandlePlayerHitCoroutine(Player player)
    {
        //PlayerPrefs.DeleteAll();
        SaveToHistory(player.SurviveTime);
        Time.timeScale = 0f;
        AudioManager.Instance.AudioSource.Pause();
        yield return StartCoroutine(player.Disappear(1));
        Time.timeScale = 1f;

        List<float> list = GetHistory();
        /*
        foreach (float element in list)
        {
            //break;
            Debug.Log(element);
        }
        //Debug.Log(list.Count);
        */
        SceneManager.LoadScene("EndMenu");
    }

    private void SaveToHistory(float surviveTime)
    {
        int historyCount = PlayerPrefs.GetInt("historyCount", 0);
        historyCount++;

        PlayerPrefs.SetFloat($"surviveTimeHistory{historyCount}", surviveTime);

        DateTime currentTime = DateTime.Now;
        int currentTimestamp = (int)(currentTime - new DateTime(1970, 1, 1)).TotalSeconds;
        PlayerPrefs.SetInt($"timestamp{historyCount}", currentTimestamp);

        PlayerPrefs.SetInt("historyCount", historyCount);
        //Debug.Log("historycount2:" + PlayerPrefs.GetInt("historyCount", 0));

        PlayerPrefs.Save();
    }

    private List<float> GetHistory()
    {
        List<float> surviveTimeHistory = new List<float>();
        int historyCount = PlayerPrefs.GetInt("historyCount", 0);

        for (int i = 1; i <= historyCount; i++)
        {
            float surviveTime = PlayerPrefs.GetFloat($"surviveTimeHistory{i}", 0f);
            surviveTimeHistory.Add(surviveTime);
        }

        return surviveTimeHistory;
    }
}
