using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class PlayerController : NetworkBehaviour
{
    public float speed = 1.75f;
    private Rigidbody2D rigidbody2D;
    float xVelocity, yVelocity;
    private PlayerBuilder builderScript;
    public GameObject[] buildingButtons = new GameObject[3];
    private GameObject buildingPlacement;
    private GameObject selectedBuilding;
    public Camera playerCamera;
    private Text gameStatusText;
    private GameObject previousSelected;

    private PlayerBuilder playerBuilder;

    private bool build = false;

    public LayerMask layerMask;

    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;

    // Use this for initialization
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        builderScript = GetComponent<PlayerBuilder>();
        buildingPlacement = builderScript.GetBuildingPlacement();
        var gameStatusCanvas = transform.Find("GameStatusText");
        gameStatusText = gameStatusCanvas.Find("Text").gameObject.GetComponent<Text>();
        EnablePlayer();
        GameBoard.Playerlist.Add(gameObject);
        playerBuilder = GetComponent<playerBuilder>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        Movement();
        BuildingSelection();
        UpdateBuildingPlacement();
        MouseClick();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void DisablePlayer()
    {
        onToggleShared.Invoke(false);

        if (isLocalPlayer)
            onToggleLocal.Invoke(false);
        else
            onToggleRemote.Invoke(false);
    }

    void EnablePlayer()
    {
        onToggleShared.Invoke(true);

        if (isLocalPlayer)
            onToggleLocal.Invoke(true);
        else
            onToggleRemote.Invoke(true);
    }

    [ClientRpc]
    public void RpcUpdateGameStatusText(string text)
    {
        UpdateGameStatusText(text);
    }

    public void UpdateGameStatusText(string text)
    {
        if (gameStatusText == null)
            return;

        gameStatusText.text = text;
    }

    private void Movement()
    {
        xVelocity = Input.GetAxis("Horizontal") * speed;
        yVelocity = Input.GetAxis("Vertical") * speed;

        float totalVelocity = Mathf.Abs(xVelocity) + Mathf.Abs(yVelocity);
        float xPercentage = totalVelocity == 0f ? 0 : xVelocity / totalVelocity;
        float yPrecentage = totalVelocity == 0f ? 0 : yVelocity / totalVelocity;

        xVelocity = speed * xPercentage * xVelocity;
        yVelocity = speed * yPrecentage * yVelocity;

        rigidbody2D.velocity = new Vector2(xVelocity, yVelocity);
    }

    private void BuildingSelection()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            builderScript.SwitchBuilding(-1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            builderScript.SwitchBuilding(0);
            playerBuilder.SelectBulding(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            builderScript.SwitchBuilding(1);
            playerBuilder.SelectBulding(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            builderScript.SwitchBuilding(2);
            playerBuilder.SelectBulding(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            builderScript.SwitchBuilding(3);
            playerBuilder.SelectBulding(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            builderScript.SwitchBuilding(4);
            playerBuilder.SelectBulding(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            builderScript.SwitchBuilding(5);
            playerBuilder.SelectBulding(5);
        }
    }

    private void MouseClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            build = false;
            return;
        }

        if (!Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0)) return;

        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, layerMask);

        if (!build && hit.collider != null && hit.collider.gameObject.layer == 9 && Input.GetMouseButtonUp(0))
        {
            builderScript.CmdBuild(buildingPlacement.transform.position, builderScript.selectedBuilding);
            build = true;
            SetSelectedBuilding(playerBuilder.GetSelectedBuilding().Gameobject);
        }
        else if (hit.collider == null && Input.GetMouseButton(0) && buildingPlacement.activeSelf)
        {
            builderScript.CmdBuild(buildingPlacement.transform.position, builderScript.selectedBuilding);
            build = true;
            SetSelectedBuilding(null);
        }
        else if (hit.collider == null || hit.collider.gameObject.layer != 9)
        {
            SetSelectedBuilding(null);
        }
        else
        {
            SetSelectedBuilding(builderScript.GetBuilding(selectedBuilding));
        }
    }

    private void UpdateBuildingPlacement()
    {
        if (!buildingPlacement.activeSelf) return;

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var x = (int)(mousePosition.x / 0.3f);
        var xoff = mousePosition.x - x * 0.3f;
        var y = (int)(mousePosition.y / 0.3f);
        var yoff = mousePosition.y - y * 0.3f;
        Vector2 position = new Vector2(((x + (int)(xoff / 0.15f)) * 0.3f), ((y + (int)(yoff / 0.15f)) * 0.3f));

        buildingPlacement.transform.position = position;
    }

    private void SetSelectedBuilding(GameObject selected)
    {
        if (selected != null)
        {
            switch (selected)
            {
                case selectedBuilding:
                    SetSelectedBuilding(null);
                    return;
                case previousSelected:
                    SetSelectedBuilding(previousSelected);
                    break;
            }

            if (selectedBuilding != null)
            {
                SetSelectedBuilding(null);
            }
            selectedBuilding = selected;
            selectedBuilding.transform.Find("Menu").gameObject.SetActive(true);
        }
        else
        {
            if (selectedBuilding != null)
            {
                if (selectedBuilding == previousSelected)
                {
                    selectedBuilding.transform.Find("Menu").gameObject.SetActive(false);
                    selectedBuilding = previousSelected;
                    return;
                }

                selectedBuilding.transform.Find("Menu").gameObject.SetActive(false);
                selectedBuilding = null;
            }
        }
    }
}
