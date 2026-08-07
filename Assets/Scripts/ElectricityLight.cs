using UnityEngine;

public class ElectricityLight : MonoBehaviour
{
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private Color defaultColor = Color.white;

    private Material mat;

    private void Awake()
    {
        mat = meshRenderer.material;
        mat.color = defaultColor;
    }

    public void TurnRed()
    {
        SetColor(Color.red);
    }

    public void TurnGreen()
    {
        SetColor(Color.green);
    }

    private void SetColor(Color newColor)
    {
        mat.color = newColor;
        defaultColor = newColor;
    }
}