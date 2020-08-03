using System.Collections;
using UnityEngine;

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
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 9.5f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemiesContainer.transform;
            float spawnRate = Random.Range(_minEnemiesSpawnRate, _maxEnemiesSpawnRate);
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private IEnumerator SpawnRandomPowerUp()
    {
        while (_allowedToSpawn)
        {
            float spawnRate = Random.Range(_minPowerUpSpawnRate, _maxPowerUpSpawnRate);
            yield return new WaitForSeconds(spawnRate);
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 9.5f, 0.0f);

            GameObject randomPowerUp = _powerUps[Random.Range(0, _powerUps.Length)];
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