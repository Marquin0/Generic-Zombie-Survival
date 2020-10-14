using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BuildingSelector {

    private Building[] availableBuidlings;
    private int currentSelection;

    public BuildingSelector(Building[] buildings)
    {
        availableBuidlings = buildings;
    }

    public void SetSelection(int index)
    {
        currentSelection = index;
    }

    public int GetCurrentSelectionIndex()
    {
        return currentSelection;
    }

    public Building GetSelectedBuilding()
    {
        return availableBuidlings[currentSelection];
    }

    public Building GetBuildingWithIndex(int index)
    {
        return availableBuidlings[index];
    }
}
