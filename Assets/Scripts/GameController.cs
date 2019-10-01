using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class GameController : MonoBehaviour
{
    private Population POP;
    private Economy EC;
    public static GameController Controller;
    public float TickSpeed;

    [Header("Pollution")]
    float pollutionLimit = 500;

    [Header("UI")]
    float pollutionLimitBarSafeSpace = 625;
    public TextMeshProUGUI PollutionAmmount;
    public Image PollutionDisplay;

    [Header("Pollution smoke")]
    [SerializeField] ParticleSystem pollutionSmokePS;
    [SerializeField] Gradient pollutionSmokeColorOverPollutionMin;
    [SerializeField] Gradient pollutionSmokeColorOverPollutionMax;

    [Header("Pollution post processing")]
    [SerializeField] PostProcessVolume pollutionPPV;

    [SerializeField, DisplayName("Post process weight over pollution")]
    AnimationCurve pollutionPPVWeightOverPollution;
    public GameObject EndGame;

    [SerializeField] List<GameObject> stuffToActivateOnGameBegin;

    public bool disableTutorial = false;

    private void Start()
    {
        Controller = this;

        POP = GetComponent<Population>();
        EC = GetComponent<Economy>();

        foreach (GameObject c in stuffToActivateOnGameBegin)
        {
            c.SetActive(false);
        }
        if (disableTutorial)
        {
            BeginGame();
            Destroy(FindObjectOfType<TutorialController>().gameObject);
        }
    }
    public void StartTick()
    {
        InvokeRepeating("RunGameTick", TickSpeed, TickSpeed);
    }

    public void BeginGame ()
    {
        foreach (GameObject c in stuffToActivateOnGameBegin)
        {
            c.SetActive(true);
        }
        StartTick();
        GTPollutionEffects();
    }

//anything that requires to be run off the game tick cycle should be hooked in here
    //this allows us to controll the speed of gameplay for balancing
    public void RunGameTick ()
    {
        //game tick pollution system
        GTPollution();

        //game tick income calculation system
        GTIncomeCalculation();

        //Population Tick
        POP.IncreasePopulation();

        //Economy tick
        EC.EarnMoney();

        //POpulation Happiness tick
        POP.GTSatisfaction();
        //pollution effects run
        GTPollutionEffects();
        //run AI place
        BuildingPlacer.BL.AIBuild();
    }

    public void GTIncomeCalculation ()
    {
        var INN = (Population.satisfactionLevel / Population.population) * 2;
        INN += (Pollution - PollLastT) * 2;
        INN = INN * 2;
        Economy.earningRate = INN;
    }

    /// <summary>
    /// POLUTION CODE BELOW
    /// Primary Gameplay loop
    /// 
    /// if pollution gets to high the game ends because of irrevesabl action to the climate
    /// is re-calculated every game tick
    /// </summary>
    public float Pollution;

    public float PollLastT;
    public List<PollutionEffector> POLF = new List<PollutionEffector> ();

    public void SubscribeToPollution (PollutionEffector ME)
    {
        POLF.Add(ME);
    }

    public void UnsubscribeFromPollution (PollutionEffector effector)
    {
        POLF.Remove(effector);
    }

    public void GTPollution ()
    {
        //do checks and reset for next tick
        if (Pollution >= pollutionLimit)
        {
            Debug.Log("You have destroyed the world you dooshbag, theres no coming back from this, look what youve left behind for us");
            //   SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            StopTick();
        }
        PollLastT = Pollution;
        Pollution = 0; 

        //tell everything to run tick
        foreach (PollutionEffector i in POLF)
        {
            i.RunTick();
        }

        //ran tick put it on the scoreboard
        PollutionAmmount.text = Pollution.ToString();
        PollutionDisplay.fillAmount = (Pollution / pollutionLimitBarSafeSpace);
      
    }

    public void AddPollution(float PollAm)
    {
        Pollution += PollAm;
        //increased pollution
    }

    public void RemovePollution(float RemovePol)
    {
        Pollution -= RemovePol;
    }

    void GTPollutionEffects()
    {
        float normalizedPollutionProgress = Pollution / pollutionLimit;

        var psMain = pollutionSmokePS.main;
        psMain.startColor = new ParticleSystem.MinMaxGradient(pollutionSmokeColorOverPollutionMin.Evaluate(normalizedPollutionProgress));
        psMain.startColor = new ParticleSystem.MinMaxGradient(pollutionSmokeColorOverPollutionMax.Evaluate(normalizedPollutionProgress));

        pollutionPPV.weight = pollutionPPVWeightOverPollution.Evaluate(normalizedPollutionProgress);
    }

    public void StopTick ()
    {
        CancelInvoke("RunGameTick");
        EndGame.SetActive(true);
    }

    public void LoadMenu ()
    {
        Application.LoadLevel(0);
    }
}
