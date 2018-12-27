using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPortraitManager : MonoBehaviour {

    public Image unitPortait;
    public Image unitHealth;
    public Image unitMana;

    private readonly int[] OverFifty = { 0, 255, 0 };
    private readonly int[] BelowFifty = { 255, 255, 0 };
    private readonly int[] BelowTwentyFive = { 255, 0, 0 };

    // Use this for initialization
    void Start () {
        HidePortrait();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdatePortrait()
    {
        var unit = UiManager.main.GetFirstUnit();
        if(unit == null)
        {
            HidePortrait();
            return;
        }
        if (!unitPortait.transform.parent.gameObject.activeSelf)
        {
            unitPortait.transform.parent.gameObject.SetActive(true);
        }
        RtsObject rtsObj = unit.GetComponent<RtsObject>();
        unitPortait.sprite = rtsObj.GetUnitImage();
        unitHealth.fillAmount = Utils.Map(rtsObj.currentHealth, rtsObj.health);
        unitMana.fillAmount = Utils.Map(rtsObj.currentHealth, rtsObj.health);
        UpdateUnitHealthText(rtsObj);
        ChangeHealthColor();
    }

    public void UpdatePortraitHealthAndMana(int currentHealth, int maxHealth, int currentMana, int maxMana)
    {
        unitHealth.fillAmount = Utils.Map(currentHealth, maxHealth);
        unitMana.fillAmount = Utils.Map(currentMana, maxMana);
        var unit = UiManager.main.GetFirstUnit();
        RtsObject rtsObj = unit.GetComponent<RtsObject>();
        UpdateUnitHealthText(rtsObj);
        ChangeHealthColor();

    }

    void ChangeHealthColor()
    {
        if(unitHealth.fillAmount >= 0.50f)
        {
            unitHealth.color = new Color(OverFifty[0], OverFifty[1], OverFifty[2]);
        } 
        else if(unitHealth.fillAmount >= 0.25f && unitHealth.fillAmount <= 0.49f)
        {
            unitHealth.color = new Color(BelowFifty[0], BelowFifty[1], BelowFifty[2]);
        }
        else
        {
            unitHealth.color = new Color(BelowTwentyFive[0], BelowTwentyFive[1], BelowTwentyFive[2]);
        }
    }

    void HidePortrait()
    {
        unitPortait.transform.parent.gameObject.SetActive(false);
    }

    void UpdateUnitHealthText(RtsObject rtsObj)
    {
        int currentHealth = rtsObj.currentHealth;
        int maxHealth = rtsObj.health;
        unitHealth.GetComponentInChildren<Text>().text = currentHealth + "/" + maxHealth;
    }
}
