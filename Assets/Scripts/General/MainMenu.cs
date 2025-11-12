using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Credits;



    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OpenCredits()
    {
        Credits.SetActive(true);
    }

    public void Quit()
    {
      Application.Quit();
    }
}
