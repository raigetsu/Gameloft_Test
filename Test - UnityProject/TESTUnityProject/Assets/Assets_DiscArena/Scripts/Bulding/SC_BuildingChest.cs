using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_BuildingChest : SC_BuildingMaster
{
    public override void BuildingDestroy()
    {
        base.BuildingDestroy();

        print("Victory");
    }
}
