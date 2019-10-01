using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] Button newGameButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button quitButton;

    [SerializeField] string gameSceneName;

    // Start is called before the first frame update
    void Start()
    {
        newGameButton.onClick.AddListener(NewGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void NewGame()
    {
        SceneManager.LoadScene(gameSceneName);
        
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
