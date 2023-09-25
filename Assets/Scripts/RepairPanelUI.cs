using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RepairPanelUI : MonoBehaviour
{
    public GameObject repairPanel;
    public Transform target;
    public RepairButton buttonPrefab;
    private Building building;
    private RepairButton repairButton;

    Transform ui;
    Transform cam;
    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main.transform;
        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                building = GetComponent<Building>();
                ui = Instantiate(repairPanel, c.transform).transform;
                ui.gameObject.SetActive(true);
                repairButton = Instantiate(buttonPrefab, ui);
                repairButton.setup(building);
                break;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            Destroy(ui.gameObject);
            ui.gameObject.SetActive(false);
        }
        if (ui != null)
        {
            ui.position = target.position;
            ui.forward = cam.forward;

            if (building.atFullHP()||gameManager.current.phase==gameManager.Phase.DEFENSE)
            {
                repairButton.gameObject.SetActive(false);
            }
            else
            {
                repairButton.gameObject.SetActive(true);
            }
        }

    }
    private void OnDestroy()
    {
        try
        {
            Destroy(ui.gameObject);
        }
        catch (Exception e)
        {

        }

    }
}
