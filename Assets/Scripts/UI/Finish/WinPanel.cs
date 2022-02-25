using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : Panel
{
    protected override void OnButtonClick()
    {
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt(AmplitudeEvents.LastLevel));
    }
}
