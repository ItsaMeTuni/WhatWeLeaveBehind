using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PolicyOption : MonoBehaviour
{
    public TMP_Text title;
    public Image icon;
    public TMP_Text description;

    public TMP_Text money;
    public TMP_Text earningRate;
    public TMP_Text satisfaction;
    public TMP_Text emissionsClean;
    public TMP_Text emissionsDirty;

    public TMP_Text requiredSatisfaction;
    public TMP_Text bribeCost;

    public Button propose;
    public Button bribe;
}
