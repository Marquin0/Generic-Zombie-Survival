using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour {

    public float dmg = 10;
    public float speed = 5f;
    float range;
    float traveled = 0;
    GameObject target;
    Vector2 velocity;

    Rigidbody2D rigidbody2D;

    bool shooting = false;

	// Use this for initialization
	void Start () {
        
	}

    void OnEnable()
    {
        traveled = 0;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isServer)
            return;

        if(traveled >= range)
        {
            RpcDisableBullet();
        }


    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!shooting) return;

        traveled += Vector2.Distance(Vector2.zero, velocity) * Time.deltaTime;

        
    }

    public void Shoot(GameObject target, float range)
    {
        this.target = target;
        this.range = range;

        shooting = true;

        CalculateVelocity();
    }

    private void CalculateVelocity()
    {
        var direction = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
        var hyp = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
        direction.x /= hyp;
        direction.y /= hyp;

        var totalDirection = Mathf.Abs(direction.x) + Mathf.Abs(direction.y);

        direction.x = speed * direction.x / totalDirection;
        direction.y = speed * direction.y / totalDirection;

        velocity = direction;
        rigidbody2D.velocity = velocity;
    }

    [Server]
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Enemy") return;

        var healthScript = other.gameObject.GetComponent<Health>();
        healthScript.RpcTakeDamage(dmg);

        RpcDisableBullet();
    }

    [ClientRpc]
    private void RpcDisableBullet()
    {
        NetworkServer.Destroy(gameObject);
    }
}
