using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpeFuerteArea : MonoBehaviour
{
    public float tiempoAntesDeDa�o = 1f; // Tiempo que debe esperar antes de hacer da�o
    public float da�oGolpeFuerte = 20f; // Da�o del golpe fuerte

    private bool golpeFuerteActivado = false;
    private float tiempoDeActivacion;

    private void OnTriggerEnter(Collider other)
    {
        if (golpeFuerteActivado && other.CompareTag("Enemigo")) // Ajusta el tag seg�n sea necesario
        {
            Salud saludEnemigo = other.GetComponent<Salud>();
            if (saludEnemigo != null)
            {
                saludEnemigo.RecibirDa�o(da�oGolpeFuerte);
                golpeFuerteActivado = false; // Desactiva el golpe fuerte despu�s de hacer da�o
            }
        }
    }

    public void IniciarGolpeFuerte()
    {
        golpeFuerteActivado = true;
        tiempoDeActivacion = Time.time;
        StartCoroutine(EsperarYAplicarDa�o());
    }

    private IEnumerator EsperarYAplicarDa�o()
    {
        yield return new WaitForSeconds(tiempoAntesDeDa�o);
        // Aqu� puedes activar el da�o si es necesario.
    }
}