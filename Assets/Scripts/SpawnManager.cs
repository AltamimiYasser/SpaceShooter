using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemiesContainer;

    [SerializeField]
    private GameObject[] _powerUps;

    [SerializeField]
    private GameObject _extraFirePowerUpPrefab;

    [SerializeField]
    private float _minEnemiesSpawnRate = 1.0f;

    [SerializeField]
    private float _maxEnemiesSpawnRate = 5.0f;

    [SerializeField]
    private float _minPowerUpSpawnRate = 3.0f;

    [SerializeField]
    private float _maxPowerUpSpawnRate = 7.0f;

    [SerializeField]
    private float _minExtraFireSpawnRate = 7.0f;

    [SerializeField]
    private float _maxExtraFireSpawnRate = 15.0f;

    private bool _allowedToSpawn = true;
    private int _wavCount = 5;
    [SerializeField] private float waveSpawnRate = 3f;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnRandomPowerUp());
        StartCoroutine(SpawnExtraFire());
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(0.3f);

        while (_allowedToSpawn)
        {
            for (int i = 0; i < _wavCount; i++)
            {
                if (!_allowedToSpawn)
                    break;
                Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 9.5f, 0);
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemiesContainer.transform;
                float spawnRate = Random.Range(_minEnemiesSpawnRate, _maxEnemiesSpawnRate);
                yield return new WaitForSeconds(1.0f);
            }
            _wavCount += 3;
            yield return new WaitForSeconds(waveSpawnRate);
        }
    }

    private IEnumerator SpawnRandomPowerUp()
    {
        while (_allowedToSpawn)
        {
            float spawnRate = Random.Range(_minPowerUpSpawnRate, _maxPowerUpSpawnRate);
            yield return new WaitForSeconds(spawnRate);
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 9.5f, 0.0f);

            int random = Random.Range(1, 101); // random number between 1 and 100
            int index = 0;

            // 0 tripleShots, 1 speed, 2, sheild, 3 ammo, 4 health, 5 steal
            switch (random)
            {
                case int n when (n > 0 && n <= 40): // 40% chance of ammo
                    index = 3;
                    break;
                case int n when (n > 40 && n <= 55): // 15% chance of health
                    index = 4;
                    break;
                case int n when (n > 55 && n <= 70): // 15% chance of speed
                    index = 1;
                    break;
                case int n when (n > 70 && n <= 80): // 10% chance of triple shots
                    index = 0;
                    break;
                case int n when (n > 80 && n <= 90): // 10% chance of sheild
                    index = 2;
                    break;
                    default: // 10% chance of steal powerup
                        index = 5;
                        break;
            }
            print("PowerUp now is at: " + index);

            GameObject randomPowerUp = _powerUps[index];
            Instantiate(randomPowerUp, posToSpawn, Quaternion.identity);
        }
    }

    private IEnumerator SpawnExtraFire()
    {
        while (_allowedToSpawn)
        {
            float spawnRate = Random.Range(_minExtraFireSpawnRate, _maxExtraFireSpawnRate);
            yield return new WaitForSeconds(spawnRate);
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 9.5f, 0.0f);
            Instantiate(_extraFirePowerUpPrefab, posToSpawn, Quaternion.identity);
        }
    }

    public void onPlayerDeath()
    {
        _allowedToSpawn = false;
    }
}