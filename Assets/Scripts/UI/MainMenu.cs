using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game"); 
    }

    public void ViewHistory()
    {
        SceneManager.LoadScene("HistoryMenu"); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
