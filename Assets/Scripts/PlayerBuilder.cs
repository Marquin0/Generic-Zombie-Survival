using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;

public class PlayerBuilder : NetworkBehaviour {

    int goldResource = 100;
    Text goldText;

    public int selectedBuilding;
    private GameObject grid;
    public GameObject gridPrefab;
    private GameObject buildingPlacement;
    public GameObject buildingPlacementPrefab;
    public Camera playerCamera;

    public GameObject buildingBtnPrefab;
    public GameObject[] buildings = new GameObject[3];
    private List<GameObject> placedBuildings = new List<GameObject>();
    private List<GameObject> buildingBtnList = new List<GameObject>();

    private BuildingSelector buildingSelector;
    
	// Use this for initialization
	void Start () {
        var resources = transform.Find("Resources");
        goldText = resources.transform.Find("txtGold").gameObject.GetComponent<Text>();
        grid = Instantiate(gridPrefab);
        var gridScript = grid.GetComponent<Grid>();
        gridScript.SetCamera(playerCamera);
        grid.SetActive(false);
        GetBuildingPlacement();
        UpdateGoldAmount();

        var buildMenu = transform.Find("BuildMenu");
        var toolTip = buildMenu.transform.Find("ToolTip");
        var buildMenuPanel = buildMenu.Find("Panel");

        foreach(var building in buildings)
        {
            var newBtn = Instantiate(buildingBtnPrefab);
            newBtn.GetComponent<Image>().sprite = building.GetComponent<SpriteRenderer>().sprite;
            var newBtnToolTipScript = newBtn.GetComponent<BtnToolTip>();
            var buildingScript = building.GetComponent<Building>();
            newBtnToolTipScript.SetTooltip(toolTip.gameObject);
            newBtnToolTipScript.title = buildingScript.name;
            newBtnToolTipScript.costs = buildingScript.costs.ToString();
            newBtnToolTipScript.life = buildingScript.life.ToString();
            if(buildingScript is TowerController)
            {
                var towerScript = (TowerController)buildingScript;
                newBtnToolTipScript.dmg = towerScript.bulletPrefab.GetComponent<BulletController>().dmg.ToString();
                newBtnToolTipScript.range = towerScript.range.ToString();
            }

            buildingBtnList.Add(newBtn);
            newBtn.transform.Find("Text").GetComponent<Text>().text = buildingBtnList.Count.ToString();
            newBtn.transform.SetParent(buildMenuPanel);
            newBtn.transform.localScale = new Vector3(1, 1, 1);
        }

        buildingSelector = new BuildingSelector(buildings.Select(x => x.GetComponent<buildings>()));
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(0))
        {

        }
	}

    public GameObject GetBuildingPlacement()
    {
        if(buildingPlacement == null)
        {
            buildingPlacement = Instantiate(buildingPlacementPrefab);
            buildingPlacement.SetActive(false);
        }
        return buildingPlacement;
    }

    public bool HasEnoughGold(int goldRequired)
    {
        return goldResource >= goldRequired;
    }

    [ClientRpc]
    public void RpcIncreaseGoldResource(int amount)
    {
        goldResource += amount;
        UpdateGoldAmount();
    }

    [ClientRpc]
    public void RpcDecreaseGoldResource(int amount)
    {
        goldResource -= amount;
        UpdateGoldAmount();
    }

    private void UpdateGoldAmount()
    {
        goldText.text = goldResource.ToString();
    }

    public void SwitchBuilding(int index)
    {
        if(selectedBuilding != -1)
        {
            buildingBtnList[selectedBuilding].transform.Find("Selected").gameObject.SetActive(false);
        }
        if(index != -1)
        {
            buildingBtnList[index].transform.Find("Selected").gameObject.SetActive(true);
        }

        selectedBuilding = index;
        grid.SetActive(index != -1);
        buildingPlacement.SetActive(index != -1);
    }

    [Command]
    public void CmdBuild(Vector2 position, int selBuilding)
    {
        if (selBuilding == -1) return;

        var buildingScript = buildings[selBuilding].GetComponent<Building>();

        if (buildingScript.costs > goldResource) return;

        var raycast = Physics2D.BoxCast(position, new Vector2(0.28f, 0.28f), 0, Vector2.zero);
        if(raycast.collider != null)
        {
            return;
        }
        
        RpcDecreaseGoldResource(buildingScript.costs);
        var newBuilding = Instantiate(buildings[selBuilding], position, Quaternion.identity);
        newBuilding.GetComponent<Building>().playerBuilderScript = this;
        placedBuildings.Add(newBuilding);
        GameBoard.Buildings.Add(newBuilding);
        NetworkServer.Spawn(newBuilding);
    }

    public void SelectBulding(int index)
    {
        buildingSelector.SetSelection(index);
    }

    public Bulding GetSelectedBuilding()
    {
        buildingSelector.GetSelectedBuilding();
    }
}
