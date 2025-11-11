using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Nama-nama scene yang ingin dipanggil
    public string scene1Name = "Gameplay1";
    public string scene2Name = "Gameplay2";
    public string scene3Name = "Gameplay3";

    public void LoadScene1()
    {
        SceneManager.LoadScene(scene1Name);
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene(scene2Name);
    }

    public void LoadScene3()
    {
        SceneManager.LoadScene(scene3Name);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game exited."); // Biar kelihatan di editor
    }
}
