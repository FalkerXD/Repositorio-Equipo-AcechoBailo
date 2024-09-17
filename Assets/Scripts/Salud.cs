using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Si est�s usando una barra de salud visual.

public class Salud : MonoBehaviour
{
    [Header("Configuraci�n de Salud")]
    [Tooltip("La salud m�xima que tendr� el personaje o enemigo.")]
    public float saludMaxima = 100f;  // Salud m�xima determinada desde el Inspector.
    private float saludActual;         // Salud actual.

    [Header("Barra de Salud (Opcional)")]
    public Image barraDeSalud;         // Referencia a la barra de salud (UI).
    public Gradient gradienteDeSalud;  // Gradiente para cambiar el color de la barra.

    void Start()
    {
        saludActual = saludMaxima;  // Al iniciar, la salud es igual a la salud m�xima.
        ActualizarBarraDeSalud();
    }

    // M�todo para recibir da�o.
    public void RecibirDa�o(float cantidadDeDa�o)
    {
        saludActual -= cantidadDeDa�o;  // Reducir la salud.
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);  // Asegurar que la salud no sea menor que 0 ni mayor que la m�xima.
        ActualizarBarraDeSalud();

        if (saludActual <= 0)
        {
            Morir();  // Llamar a la funci�n de muerte si la salud llega a 0.
        }
    }

    // M�todo para curarse.
    public void Curar(float cantidadDeCura)
    {
        saludActual += cantidadDeCura;  // Aumentar la salud.
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);  // Asegurar que la salud no supere la m�xima.
        ActualizarBarraDeSalud();
    }

    // M�todo para actualizar la barra de salud visual.
    private void ActualizarBarraDeSalud()
    {
        if (barraDeSalud != null)
        {
            barraDeSalud.fillAmount = saludActual / saludMaxima;  // Actualizar la barra de salud.
            barraDeSalud.color = gradienteDeSalud.Evaluate(barraDeSalud.fillAmount);  // Cambiar el color seg�n el gradiente.
        }
    }

    // M�todo que se llama cuando el personaje o enemigo muere.
    private void Morir()
    {
        Debug.Log(gameObject.name + " ha muerto.");
        gameObject.SetActive(false);  // Desactivar el objeto al morir.
    }
}