using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionEffector : MonoBehaviour
{
    public float PollutionClean;
    public float PollutionDirty;
    private void Start()
    {
        GameController.Controller.SubscribeToPollution(this);
    }
    public void RunTick ()
    {
        DoPollution();
    }
    public void DoPollution ()
    {
        //display plus ang negative stuff above object displaying positive and negative effects
        GameController.Controller.AddPollution(PollutionDirty);
        GameController.Controller.RemovePollution(PollutionClean);
    }
    private void OnDestroy()
    {
        GameController.Controller.UnsubscribeFromPollution(this);
    }
}
