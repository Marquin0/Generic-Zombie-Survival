using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnToolTip : MonoBehaviour {

    private GameObject toolTip;
    public string title, costs, life, dmg, range;
    private Text titleTxt, costsTxt, lifeTxt, dmgTxt, rangeTxt;
    
    public void SetTooltip(GameObject toolTip)
    {
        this.toolTip = toolTip;
        titleTxt = toolTip.transform.Find("TitleTxt").GetComponent<Text>();
        costsTxt = toolTip.transform.Find("CostsTxt").GetComponent<Text>();
        lifeTxt = toolTip.transform.Find("LifeTxt").GetComponent<Text>();
        dmgTxt = toolTip.transform.Find("DmgTxt").GetComponent<Text>();
        rangeTxt = toolTip.transform.Find("RangeTxt").GetComponent<Text>();
    }

    public void Show()
    {
        toolTip.SetActive(true);
        titleTxt.text = title;
        costsTxt.text = "Costs: " + costs;
        lifeTxt.text = "Life: " + life;
        if (!string.IsNullOrEmpty(dmg))
            dmgTxt.text = "Dmg: " + dmg;
        else
            dmgTxt.text = "";
        if (!string.IsNullOrEmpty(range))
            rangeTxt.text = "Range: " + range;
        else
            rangeTxt.text = "";

        toolTip.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y+0.3f);
    }

    public void Hide()
    {
        toolTip.SetActive(false);
    }
}
