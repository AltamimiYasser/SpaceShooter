using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    [SerializeField] private float waveSpeed = 3f;

    private enum Callers { player, enemy };
    private enum LaserType { Regular, Wavy }

    private Callers _caller;
    private LaserType _type;

    private void Start()
    { 
        _type = (LaserType) Random.Range(0, Enum.GetValues(typeof(LaserType)).Length); // pick random type
    }

   
    // Update is called once per frame
    private void Update()
    {
        switch (_caller)
        {
            case Callers.player:
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
                break;
            }

            case Callers.enemy:
            {
                MoveEnemyLaser();
                break;
            }
        }

        if (transform.position.y > 8 || transform.position.y < -10)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(this.gameObject);
        }
    }

    private void MoveEnemyLaser()
    {
        switch (_type)
        {
            case LaserType.Regular:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                break;
            case LaserType.Wavy:
                var xDisplacement = Mathf.Cos(Time.time);
                var displacement = new Vector3(xDisplacement, -1, 0);
                transform.Translate(displacement * waveSpeed * Time.deltaTime);
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Asteroid")
            Destroy(this.gameObject);
    }

    public void setCaller(string caller)
    {
        switch (caller)
        {
            case "Player":
                _caller = Callers.player;
                break;

            case "Enemy":
                _caller = Callers.enemy;
                break;

            default:
                Debug.LogError("Caller incorrect");
                break;
        }
    }

    public string getCaller()
    {
        switch (_caller)
        {
            case Callers.player:
                return "Player";

            case Callers.enemy:
                return "Enemy";

            default:
                return "error";
        }
    }
}