using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Systems/Policy", fileName = "New Policy")]
public class Policy : ScriptableObject
{
    //How much money is this policy gonna cost (negative number)
    //or how much will the player earn with it (positive number)
    public float money;

    //How is this policy going to affect the amount of money the player
    //gets every second
    public float earningRate;

    //How much citizen satisfaction will the player win/loose with
    //this policy
    public float satisfaction;

    //How much satisfaction the player needs for this policy
    //to be approved?
    public float requiredSatisfaction;

    //How much money the player has to pay in order to bribe
    //the city council and approve the policy if the current 
    //satisfaction level is not high enough
    public float bribeCost;

    public float emissionsClean;
    public float emissionsDirty;

    //UI stuff
    public Sprite icon;
    public string name;

    [TextArea]
    public string description;
}
