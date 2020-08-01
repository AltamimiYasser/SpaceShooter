using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _livesSprites;

    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartTextInstruction;

    private GameManager _gameManager;


    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.LogError("GameManger is null");
    }


    public void setScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void setLives(int lives)
    {
        _livesImage.sprite = _livesSprites[lives];
    }

    public void EnableGameover()
    {
        StartCoroutine(FlickerGameoverText());
        _restartTextInstruction.gameObject.SetActive(true);
        _gameManager.setGameover();
    }

    IEnumerator FlickerGameoverText()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
