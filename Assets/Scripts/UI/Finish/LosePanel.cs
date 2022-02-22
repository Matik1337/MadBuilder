using UnityEngine.SceneManagement;

public class LosePanel : Panel
{
    protected override void OnButtonClick()
    {
        RestartLevel();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
