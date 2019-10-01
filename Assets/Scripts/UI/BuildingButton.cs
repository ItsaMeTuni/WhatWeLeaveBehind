using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButton : MonoBehaviour
{

    public GameObject GhostPrefab;
    public GameObject BuildPrefab;
    public int cost;

    public void SelectTing ()
    {
        BuildingPlacer.BL.SelectedBuilding = GhostPrefab;
        BuildingPlacer.BL.FinishedBuilding = BuildPrefab;

        if (BuildingPlacer.BL.GhostPlaced != null)
        {
            Destroy(BuildingPlacer.BL.GhostPlaced);
            BuildingPlacer.BL.GhostPlaced = null;
        }
        BuildingPlacer.BL.Building = true;
        BuildingPlacer.BL.CostBuilding = cost;
        
    }


}
