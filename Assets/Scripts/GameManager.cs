using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private bool _gameIsOver;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _gameIsOver)
            SceneManager.LoadScene(1); // game

        if (Input.GetKeyDown(KeyCode.Escape) && Screen.fullScreen)
            Application.Quit();
    }

    public void setGameover()
    {
        _gameIsOver = true;
    }
}