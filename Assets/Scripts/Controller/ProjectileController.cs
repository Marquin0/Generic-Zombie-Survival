using Assets.Scripts.Systems.Entities;
using Assets.Scripts.Systems.Modifier;
using Assets.Scripts.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileController : AppliesModifiers, IHasCollision, IIsPoolObject<ProjectileController>
{
    public ProjectileController()
    {
    }

    private Vector3 targetDirection;
    private bool fired = false;

    private Rigidbody rb;
    private CollisionDetection collisionDetection;

    public float baseSpeed = 15f;
    public float baseLifetime = 5f;
    public bool IsPlayer;
    private string targetTag;
    private IObjectPool<ProjectileController> pool;

    private void Start()
    {
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collisionDetection = GetComponent<CollisionDetection>();
        collisionDetection.Collided += CollisionDetection_Collided;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!fired)
        {
            return;
        }

        rb.velocity = targetDirection * baseSpeed;
    }

    public void Fire(Vector3 targetDirection, bool isPlayer)
    {
        fired = true;
        this.targetDirection = targetDirection;
        this.IsPlayer = isPlayer;
        collisionDetection.SetColliders(targetTag, "Wall", "...");

        StartCoroutine(DecreaseLifeSpan());
    }

    private void CollisionDetection_Collided(GameObject sender, GameObject collider)
    {
        StopAllCoroutines();
        ApplyModifiers(collider.GetComponent<Entity>());

        ResetToPool();
    }

    private IEnumerator DecreaseLifeSpan()
    {
        yield return new WaitForSeconds(baseLifetime);
        ResetToPool();
    }

    public void ResetToPool()
    {
        gameObject.SetActive(false);
        pool.Release(this);
    }

    public void SetTargetTag(string targetTag)
    {
        this.targetTag = targetTag;
    }

    public void SetPool(IObjectPool<ProjectileController> pool)
    {
        this.pool = pool;
    }
}
