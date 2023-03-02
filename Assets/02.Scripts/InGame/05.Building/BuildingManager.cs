using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<Building> buildings;

    void Awake()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].Setup();
            for (int j = 0; j < buildings[i].structures.Count; j++)
            {
                buildings[i].structures[j].Setup(buildings[i]);
            }
        }

    }

    void Update()
    {
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].Updated();
            for (int j = 0; j < buildings[i].structures.Count; j++)
            {
                buildings[i].structures[j].Updated();
            }
        }
    }
}
