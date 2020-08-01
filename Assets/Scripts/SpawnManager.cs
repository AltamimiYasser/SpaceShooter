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
    private float _minEnemiesSpawnRate = 1.0f;

    [SerializeField]
    private float _maxEnemiesSpawnRate = 5.0f;

    [SerializeField]
    private float minPowerUpSpawnRate = 3.0f;

    [SerializeField]
    private float maxPowerUpSpawnRate = 7.0f;

    private bool _allowedToSpawn = true;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(spawnRandomPowerUp());
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

    private IEnumerator spawnRandomPowerUp()
    {
        yield return new WaitForSeconds(0.3f);

        while (_allowedToSpawn)
        {
            float spawnRate = Random.Range(minPowerUpSpawnRate, maxPowerUpSpawnRate);
            yield return new WaitForSeconds(spawnRate);
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 9.5f, 0.0f);

            GameObject randomPowerUp = _powerUps[Random.Range(0, _powerUps.Length)];
            Instantiate(randomPowerUp, posToSpawn, Quaternion.identity);
        }
    }

    public void onPlayerDeath()
    {
        _allowedToSpawn = false;
    }
}