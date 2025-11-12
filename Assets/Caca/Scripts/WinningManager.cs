using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningManager : MonoBehaviour
{
    public GameObject winScreen;
    public string scene1Name; // Scene pertama (misalnya: Next Level)
    public string scene2Name; // Scene kedua (misalnya: Main Menu)

    void Start()
    {
        if (winScreen != null)
            winScreen.SetActive(false);
    }

    public void ShowWin()
    {
        if (winScreen != null)
            winScreen.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene1Name);
    }

    public void LoadScene2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene2Name);
    }
}
