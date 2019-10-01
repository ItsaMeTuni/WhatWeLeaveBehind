using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionWaypoint : MonoBehaviour
{
    [EnumMaskAttribute(alwaysFoldOut = true)]
    public Direction connectedRoads;
}
