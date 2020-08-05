using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyMovementType
{
    Straight,
    Angle,
    Wavy
}


public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4;

    [SerializeField] private GameObject _laserPrefab;

    private Player _player;
    private Animator _anim;

    private EnemyMovementType _movementType = EnemyMovementType.Straight;

    private AudioSource _explosionSound;

    private void Start()
    {
        SetupTypes();
        InstantiateObjects();
        StartCoroutine(Fire());
    }

    private void SetupTypes()
    {
        // pick a random type from enum
        _movementType = (EnemyMovementType) Random.Range(0, Enum.GetValues(typeof(EnemyMovementType)).Length);
    }

    private void InstantiateObjects()
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
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        Reposition();
    }

    private void Reposition()
    {
        if (transform.position.y < -7.5)
        {
            var newX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(newX, 9.5f, 0);
        }
    }

    private void Move()
    {
        if (_movementType == EnemyMovementType.Straight)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else if (_movementType == EnemyMovementType.Wavy)
        {
            var xDisplacement = Mathf.Cos(Time.time);
            var displacement = new Vector3(xDisplacement, -1, 0);
            transform.Translate(displacement * _speed * Time.deltaTime);
        }
        else
        {
            var xDisplacement = transform.position.x >= 0 ? -1 : 1;
            var displacement = new Vector3(xDisplacement, -1, 0);
            transform.Translate(displacement * _speed * Time.deltaTime);
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
            Destroy(gameObject, 2.5f);
        }

        if (other.tag == "Laser" && other.GetComponent<Laser>().getCaller() == "Player")
        {
            Destroy(other.gameObject);
            _player.AddToScore(1);
            _anim.SetTrigger("Explode");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            _speed *= 1.2f;
            _explosionSound.Play();
            Destroy(gameObject, 2.5f);
        }
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            float random = Random.Range(6, 10);
            yield return new WaitForSeconds(random);
            var offset = new Vector3(0, -1.05f, 0);
            var laserObject = Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            var laser = laserObject.GetComponent<Laser>();
            if (laserObject == null || laser == null)
                Debug.LogError("Laser object or laser is null");

            laser.setCaller("Enemy");
        }
    }
}