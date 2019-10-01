using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEnable : MonoBehaviour
{
    public Animator ST;
    // Start is called before the first frame update
    void Start()
    {
        ST.SetTrigger("GO");
    }

   
}
