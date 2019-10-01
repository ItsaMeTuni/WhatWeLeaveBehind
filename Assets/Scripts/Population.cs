using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Population : MonoBehaviour
{
    public static Population instance;

    public float startingPopulation;
    public float startingSatisfactionLevel = 50;

    public static float population;
    public static float satisfactionLevel;

    //Should maybe be changed to a polled value
    //based on other game metrics like pollution level, etc
    public float birthRate;
    public float Happy;
    [SerializeField] private Text populationText;
    [SerializeField] private Text SatisfactionText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        population = startingPopulation;
        satisfactionLevel = startingSatisfactionLevel;

      //  InvokeRepeating("IncreasePopulation", 1, 1);
    }

   public void IncreasePopulation()
    {
        population += birthRate;
    }

    // Update is called once per frame
    void Update()
    {
        populationText.text = ((int)population).ToString();
        SatisfactionText.text = ((int)satisfactionLevel).ToString() + "%";
    }




    public List<HappinessEffector> POLF = new List<HappinessEffector>();
    //UI FOR THIS
   // public TextMeshProUGUI HappyTT;
  //  public Image PollutionDisplay;

  
    public void GTSatisfaction()
    {

        var FV = Happy / 3;
        FV = Mathf.Clamp(FV, 0, 100);
        satisfactionLevel = FV;

       

       
      
     //   PollutionDisplay.fillAmount = (Pollution / 625);

    }

    public void AddSatisifaction(float PollAm)
    {
        Happy += PollAm;
        //increased pollution
    }
    public void RemoveSatisfaction(float RemovePol)
    {
        Happy -= RemovePol;
    }
}