using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct TutorialStep
{
    [TextArea]
    public string text;
    public List<GameObject> activate;
    public List<GameObject> deactivate;
}

public class TutorialController : MonoBehaviour
{
    public List<TutorialStep> steps = new List<TutorialStep>();

    public TMP_Text text;

    int currStep = -1;

    public float gibberishSfxDurationMin;
    public float gibberishSfxDurationMax;

    AmbientSfxController sfxController;
    float stopSfxControllerTime;

    // Start is called before the first frame update
    void Start()
    {
        sfxController = GetComponent<AmbientSfxController>();
        
        Next();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= stopSfxControllerTime)
        {
            sfxController.Stop();
        }
    }

    public void Next()
    {
        currStep++;

        if (currStep < steps.Count)
        {
            text.text = steps[currStep].text;

            //Activate objects
            for(int i = 0; i < steps[currStep].activate.Count; i++)
            {
                steps[currStep].activate[i].SetActive(true);
            }

            //Deactivate objects
            for (int i = 0; i < steps[currStep].deactivate.Count; i++)
            {
                steps[currStep].deactivate[i].SetActive(false);
            }

            sfxController.Play();
            stopSfxControllerTime = Time.time + Random.Range(gibberishSfxDurationMin, gibberishSfxDurationMax);
        }
        else
        {
            GameController.Controller.BeginGame();
            Destroy(gameObject);
        }
    }
}
