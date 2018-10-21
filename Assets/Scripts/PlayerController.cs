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



    private bool build = false;

    public LayerMask layerMask;

    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;

    // Use this for initialization
    void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        builderScript = GetComponent<PlayerBuilder>();
        buildingPlacement = builderScript.GetBuildingPlacement();
        var gameStatusCanvas = transform.Find("GameStatusText");
        gameStatusText = gameStatusCanvas.Find("Text").gameObject.GetComponent<Text>();
        EnablePlayer();
        GameBoard.Playerlist.Add(gameObject);
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

    void DisablePlayer()
    {
        //if (isLocalPlayer)
        //    mainCamera.SetActive(true);

        onToggleShared.Invoke(false);

        if (isLocalPlayer)
            onToggleLocal.Invoke(false);
        else
            onToggleRemote.Invoke(false);
    }

    void EnablePlayer()
    {
        //if (isLocalPlayer)
        //    mainCamera.SetActive(false);

        onToggleShared.Invoke(true);

        if (isLocalPlayer)
            onToggleLocal.Invoke(true);
        else
            onToggleRemote.Invoke(true);
    }

    private void Movement()
    {
        xVelocity = Input.GetAxis("Horizontal") * speed;
        yVelocity = Input.GetAxis("Vertical") * speed;

        float totalVelocity = Mathf.Abs(xVelocity) + Mathf.Abs(yVelocity);
        float xPercentage = totalVelocity == 0f ? 0 : xVelocity / totalVelocity;
        float yPrecentage = totalVelocity == 0f ? 0 : yVelocity / totalVelocity;

        xVelocity = speed * xPercentage;
        yVelocity = speed * yPrecentage;

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
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            builderScript.SwitchBuilding(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            builderScript.SwitchBuilding(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            builderScript.SwitchBuilding(3);
        }
    }

    private void MouseClick()
    {
        if (!Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0)) return;

        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, layerMask);

        if (!build && hit.collider != null && hit.collider.gameObject.layer == 9 && Input.GetMouseButtonUp(0))
        {
            SetSelectedBuilding(hit.collider.gameObject);
        }
        else if (hit.collider == null && Input.GetMouseButton(0))
        {
            if (buildingPlacement.activeSelf)
            {
                builderScript.CmdBuild(buildingPlacement.transform.position, builderScript.selectedBuilding);
                build = true;
                SetSelectedBuilding(null);
            }
        }
        else if (hit.collider == null || hit.collider.gameObject.layer != 9)
        {
            SetSelectedBuilding(null);
        }

        if (Input.GetMouseButtonUp(0))
        {
            build = false;
        }
    }

    private void SetSelectedBuilding(GameObject selected)
    {
        if (selected != null)
        {
            if (selected == selectedBuilding)
            {
                SetSelectedBuilding(null);
                return;
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
                selectedBuilding.transform.Find("Menu").gameObject.SetActive(false);
                selectedBuilding = null;
            }
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

    // Update is called once per frame
    void FixedUpdate () {

    }
}
