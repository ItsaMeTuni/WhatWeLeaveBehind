using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicySystem : MonoBehaviour
{
    [SerializeField] int optionCount = 3;
    [SerializeField] Color neutralEffectColor = Color.white;
    [SerializeField] Color badEffectColor = Color.red;
    [SerializeField] Color goodEffectColor = Color.green;

    [SerializeField] float randomDialogOpenTimeMin = 30;
    [SerializeField] float randomDialogOpenTimeMax = 60;
    [SerializeField] float dialogChooseTimeLimit = 25;

    [SerializeField] GameObject dialogGameObject;
    [SerializeField] GameObject dialogDarkenBgGameObject;
    [SerializeField] GameObject policyOptionsListGameObject;
    [SerializeField] GameObject policyOptionPrefab;

    List<Policy> policies = new List<Policy>();

    float nextDialogOpenTime;
    float dialogCloseTime;
    bool dialogOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        policies.AddRange(Resources.LoadAll<Policy>(""));

        CalcNextDialogOpenTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextDialogOpenTime && !dialogOpen)
        {
            OpenDialog();
        }

        if(Time.time >= dialogCloseTime && dialogOpen)
        {
            CloseDialog();
        }
    }


    void OpenDialog()
    {
        dialogGameObject.SetActive(true);
        dialogDarkenBgGameObject.SetActive(true);
        dialogOpen = true;

        //Create the options
        List<Policy> presentedOptions = new List<Policy>();

       
        for (int i = 0; i < optionCount; i++)
        {
            Policy option = policies[Random.Range(0, policies.Count)];
            if (presentedOptions.Contains(option))
            {
                //Try to find a different option
                i--;
                continue;
            }
            else
            {
                presentedOptions.Add(option);
            }
        }

        //Display them on the UI
        foreach (Policy policy in presentedOptions)
        {
            PolicyOption policyOption = Instantiate(policyOptionPrefab, policyOptionsListGameObject.transform).GetComponent<PolicyOption>();

            policyOption.icon.sprite = policy.icon;
            policyOption.title.text = policy.name;
            policyOption.description.text = policy.description;

            policyOption.money.text = policy.money.ToString();
            policyOption.money.color = GetEffectColor(policy.money);

            policyOption.earningRate.text = policy.earningRate.ToString("+#;-#;0");
            policyOption.earningRate.color = GetEffectColor(policy.earningRate);

            policyOption.satisfaction.text = policy.satisfaction.ToString("+#;-#;0");
            policyOption.satisfaction.color = GetEffectColor(policy.satisfaction);

            policyOption.emissionsClean.text = policy.emissionsClean.ToString("+#;-#;0");
            policyOption.emissionsClean.color = GetEffectColor(policy.emissionsClean);

            policyOption.emissionsDirty.text = policy.emissionsDirty.ToString("+#;-#;0");
            policyOption.emissionsDirty.color = GetEffectColor(policy.emissionsDirty, true);

            policyOption.requiredSatisfaction.text = policy.requiredSatisfaction.ToString();
            policyOption.bribeCost.text = policy.bribeCost.ToString();

            policyOption.propose.onClick.AddListener(() => { Propose(policy); });
            policyOption.bribe.onClick.AddListener(() => { Bribe(policy); });
        }
    }

    Color GetEffectColor(float value, bool invert = false)
    {
        if(invert)
        {
            value = -value;
        }

        if(value > 0)
        {
            return goodEffectColor;
        }
        else if (value < 0)
        {
            return badEffectColor;
        }
        else
        {
            return neutralEffectColor;
        }
    }

    void Propose(Policy policy)
    {
        ApplyPolicy(policy);
        CloseDialog();
    }

    void Bribe(Policy policy)
    {
        ApplyPolicy(policy);
        CloseDialog();
    }

    void ApplyPolicy(Policy policy)
    {
        Population.satisfactionLevel += policy.satisfaction;
        Economy.money += policy.money;
        Economy.earningRate += policy.earningRate;

        PollutionEffector pollutionEffector = (new GameObject("PolicyPollutionEffector")).AddComponent<PollutionEffector>();
        pollutionEffector.PollutionDirty = policy.emissionsDirty;
        pollutionEffector.PollutionClean = policy.emissionsClean;
    }

    void CloseDialog()
    {
        dialogOpen = false;
        dialogGameObject.SetActive(false);
        dialogDarkenBgGameObject.SetActive(false);
        CalcNextDialogOpenTime();

        for (int i = 0; i < policyOptionsListGameObject.transform.childCount; i++)
        {
            Destroy(policyOptionsListGameObject.transform.GetChild(i).gameObject);
        }
    }

    void CalcNextDialogOpenTime()
    {
        nextDialogOpenTime = Time.time + Random.Range(randomDialogOpenTimeMin, randomDialogOpenTimeMax);
        dialogCloseTime = Time.time + dialogChooseTimeLimit;
    }
}
