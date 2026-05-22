using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BombFrequency : BombMinigame
{
    
    [Header("UI Elements")]
    [SerializeField]
    private Slider freq_slider;
    [SerializeField]
    private Slider amp_slider;

    [Header("Screen Materials")] 
    [SerializeField]
    private Material targetMaterial;
    [SerializeField]
    private Material playerMaterial;

    [Header("Stats")] 
    [SerializeField]
    private float maxFreq = 20f;
    [SerializeField]
    private float minFreq = 3f;
    [SerializeField]
    private float maxAmp = 0.25f;
    [SerializeField]
    private float minAmp = 0.01f;
    
    [Header("Error Margins")]
    [SerializeField] 
    private float maxFreqOffset = 0f;
    [SerializeField] 
    private float maxAmpOffset = 0f;

    [Header("Runtime Info")] 
    [SerializeField]
    private bool freqOk = false;
    [SerializeField]
    private bool ampOk = false;
    [SerializeField]
    private bool isAdjustingFreq = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set needed EventListeners
        //SetEventListeners();
        
        //TODO: later to be replaced by the IOHandler
        //Frequency Setup
        freq_slider.maxValue = maxFreq;
        freq_slider.minValue = minFreq;
        freq_slider.value = playerMaterial.GetFloat("_waves_freq");;
        freq_slider.onValueChanged.AddListener(value => CheckIfSolved(true));
        
        //Amplitude Setup
        amp_slider.maxValue = maxAmp;
        amp_slider.minValue = minAmp;
        amp_slider.value = playerMaterial.GetFloat("_waves_freq");
        amp_slider.onValueChanged.AddListener(value => CheckIfSolved(true));
        
        //Set Margins
        maxFreqOffset = (maxFreq - minFreq) / 20; // (20-3) / 10 = 0.85
        maxAmpOffset = (maxAmp - minAmp) / 20;

        //StartMinigame();
        SwitchUISliders(false);
    }
    
    public override void SetEventListeners()
    {
        IOHandler.potentiometerUsed.AddListener(CheckIfSolved);
        IOHandler.buttonUsed.AddListener(SwitchBetweenFreqAndAmp);
    }

    public override void UnbindEventListeners()
    {
        IOHandler.potentiometerUsed.RemoveListener(CheckIfSolved);
        IOHandler.buttonUsed.RemoveListener(SwitchBetweenFreqAndAmp);
    }

    public override void StartMinigame()
    {
        StartFreqMinigame();
        this.isStarted = true;
    }

    public override async Awaitable<bool> EnterMinigame()
    {
        SetEventListeners();
        SwitchUISliders(true);
        while (!wantsToExit)
        {
            await Awaitable.NextFrameAsync();
        }

        wantsToExit = false;
        return true;
    }
    
    public override void ExitMinigame()
    {
        UnbindEventListeners();
        SwitchUISliders(false);
        wantsToExit = true;
    }

    //Needed this cause Events Suck 
    private void CheckIfSolved()
    {
        CheckIfSolved(false);
    }
    private void CheckIfSolved(bool usesUI)
    {
        if (!usesUI) //Used to skip IO Handler if no Arduino exists yet
        {
            if (isAdjustingFreq)
            {
                AdjustSliderValue(IOHandler.potentiometerInput ,ref freq_slider, minFreq, maxFreq);    
            }
            else
            {
                AdjustSliderValue(IOHandler.potentiometerInput, ref amp_slider, minAmp, maxAmp);
            }    
        }
        
        
        CheckValues(
            freq_slider.value, 
            targetMaterial.GetFloat("_waves_freq"), 
            maxFreqOffset, 
            ref freqOk);
        
        CheckValues(
            amp_slider.value, 
            targetMaterial.GetFloat("_waves_amp"), 
            maxAmpOffset, 
            ref ampOk);

        if (freqOk && ampOk)
        {
            Debug.Log("Frequency Minigame Complete!!");   
            ExitMinigame();
        }
    }

    private void AdjustSliderValue(float newVal, ref Slider slider, float toMin, float toMax)
    {
        slider.value = Remap(newVal, 0,100, toMin, toMax);
    }

    //Switch between Freq and Amp
    private void SwitchBetweenFreqAndAmp()
    {
        Debug.Log("Switching between Freq and Amp");
        isAdjustingFreq = !isAdjustingFreq;
    }

    private void SwitchUISliders(bool b)
    {
        freq_slider.interactable = b;
        amp_slider.interactable = b;
    }

    private void CheckValues(float val, float targetVal, float maxOffset ,ref bool isOk)
    {
        isOk = (val > targetVal - maxOffset && val < targetVal + maxOffset);
    }

    // Update is called once per frame
    private void StartFreqMinigame()
    {
        Debug.Log("Starting Frequency Minigame");
        GenerateRandomTarget();
    }


    private void GenerateRandomTarget()
    {
        targetMaterial.SetFloat("_waves_freq", Random.Range(minFreq, maxFreq));
        targetMaterial.SetFloat("_waves_amp", Random.Range(minAmp, maxAmp));
    }

    public void UpdateFreq()
    {
        playerMaterial.SetFloat("_waves_freq", freq_slider.value);
    }

    public void UpdateAmp()
    {
        playerMaterial.SetFloat("_waves_amp", amp_slider.value);
    }

    
    public float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        // InverseLerp returns a value between 0 and 1
        float t = Mathf.InverseLerp(fromMin, fromMax, value);
    
        // Lerp takes that 0-1 and projects it onto the new range
        return Mathf.Lerp(toMin, toMax, t);
    }
    
}
