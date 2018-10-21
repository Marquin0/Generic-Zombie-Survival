using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TowerController : Building {

    public float shootingSpeed = 2f;
    float shootingDelay = 0f;
    public float range = 5f;

    public GameObject selectedEnemy;
    public GameObject bulletPrefab;

    public float rangeLvlScale = 1;
    public float speedLvlScale = 1;
    public float dmgLvlScale = 1f;

	// Use this for initialization
	public override void Start () {
        GetComponent<Health>().SetMaxHealth(life);
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isServer)
            return;

        SelectEnemy();
        Shoot();
	}

    [ClientRpc]
    public override void RpcUpgrade()
    {
        if (currentLvl >= maxLvl)
            return;
        
        range *= rangeLvlScale;
        shootingSpeed *= speedLvlScale;

        Upgrade();
    }

    public override void UpdateMenu()
    {
        base.UpdateMenu();

        menuTxtRange.text = range.ToString();
        menuTxtDmg.text = (bulletPrefab.GetComponent<BulletController>().dmg*dmgLvlScale).ToString();
        menuTxtSpeed.text = shootingSpeed.ToString();
    }

    private void SelectEnemy()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        selectedEnemy = null;

        float shortestDistance = float.MaxValue;
        foreach (var enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (shortestDistance > distance && range*0.3 >= distance)
            {
                selectedEnemy = enemy;
                shortestDistance = distance;
            }
        }
    }
    
    private void Shoot()
    {
        shootingDelay -= Time.deltaTime;
        if (selectedEnemy == null) return;

        if(shootingDelay <= 0)
        {
            shootingDelay = 1f / shootingSpeed;
            var newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = transform.position;
            var bulletScript = newBullet.GetComponent<BulletController>();

            if(currentLvl != 1)
                bulletScript.dmg += bulletScript.dmg * dmgLvlScale * currentLvl;
            bulletScript.Shoot(selectedEnemy, range*0.3f);

            NetworkServer.Spawn(newBullet);
            
        }
    }
}
