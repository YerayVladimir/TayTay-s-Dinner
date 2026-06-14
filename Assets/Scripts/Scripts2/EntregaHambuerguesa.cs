using UnityEngine;

public class EntregaHamburguesa :
    MonoBehaviour
{
    public HamburguesaActual
        hamburguesaActual;

    public bool listaParaEntregar;

    void Update()
    {
        if
        (hamburguesaActual
         .ingredientes.Count >= 3)
        {
            listaParaEntregar = true;
        }
    }
}