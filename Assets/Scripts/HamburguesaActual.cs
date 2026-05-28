using System.Collections.Generic;
using UnityEngine;

public class HamburguesaActual : MonoBehaviour
{
    public List<int> ingredientes = new List<int>();

    public void agregarIngrediente(int id)
    {
        ingredientes.Add(id);
        Debug.Log("Agregado: " + id);
    }

    public void limpiarHamburguesa()
    {
        ingredientes.Clear();
        Debug.Log("Hamburguesa limpia");
    }
}