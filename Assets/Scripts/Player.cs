using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _lives = 3;

    private int _score;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleLaserPrefab;

    [SerializeField]
    private float _speed = 3.5f;

    private float _originalSpeed;
    private float _thrusterSpeed = 10.0f;

    [SerializeField]
    private float _fireRate = 0.5f;

    private float _canFire = -1f;
    private bool _tripleShotsEnabled = false;

    [SerializeField]
    private GameObject _playerSheild;

    private bool _sheildActive = false;
    [SerializeField] private int _sheildHits = 3;
    private SpriteRenderer _sheildRenderer;

    [SerializeField]
    private GameObject _enginLeftHurt, _enginRightHurt;

    private SpawnManager spawnManager;

    private UIManager _uIManager;

    private void Start()
    {
        _originalSpeed = _speed;
        _score = 0;

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); // get spawn manager
        if (spawnManager == null)
            Debug.LogError("Spawn Manager is null");

        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uIManager == null)
            Debug.LogError("UIManger is null");

        _sheildRenderer = transform.Find("PlayerSheild").GetComponent<SpriteRenderer>();
        if (_sheildRenderer == null)
            Debug.LogError("Sprite renderer is null");
    }

    // Update is called once per frame
    private void Update()
    {
        HandelInput();
        RestrictMovement();

        if (Input.GetKeyDown(KeyCode.Space) && _canFire < Time.time)
            Fire();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            if (other.gameObject.GetComponent<Laser>().getCaller() == "Enemy")
                Damage();
        }
    }

    private void HandelInput()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector3 displacement = new Vector3(xInput, yInput, 0);

        if (Input.GetKey(KeyCode.LeftShift))
            _speed = _thrusterSpeed;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            _speed = _originalSpeed;

        transform.Translate(displacement * _speed * Time.deltaTime);
    }

    private void RestrictMovement()
    {
        if (transform.position.x > 11.25)
            transform.position = new Vector3(-11.25f, transform.position.y, 0);
        else if (transform.position.x < -11.25)
            transform.position = new Vector3(11.25f, transform.position.y, 0);

        float yPos = Mathf.Clamp(transform.position.y, -3.95f, 0);
        transform.position = new Vector3(transform.position.x, yPos, 0);
    }

    private void Fire()
    {
        _canFire = Time.time + _fireRate;
        if (_tripleShotsEnabled)
        {
            Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Vector3 offset = new Vector3(0, 1.05f, 0);
            GameObject laserObject = Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            Laser laser = laserObject.GetComponent<Laser>();
            if (laserObject == null || laser == null)
                Debug.LogError("Laser object or laser is null");

            laser.setCaller("Player");
        }
    }

    public void Damage()
    {
        if (_sheildActive)
        {
            _sheildHits--;

            if (_sheildHits == 0)
            {
                _sheildActive = false;
                _playerSheild.SetActive(false);
                _sheildHits = 3;
            }

            switch (_sheildHits)
            {
                case 3:
                    _sheildRenderer.color = new Color(0f, 0f, 1f); // blue
                    break;
                case 2:
                    _sheildRenderer.color = new Color(1f, 0.54f, 0f); // orange
                    break;
                case 1:
                    _sheildRenderer.color = new Color(1, 0, 0); // red
                    break;
            }
            return;
        }

        _lives--;
        _uIManager.setLives(_lives);

        // burn animation
        switch (_lives)
        {
            case 2:
                _enginLeftHurt.SetActive(true);
                break;

            case 1:
                _enginRightHurt.SetActive(true);
                break;
        }

        if (_lives < 1)
        {
            spawnManager.onPlayerDeath();
            _uIManager.EnableGameover();
            Destroy(this.gameObject);
        }
    }

    public void EnableTripleShot()
    {
        _tripleShotsEnabled = true;
        StartCoroutine(DisableTripleShot());
    }

    private IEnumerator DisableTripleShot()
    {
        yield return new WaitForSeconds(5);
        _tripleShotsEnabled = false;
    }

    public void EnableSpeedBoost()
    {
        _speed = 10.0f;
        StartCoroutine(DisableSpeedBoost());
    }

    private IEnumerator DisableSpeedBoost()
    {
        yield return new WaitForSeconds(5);
        _speed = _originalSpeed;
    }

    public void EnableSheild()
    {
        _sheildActive = true;
        _playerSheild.SetActive(true);
    }

    public void AddToScore(int points)
    {
        _score += points;
        _uIManager.setScore(_score);
    }

    public void setSheildHits()
    {
        _sheildHits = 3;
    }
}