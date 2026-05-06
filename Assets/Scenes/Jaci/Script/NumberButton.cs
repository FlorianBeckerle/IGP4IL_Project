using UnityEngine;

public class NumberButton : MonoBehaviour
{
    [SerializeField] private string number;
    [SerializeField] private KeyPasswordGame passwordGame;

    private void OnMouseDown()
    {
        passwordGame.PressNumber(number);
    }
}