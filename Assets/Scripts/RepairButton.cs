using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RepairButton : MonoBehaviour
{
    private Building building;// Start is called before the first frame update
    private Button button;
    public TextMeshProUGUI repairCost;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void setup(Building b)
    {
        building = b;
        button.onClick.AddListener(() => building.repairButton());
    }
    public void Update()
    {
        repairCost.text = building.getRepairCost().ToString();
        if (building.canAffordRepair())
        {
            repairCost.color = Color.yellow;
        }
        else
        {
            repairCost.color = Color.red;
        }
    }
}
