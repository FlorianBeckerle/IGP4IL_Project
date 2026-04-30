using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BombFrequency : MonoBehaviour
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
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Frequency Setup
        freq_slider.maxValue = maxFreq;
        freq_slider.minValue = minFreq;
        freq_slider.value = playerMaterial.GetFloat("_waves_freq");;
        
        //Amplitude Setup
        amp_slider.maxValue = maxAmp;
        amp_slider.minValue = minAmp;
        amp_slider.value = playerMaterial.GetFloat("_waves_freq");
        
        //Set Margins
        maxFreqOffset = (maxFreq - minFreq) / 20; // (20-3) / 10 = 0.85
        maxAmpOffset = (maxAmp - minAmp) / 20;

        StartCoroutine(StartFreqMinigame());
    }

    // Update is called once per frame
    private IEnumerator StartFreqMinigame()
    {
        GenerateRandomTarget();
        while (!(freqOk && ampOk))
        {
            float val = freq_slider.value;
            float targetVal = targetMaterial.GetFloat("_waves_freq");
            if (val > targetVal - maxFreqOffset || val < targetVal + maxFreqOffset)
            {
                freqOk = true;
            }
            else
            {
                freqOk = false;
            }
        
        
            val = amp_slider.value;
            targetVal = targetMaterial.GetFloat("_waves_amp");
            if (val > targetVal - maxAmpOffset || val < targetVal + maxAmpOffset)
            {
                ampOk = true;
            }
            else
            {
                ampOk = false;
            }

            yield return new WaitForFixedUpdate();
        }
        
        Debug.Log("Frequency Minigame Complete!!");    
        
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
}
