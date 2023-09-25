using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderSelect : Selectable
{
    private UIManager ui;
    protected override void Awake()
    {
        ui = FindObjectOfType<UIManager>();
        base.Awake();
    }
    public override void onSingleSelect()
    {
        ui.showShop();   
    }
    public override void onDeselect()
    {
        ui.hideShop();
    }
}
