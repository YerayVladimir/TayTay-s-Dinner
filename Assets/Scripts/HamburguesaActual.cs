using System.Collections.Generic;
using UnityEngine;

public class HamburguesaActual : MonoBehaviour
{
    public List<int> ingredientes = new List<int>();

    public void AgregarIngrediente(int id)
    {
        ingredientes.Add(id);
        Debug.Log("Agregado: " + id);
    }

    public void LimpiarHamburguesa()
    {
        ingredientes.Clear();
        Debug.Log("Hamburguesa limpia");
    }
}
