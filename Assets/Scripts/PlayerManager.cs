using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : SingletonManager<PlayerManager>
{
    //private Player player;

    protected override void Awake()
    {
        base.Awake();
    }

    public void HandlePlayerHit(Player player)
    {
        StartCoroutine(HandlePlayerHitCoroutine(player));
    }

    private IEnumerator HandlePlayerHitCoroutine(Player player)
    {
        PlayerPrefs.SetFloat("surviveTime", player.SurviveTime);
        PlayerPrefs.Save();
        Time.timeScale = 0f;
        AudioManager.Instance.AudioSource.Pause();
        yield return StartCoroutine(player.Disappear(1));
        Time.timeScale = 1f;
        SceneManager.LoadScene("EndMenu");
    }
}
