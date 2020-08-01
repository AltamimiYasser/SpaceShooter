using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;

    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private Animator _anim;

    private AudioSource _explosionSound;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
            Debug.LogError("Player is null");

        _anim = GetComponent<Animator>();
        if (_anim == null)
            Debug.LogError("Animator is null");

        _explosionSound = GetComponent<AudioSource>();
        if (_explosionSound == null)
            Debug.LogError("Explosion sound is null");

        StartCoroutine(Fire());
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7.5)
        {
            float newX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(newX, 9.5f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player.Damage();
            _anim.SetTrigger("Explode");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            _speed *= 1.2f;
            _explosionSound.Play();
            Destroy(this.gameObject, 2.5f);
        }

        if (other.tag == "Laser" && other.GetComponent<Laser>().getCaller() == "Player")
        {

            print("Player shooter");
            Destroy(other.gameObject);
            _player.AddToScore(1);
            _anim.SetTrigger("Explode");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            _speed *= 1.2f;
            _explosionSound.Play();
            Destroy(this.gameObject, 2.5f);

        }
    }

    IEnumerator Fire()
    {
        while (true)
        {
            float random = Random.Range(3, 8);
            yield return new WaitForSeconds(random);
            Vector3 offset = new Vector3(0, -1.05f, 0);
            GameObject laserObject = Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            Laser laser = laserObject.GetComponent<Laser>();
            if (laserObject == null || laser == null)
                Debug.LogError("Laser object or laser is null");

            laser.setCaller("Enemy");
        }
    }
}