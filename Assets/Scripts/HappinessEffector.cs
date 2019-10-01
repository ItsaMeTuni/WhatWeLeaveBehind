using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinessEffector : MonoBehaviour
{
   public float HappyIncrease;
    public float HappyDecrease;
    private void Start()
    {
        //    Population.instance.SubscribeToSatisfaction(this);
        Population.instance.AddSatisifaction(HappyIncrease);
        Population.instance.RemoveSatisfaction(HappyDecrease);
    }

    public void RunTick () {
     //   Population.instance.AddSatisifaction(HappyIncrease);
    }
}
