using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyController : NetworkBehaviour {

    Rigidbody2D rigidbody2D;
    Collider2D collider2D;
    float speed = 1.5f;

    float lastPlayerCheck = 0;
    float lastPlayerCheckDelay = 5f;
    float lastBuildingCheck = 0;
    float lastBuildingCheckDelay = 2f;
    GameObject selectedPlayer;
    GameObject selectedBuilding;

    float attackDistance = 0.35f;
    float attackDmg = 5f;
    float attackDelay = 1.5f;
    float currentAttackDelay = 0f;

    Health healthScript;

	// Use this for initialization
	void Start () {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        healthScript = GetComponent<Health>();
	}

    void Update()
    {
        if (!isServer)
            return;

        if (healthScript.IsDead())
        {
            EnemySpawner.Enemies.Remove(gameObject);
            NetworkServer.Destroy(gameObject);
        }

        SelectPlayer();
        SelectBuilding();
        AttackPlayer();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!isServer)
            return;

        MoveToPlayer();
    }

    private void SelectPlayer()
    {
        lastPlayerCheck -= Time.deltaTime;
        if (selectedPlayer == null || lastPlayerCheck <= 0)
        {
            lastPlayerCheck = lastPlayerCheckDelay;
            var players = GameObject.FindGameObjectsWithTag("Player");

            float shortestDistance = float.MaxValue;
            foreach (var player in players)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (shortestDistance > distance)
                {
                    selectedPlayer = player;
                    shortestDistance = distance;
                }
            }
        }
    }

    private void MoveToPlayer()
    {
        if (selectedPlayer != null)
        {
            var enemyPosition = selectedPlayer.transform.position;

            var direction = new Vector2(enemyPosition.x - transform.position.x, enemyPosition.y - transform.position.y);
            var hyp = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
            direction.x /= hyp;
            direction.y /= hyp;

            var totalDirection = Mathf.Abs(direction.x) + Mathf.Abs(direction.y);

            direction.x = speed * direction.x / totalDirection;
            direction.y = speed * direction.y / totalDirection;

            rigidbody2D.velocity = direction;
        }
    }

    [Server]
    private void AttackPlayer()
    {
        currentAttackDelay -= Time.deltaTime;
        
        if (selectedPlayer == null)
        {
            AttackBuilding();
        }
        else
        {
            var distance = Vector2.Distance(transform.position, selectedPlayer.transform.position);

            if (currentAttackDelay <= 0)
            {
                if (distance <= attackDistance)
                {
                    currentAttackDelay = attackDelay;
                    var healthScript = selectedPlayer.GetComponent<Health>();
                    healthScript.RpcTakeDamage(attackDmg);
                }
                else
                {
                    AttackBuilding();
                }
            }
        }
    }

    private void SelectBuilding()
    {
        lastBuildingCheck -= Time.deltaTime;
        if (selectedBuilding == null || lastBuildingCheck <= 0)
        {
            lastBuildingCheck = lastBuildingCheckDelay;

            float shortestDistance = float.MaxValue;
            foreach (var building in GameBoard.Buildings)
            {
                float distance = Vector2.Distance(transform.position, building.transform.position);
                if (shortestDistance > distance)
                {
                    selectedBuilding = building;
                    shortestDistance = distance;
                }
            }
        }
    }

    private void AttackBuilding()
    {
        if (selectedBuilding == null)
            return;

        var distance = Vector2.Distance(transform.position, selectedBuilding.transform.position);

        if (currentAttackDelay <= 0)
        {
            if (distance <= attackDistance)
            {
                currentAttackDelay = attackDelay;
                var healthScript = selectedBuilding.GetComponent<Health>();
                healthScript.RpcTakeDamage(attackDmg);
            }
        }
    }
    
}
