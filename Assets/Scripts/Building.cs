using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Building : NetworkBehaviour
{
    public string name;
    public int costs = 10;
    public float life = 100;
    public PlayerBuilder playerBuilderScript;

    public GameObject menu;
    public Text menuUpgradeText;
    public Text menuUpgradeCosts;

    public Text menuLblName;
    public Text menuTxtRange;
    public Text menuTxtDmg;
    public Text menuTxtSpeed;

    public int currentLvl = 1;
    public int maxLvl = 5;
    public int upgradeCosts = 50;
    public float upgradeCostsScale = 2f;
    public float lifeScale = 2f;
    public int levelStep = 1;
    public int maxLevelStep = 10;

    // Use this for initialization
    public virtual void Start()
    {
        GetComponent<Health>().SetMaxHealth(life);
        menu = transform.Find("Menu").gameObject;
        var menuUpgrade = menu.transform.Find("Upgrade");
        menuUpgradeText = menuUpgrade.Find("Text").gameObject.GetComponent<Text>();
        menuUpgradeCosts = menuUpgrade.Find("UpgradeCosts").gameObject.GetComponent<Text>();

        var menuPanel = menu.transform.Find("Panel");
        menuLblName = menuPanel.Find("lblName").gameObject.GetComponent<Text>();
        menuTxtRange = menuPanel.Find("txtRange").gameObject.GetComponent<Text>();
        menuTxtDmg = menuPanel.Find("txtDmg").gameObject.GetComponent<Text>();
        menuTxtSpeed = menuPanel.Find("txtSpeed").gameObject.GetComponent<Text>();

        UpdateMenu();
    }

    [Command]
    public void CmdUpgrade()
    {
        if (currentLvl >= maxLvl)
            return;

        if (!playerBuilderScript.HasEnoughGold(upgradeCosts))
            return;

        playerBuilderScript.RpcDecreaseGoldResource(upgradeCosts);
        RpcUpgrade();
    }

    [ClientRpc]
    public virtual void RpcUpgrade()
    {
        Upgrade();
    }

    public virtual void Upgrade()
    {
        if (currentLvl >= maxLvl)
            return;

        currentLvl += levelStep;
        upgradeCosts = (int)(upgradeCosts * upgradeCostsScale * currentLvl * levelStep);
        life = (int)(life * lifeScale * currentLvl);
        GetComponent<Health>().SetMaxHealth(life);

        UpdateMenu();
    }

    public virtual void UpdateMenu()
    {
        menuLblName.text = name + " " + "Lvl " + currentLvl + $" (Levelstep: {levelStep})";

        if (currentLvl == maxLvl)
        {
            menuUpgradeText.text = "Max Level";
            menuUpgradeCosts.text = "";
        }
        else
        {
            menuUpgradeCosts.text = upgradeCosts.ToString();
        }
    }

    public virtual void IncreaseLevelStep(int levelIncrease)
    {
        levelStep += levelIncrease;
        levelStep %= maxLevelStep;
    }
}
