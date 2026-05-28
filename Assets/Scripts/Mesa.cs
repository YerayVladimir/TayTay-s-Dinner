using UnityEngine;
using TMPro;

public class Mesa : MonoBehaviour
{
    public TextMeshProUGUI textoOrden; // 👈 ESTE ES EL CAMBIO

    public void MostrarOrden(int numero)
    {
        textoOrden.text = "Orden #" + numero;
    }
}