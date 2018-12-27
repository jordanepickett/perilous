using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUiSender : MonoBehaviour {

    private Icommand command;
    private bool isReadyForLocation = false;

    public GameObject infoPanel;
    Text text;

    public Icommand GetCommand()
    {
        return command;
    }

    public void SetCommand(Icommand newCommand)
    {
        command = newCommand;
    }

    private Button button;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetUpCommand);
	}

    void Update()
    {
        if(Input.GetKeyDown(GetCommand().GetKeyBind()))
        {
            GetCommand().KeyBindCommand();
        }
    }

    void OnMouseOver()
    {
        infoPanel.SetActive(true);
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
    }

    void OnMouseExit()
    {
        infoPanel.SetActive(false);
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
    }

    void SetUpCommand()
    {
        command.InitializeCommand();
    }

    public void HoverInfo()
    {
        if(infoPanel)
        {
            text = infoPanel.GetComponentInChildren<Text>();
            DisplayInfo(text);
            infoPanel.SetActive(true);
        }
        
    }

    public void CloseHoverInfo()
    {
        if (infoPanel)
            infoPanel.SetActive(false);
    }

    void DisplayInfo(Text text)
    {
        Item item = command.GetUnit().GetComponent<Unit>().GetItem();
        text.text = item.Name + "\n" +
            "GOLD: " + item.Cost.ToString() + "\n" +
            "LUMBER: " + item.Lumber.ToString() + "\n" +
            "FOOD: " + item.food.ToString() + "\n \n" +
            "AVG DAMAGE: " + item.attack.damage.ToString();
    }
}
