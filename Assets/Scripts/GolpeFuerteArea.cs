using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpeFuerteArea : MonoBehaviour
{
    public float tiempoAntesDeDaño = 1f; // Tiempo que debe esperar antes de hacer daño
    public float dañoGolpeFuerte = 20f; // Daño del golpe fuerte

    private bool golpeFuerteActivado = false;
    private float tiempoDeActivacion;

    private void OnTriggerEnter(Collider other)
    {
        if (golpeFuerteActivado && other.CompareTag("Enemigo")) // Ajusta el tag según sea necesario
        {
            Salud saludEnemigo = other.GetComponent<Salud>();
            if (saludEnemigo != null)
            {
                saludEnemigo.RecibirDaño(dañoGolpeFuerte);
                golpeFuerteActivado = false; // Desactiva el golpe fuerte después de hacer daño
            }
        }
    }

    public void IniciarGolpeFuerte()
    {
        golpeFuerteActivado = true;
        tiempoDeActivacion = Time.time;
        StartCoroutine(EsperarYAplicarDaño());
    }

    private IEnumerator EsperarYAplicarDaño()
    {
        yield return new WaitForSeconds(tiempoAntesDeDaño);
        // Aquí puedes activar el daño si es necesario.
    }
}