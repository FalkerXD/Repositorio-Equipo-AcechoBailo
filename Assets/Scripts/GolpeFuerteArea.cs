using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpeFuerteArea : MonoBehaviour
{
    public float tiempoAntesDeDaño = 1f;   // Tiempo de espera antes de aplicar daño
    public float dañoGolpeFuerte = 20f;    // Daño del golpe fuerte
    public float tiempoActivoGolpe = 0.5f; // Duración del golpe
    private bool golpeFuerteActivado = false;
    private List<Collider> enemigosDentroDelArea = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo")) // Verifica si el objeto es un enemigo
        {
            enemigosDentroDelArea.Add(other); // Añadir el enemigo a la lista
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemigo")) // Cuando el enemigo sale del área
        {
            enemigosDentroDelArea.Remove(other); // Eliminar el enemigo de la lista
        }
    }

    public void IniciarGolpeFuerte()
    {
        golpeFuerteActivado = true;
        StartCoroutine(EsperarYAplicarDaño());
    }

    private IEnumerator EsperarYAplicarDaño()
    {
        yield return new WaitForSeconds(tiempoAntesDeDaño); // Espera antes de aplicar el daño

        if (golpeFuerteActivado)
        {
            foreach (Collider enemigo in enemigosDentroDelArea)
            {
                Salud saludEnemigo = enemigo.GetComponent<Salud>();
                if (saludEnemigo != null)
                {
                    saludEnemigo.RecibirDaño(dañoGolpeFuerte); // Aplica daño a cada enemigo dentro del área
                }
            }
        }
        golpeFuerteActivado = false; // Desactiva el golpe después de aplicar daño
    }

    public void DesactivarGolpeFuerte()
    {
        golpeFuerteActivado = false;
    }
}