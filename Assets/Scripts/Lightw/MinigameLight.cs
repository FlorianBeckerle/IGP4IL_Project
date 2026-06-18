using UnityEngine;

public class MinigameLight : MonoBehaviour
{
    
    [Header("Visual")]
    [SerializeField]
    private MeshRenderer _meshRenderer;
    
    [SerializeField]
    private Material _onMaterial;
    [SerializeField]
    private Material _offMaterial;
    
    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource;

    public void TurnOn()
    {
        SwitchMaterial(true);
        audioSource.Play();
    }

    public void TurnOff()
    {
        SwitchMaterial(false);
    }


    private void SwitchMaterial(bool isOn)
    {
        Material swtichTo = isOn ? _onMaterial : _offMaterial;
        
        _meshRenderer.material = swtichTo;
    }
}
