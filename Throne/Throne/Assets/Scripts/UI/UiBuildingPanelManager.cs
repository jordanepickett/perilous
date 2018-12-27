using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildingPanelManager : MonoBehaviour {

    public static UiBuildingPanelManager main;
    private UiManager manager;
    public Image unitTimer;
    public Button[] unitQueue;

    [SerializeField]
    private Sprite[] btnSprites;

    private void Awake()
    {
        main = this;
        manager = UiManager.main;
        ClearQueue();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayUnitPanel();
    }

    void DisplayUnitPanel()
    {
        if (manager.GetFirstUnit())
        {
            if (manager.GetFirstUnit().GetComponent<Unit>().unitType == UnitType.Building && manager.GetFirstUnit().GetComponent<Unit>().GetState() == state.BUILDING)
            {
                unitTimer.transform.parent.gameObject.SetActive(true);
                var filler = Utils.Map(manager.GetFirstUnit().GetComponent<Unit>().GetBuildingTime(), manager.GetFirstUnit().GetComponent<Unit>().GetBuildSeconds());
                unitTimer.fillAmount = filler;
                if (filler >= 1)
                {
                    unitTimer.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }

    public void DisplayQueueUI(List<Icommand> queueCommands)
    {
        ClearQueue();
        if (queueCommands.Count > 0)
        {
            for(int i = 0; i < queueCommands.Count; i++)
            {
                Button btn = unitQueue[i];
                Icommand command = queueCommands[i];
                Image btnImage = btn.GetComponent<Image>();
                btn.interactable = true;
                btnImage.sprite = command.GetUnit().GetComponent<RtsObject>().GetUnitImage();
                btn.onClick.AddListener(delegate { CancelQueue(i); });
            }
        }
    }

    void ClearQueue()
    {
        for(int i = 0; i < unitQueue.Length; i++)
        {
            Button btn = unitQueue[i];
            Image btnImage = btn.GetComponent<Image>();
            btn.interactable = false;
            btnImage.sprite = btnSprites[0];
        }
    }

    public void CancelQueue(int index)
    {
        SelectableUnit selectedUnit = SelectionManager.main.FirstUnit();
        selectedUnit.GetComponent<Unit>().RemoveCommandFromQueue(index);
    }
}
