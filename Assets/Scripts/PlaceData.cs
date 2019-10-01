using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlaceData : MonoBehaviour
{
    public TextMeshProUGUI CostT;
    public TextMeshProUGUI CleanT;
    public TextMeshProUGUI DirtyT;
    public TextMeshProUGUI HappyT;

    public void SetThem(int Cost, float Clean, float Dirt, float Happy)
    {
        CostT.text = "$" + Cost.ToString();
        CleanT.text = Clean.ToString();
        DirtyT.text = Dirt.ToString();
        HappyT.text = Happy.ToString();
    }
}
