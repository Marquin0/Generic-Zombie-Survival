using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public float maxHealth = 100;
    public float currentHealth;

    List<Healthbar> healthbars = new List<Healthbar>();

    public bool destroyOnDeath = false;

	// Use this for initialization
	void Start () {
        FindAllHealthbars();

        currentHealth = maxHealth;
        UpdateHealthbar();
    }

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        currentHealth = maxHealth;
        UpdateHealthbar();
    }

    private void FindAllHealthbars()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            var t = transform.GetChild(i);
            if(t.gameObject.name == "Healthbar")
            {
                healthbars.Add(new Healthbar(t.gameObject));
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(currentHealth < maxHealth && healthbars.Count != 0 && !healthbars[0].HealthbarUI.activeSelf)
        {
            UpdateHealthbar();
            foreach(var bar in healthbars)
                bar.HealthbarUI.SetActive(true);
        }
        if (currentHealth == maxHealth && healthbars.Count != 0 && healthbars[0].HealthbarUI.activeSelf && gameObject.tag != "Player")
        {
            foreach (var bar in healthbars)
                bar.HealthbarUI.SetActive(false);
        }

        if(isServer)
        {
            if (IsDead() && destroyOnDeath)
            {
                GameBoard.Buildings.Remove(gameObject);
                NetworkServer.Destroy(gameObject);
            }
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(float dmg)
    {
        currentHealth -= dmg;
        UpdateHealthbar();
    }

    public void Heal(float? amount = null)
    {
        if(amount.HasValue)
        {
            currentHealth += amount.Value;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
        }
        else
        {
            currentHealth = maxHealth;
        }
        UpdateHealthbar();
    }
    
    void UpdateHealthbar()
    {
        if (healthbars.Count == 0)
            return;
        foreach(var bar in healthbars)
            bar.HealthbarTransform.sizeDelta = new Vector2(currentHealth / maxHealth * bar.HealthbarMaxWidth, bar.HealthbarTransform.sizeDelta.y);
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}

class Healthbar
{
    public GameObject HealthbarUI;
    public GameObject HealthImage;
    public float HealthbarMaxWidth;
    public RectTransform HealthbarTransform;

    public Healthbar(GameObject bar)
    {
        HealthbarUI = bar;
        HealthImage = HealthbarUI.transform.Find("Health").gameObject;
        HealthbarTransform = HealthImage.GetComponent<RectTransform>();
        HealthbarMaxWidth = HealthbarTransform.sizeDelta.x;
    }
}