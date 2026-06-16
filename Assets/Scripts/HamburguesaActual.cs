using System.Collections.Generic;
using UnityEngine;

public class HamburguesaActual : MonoBehaviour
{
    [Header("Ingredientes actuales de la hamburguesa")]
    public List<int> ingredientes = new List<int>();

    public void AgregarIngrediente(int id)
    {
        ingredientes.Add(id);

        Debug.Log("Ingrediente agregado a la hamburguesa. ID: " + id);
    }

    public void LimpiarHamburguesa()
    {
        ingredientes.Clear();
    }
}