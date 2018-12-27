using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementTrigger : MonoBehaviour {

    private List<Collider> colliders = new List<Collider>();

    private Renderer rend;

    public Color clear;

    public Color notClear;

    public bool isMainBuilding = false;

    public bool closeToMines = false;

    public static int MAX_GOLD_MINE_DISTANCE = 6;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public List<Collider> GetColliders()
    {
        return colliders;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Ground")
        {
            colliders.Add(other);
            rend.material.SetColor("_Color", notClear);
        }
        Debug.Log("Object entered the trigger");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Ground")
        {
            colliders.Remove(other);
            if(colliders.Count == 0)
            {
                rend.material.SetColor("_Color", clear);
            }
        }
        Debug.Log("Object exited the trigger");
    }

    private void Update()
    {
        if (isMainBuilding == true)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, MAX_GOLD_MINE_DISTANCE);
            foreach (var collider in hitColliders)
            {
                if (collider.gameObject.GetComponent<GatherNode>() && collider.gameObject.GetComponent<GatherNode>().gatherType == GatherType.GOLD)
                {
                    rend.material.SetColor("_Color", notClear);
                    closeToMines = true;
                    break;
                }
                closeToMines = false;
            }
            if (!closeToMines && colliders.Count == 0)
            {
                rend.material.SetColor("_Color", clear);
            }
        }
    }

}
