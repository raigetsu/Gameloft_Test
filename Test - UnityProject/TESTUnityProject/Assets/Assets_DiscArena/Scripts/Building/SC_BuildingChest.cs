using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SC_BuildingChest : SC_BuildingMaster
{
    [SerializeField] public UnityEvent OnChestDestroy = new UnityEvent();

    public override void BuildingDestroy()
    {
        base.BuildingDestroy();
        OnChestDestroy?.Invoke();
    }
}
