using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    private TrhurserCharg _trhurserCharg;
    private TrhurserCharg _ammoCharge;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.LogError("GameManger is null");

        _trhurserCharg = GameObject.Find("ThrusterBarCharg").GetComponent<TrhurserCharg>();
        if (_trhurserCharg == null)
            Debug.LogError("Thruster charge is null");

        _ammoCharge = GameObject.Find("AmmoUI").GetComponent<TrhurserCharg>();
        if (_ammoCharge == null)
            Debug.LogError("Ammo charge is null");
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

    private IEnumerator FlickerGameoverText()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void UpdateThrusterCharge(float charge)
    {
        _trhurserCharg.UpdateCharge(charge);
    }

    public void UpdateAmmoCharge(int ammo)
    {
        _ammoCharge.UpdateAmmo(ammo);
    }
}