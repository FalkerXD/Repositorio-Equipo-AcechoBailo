using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpeFuerteArea : MonoBehaviour
{
    public float tiempoAntesDeDa�o = 1f;   // Tiempo de espera antes de aplicar da�o
    public float da�oGolpeFuerte = 20f;    // Da�o del golpe fuerte
    public float tiempoActivoGolpe = 0.5f; // Duraci�n del golpe
    private bool golpeFuerteActivado = false;
    private List<Collider> enemigosDentroDelArea = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo")) // Verifica si el objeto es un enemigo
        {
            enemigosDentroDelArea.Add(other); // A�adir el enemigo a la lista
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemigo")) // Cuando el enemigo sale del �rea
        {
            enemigosDentroDelArea.Remove(other); // Eliminar el enemigo de la lista
        }
    }

    public void IniciarGolpeFuerte()
    {
        golpeFuerteActivado = true;
        StartCoroutine(EsperarYAplicarDa�o());
    }

    private IEnumerator EsperarYAplicarDa�o()
    {
        yield return new WaitForSeconds(tiempoAntesDeDa�o); // Espera antes de aplicar el da�o

        if (golpeFuerteActivado)
        {
            foreach (Collider enemigo in enemigosDentroDelArea)
            {
                Salud saludEnemigo = enemigo.GetComponent<Salud>();
                if (saludEnemigo != null)
                {
                    saludEnemigo.RecibirDa�o(da�oGolpeFuerte); // Aplica da�o a cada enemigo dentro del �rea
                }
            }
        }
        golpeFuerteActivado = false; // Desactiva el golpe despu�s de aplicar da�o
    }

    public void DesactivarGolpeFuerte()
    {
        golpeFuerteActivado = false;
    }
}