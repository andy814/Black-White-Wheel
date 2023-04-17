using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndMenu : MonoBehaviour
{
    public TextMeshProUGUI timeSurvivedText;
    private float timeSurvived;

    private void Awake()
    {
        Cursor.visible = true;
    }

    void Start()
    {
        // Retrieve the time survived value from PlayerPrefs
        int historyCount = PlayerPrefs.GetInt("historyCount", 0);
        timeSurvived = PlayerPrefs.GetFloat($"surviveTimeHistory{historyCount}", 0);

        // Update the timeSurvivedText with the actual timeSurvived value
        //timeSurvivedText.text = $"{timeSurvived}";
        timeSurvivedText.text = timeSurvived.ToString("0.00");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game"); // Replace "GameScene" with the name of your main game scene
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the name of your main menu scene
    }
}
