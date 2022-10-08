using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EventTimer timer = new EventTimer(2f);

    public EnemyController enemyPrefab;

    public static EnemyManager Instance { get; private set; }

    public List<EnemyController> Enemies { get; private set; } = new List<EnemyController>();

    public EnemyController FindClosestEnemy(Vector2 position)
    {
        EnemyController tMin = null;
        float minDist = Mathf.Infinity;
        foreach (var t in Enemies)
        {
            float dist = Vector2.Distance(t.transform.position, position);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    public EnemyController FindRandomEnemy()
    {
        return Enemies[Random.Range(0, Enemies.Count)];
    }

    public EnemyController FindRandomEnemy(Vector2 position, float range)
    {
        EnemyController[] enemiesInRange = Enemies.Where(x => Vector2.Distance(x.transform.position, position) <= range).ToArray();
        if(enemiesInRange.Length == 0)
        {
            return null;
        }

        return enemiesInRange[Random.Range(0, enemiesInRange.Length)];
    }

    // Start is called before the first frame update
    void Start()
    {
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        timer.Update();
    }

    private void Awake()
    {
        Instance = this;
        Enemies.AddRange(FindObjectsOfType<EnemyController>());
    }

    private void Timer_Tick()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        EnemyController newEnemy = Instantiate(enemyPrefab);
        Enemies.Add(newEnemy);
        newEnemy.transform.position = Random.insideUnitCircle * 15;
        newEnemy.EntityDied += Enemy_EntityDied;
    }

    private void Enemy_EntityDied(Assets.Scripts.Systems.Entities.Entity entity)
    {
        Enemies.Remove(entity.gameObject.GetComponent<EnemyController>());
        Destroy(entity.gameObject);
    }
}
