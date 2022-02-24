using Extensions;
using UnityEngine.SceneManagement;

public class LosePanel : Panel
{
    protected override void OnButtonClick()
    {
        RestartLevel();
    }

    private void RestartLevel()
    {
        Amplitude.Instance.LogLevelRestart(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
