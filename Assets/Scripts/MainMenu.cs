using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject levelsCanvas;
    [SerializeField] private GameObject loadingScreen;

    public void Play()
    {
        loadingScreen.SetActive(true);
        mainMenuCanvas.SetActive(false);
        SceneManager.LoadScene(1);
    }
    public void Levels()
    {
        mainMenuCanvas.SetActive(false);
        levelsCanvas.SetActive(true);
    }
    public void Level1()
    {
        loadingScreen.SetActive(true);
        levelsCanvas.SetActive(false);
        SceneManager.LoadScene(1);
    }
    public void Level2()
    {
        loadingScreen.SetActive(true);
        levelsCanvas.SetActive(false);
        SceneManager.LoadScene(2);
    }
    public void Back()
    {
        mainMenuCanvas.SetActive(true);
        levelsCanvas.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
