using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Si estás usando una barra de salud visual.

public class Salud : MonoBehaviour
{
    [Header("Configuración de Salud")]
    [Tooltip("La salud máxima que tendrá el personaje o enemigo.")]
    public float saludMaxima = 100f;  // Salud máxima determinada desde el Inspector.
    private float saludActual;         // Salud actual.

    [Header("Barra de Salud (Opcional)")]
    public Image barraDeSalud;         // Referencia a la barra de salud (UI).
    public Gradient gradienteDeSalud;  // Gradiente para cambiar el color de la barra.

    void Start()
    {
        saludActual = saludMaxima;  // Al iniciar, la salud es igual a la salud máxima.
        ActualizarBarraDeSalud();
    }

    // Método para recibir daño.
    public void RecibirDaño(float cantidadDeDaño)
    {
        saludActual -= cantidadDeDaño;  // Reducir la salud.
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);  // Asegurar que la salud no sea menor que 0 ni mayor que la máxima.
        ActualizarBarraDeSalud();

        if (saludActual <= 0)
        {
            Morir();  // Llamar a la función de muerte si la salud llega a 0.
        }
    }

    // Método para curarse.
    public void Curar(float cantidadDeCura)
    {
        saludActual += cantidadDeCura;  // Aumentar la salud.
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);  // Asegurar que la salud no supere la máxima.
        ActualizarBarraDeSalud();
    }

    // Método para actualizar la barra de salud visual.
    private void ActualizarBarraDeSalud()
    {
        if (barraDeSalud != null)
        {
            barraDeSalud.fillAmount = saludActual / saludMaxima;  // Actualizar la barra de salud.
            barraDeSalud.color = gradienteDeSalud.Evaluate(barraDeSalud.fillAmount);  // Cambiar el color según el gradiente.
        }
    }

    // Método que se llama cuando el personaje o enemigo muere.
    private void Morir()
    {
        Debug.Log(gameObject.name + " ha muerto.");
        gameObject.SetActive(false);  // Desactivar el objeto al morir.
    }
}