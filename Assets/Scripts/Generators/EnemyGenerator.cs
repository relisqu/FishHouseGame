using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;

public class EnemyGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Spawn Settings")]
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] int enemyAmount;
    [SerializeField] int initialAmount;
    [SerializeField] float enemyRespawnTime;
    [SerializeField] Transform spawnPoint;

    [Header("Movement Settings")]
    [SerializeField] float moveRadius;
    [SerializeField] private float moveSpeed = 3f; // Speed of the AI movement
    [SerializeField] private float rotationSpeed = 2f; // Speed of the AI rotation


    float _nextSpawnTime;

    List<(GameObject gameObject, Vector3 target, float timePassed)> activeEnemies;
    void Start()
    {
        activeEnemies = new();
        for (int i = 0; i < initialAmount; i++)
            SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var enemy in activeEnemies)
        {
            if (enemy.gameObject == null)
            {
                activeEnemies.Remove(enemy);
                break;
            }
            
        }
        if (activeEnemies.Count < enemyAmount && _nextSpawnTime < Time.time)
        {
            SpawnEnemy();
            _nextSpawnTime = Time.time + enemyRespawnTime;
        }

        MoveEnemies();
    }

    private void SpawnEnemy()
    {
        var enemy = Instantiate(enemyPrefab, GetRandomPositionInCircle(), spawnPoint.rotation).gameObject;

        activeEnemies.Add((enemy, GetRandomPositionInCircle(), 0));
    }

    private void MoveEnemies()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[i].gameObject == null) continue;

            if (ReachedTargetPosition(activeEnemies[i].target, activeEnemies[i].gameObject.transform.position))
            {
                Vector3 target = GetRandomPositionInCircle();
                target.y = activeEnemies[i].gameObject.transform.position.y;
                activeEnemies[i] =(activeEnemies[i].gameObject, target , 0);
            }

            activeEnemies[i].gameObject.transform.position = Vector3.MoveTowards(activeEnemies[i].gameObject.transform.position, activeEnemies[i].target, moveSpeed * Time.deltaTime);
            RotateTowards(activeEnemies[i].target, activeEnemies[i].gameObject);
        }
    }

    private Vector3 GetRandomPositionInCircle()
    {

        var point = Random.insideUnitCircle;
        var point3 = new Vector3(point.x, 0, point.y) * moveRadius + spawnPoint.position;


        return point3;
    }

    private void RotateTowards(Vector3 target, GameObject enemy)
    {
        // Calculate the direction to the target
        Vector3 direction = (target - enemy.transform.position).normalized;

        if (direction != Vector3.zero)
        {
            // Calculate the rotation angle towards the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target rotation
            enemy.transform.rotation =
                Quaternion.Slerp(enemy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private bool ReachedTargetPosition(Vector3 targetPosition, Vector3 currentPosition)
    {
        // Check if AI has reached the target position within certain distance
        return Vector3.Distance(currentPosition, targetPosition) < 0.1f;
    }

}
