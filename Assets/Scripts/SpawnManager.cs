using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform player;
    [SerializeField] private float respawnDelay = 2f;

    private GameObject currentEnemy;
    private Coroutine spawnRoutine;

    private void Start()
    {
        SpawnEnemy();
    }

    private void Update()
    {
        if (currentEnemy == null && spawnRoutine == null)
        {
            spawnRoutine = StartCoroutine(RespawnEnemy());
        }
    }

    private IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(respawnDelay);

        SpawnEnemy();
        spawnRoutine = null;
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null || player == null)
        {
            Debug.LogError("SpawnManager references are missing.");
            return;
        }

        currentEnemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        EnemyMovement enemyMovement =
            currentEnemy.GetComponent<EnemyMovement>();

        if (enemyMovement != null)
        {
            enemyMovement.SetTarget(player);
        }
    }
}