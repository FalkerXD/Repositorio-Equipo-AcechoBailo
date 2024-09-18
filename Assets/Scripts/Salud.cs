using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salud : MonoBehaviour
{
    public float saludMaxima = 100f;  // Salud máxima del enemigo
    public float saludActual;         // Salud actual del enemigo, visible en el Inspector
    public GameObject efectoMuerte;   // Efecto visual o de partículas para la muerte
    public bool desactivarEnemigo = true;  // Opción para desactivar el GameObject en lugar de destruirlo

    void Start()
    {
        // Inicializa la salud actual con la salud máxima
        saludActual = saludMaxima;
    }

    // Método para aplicar daño
    public void RecibirDaño(float cantidad)
    {
        saludActual -= cantidad;  // Reducir la salud actual por la cantidad de daño recibido
        saludActual = Mathf.Clamp(saludActual, 0f, saludMaxima);  // Asegurarse de que la salud no sea menor que 0

        // Verificar si la salud ha llegado a 0 o menos
        if (saludActual <= 0f)
        {
            Morir();
        }

        // Aquí podrías agregar otras lógicas relacionadas con la muerte o efectos visuales
        Debug.Log($"Salud actual: {saludActual}"); // Opcional: Log para verificar en la consola
    }

    // Método para manejar la muerte del enemigo
    void Morir()
    {
        // Reproducir efecto de muerte si se ha asignado uno
        if (efectoMuerte != null)
        {
            Instantiate(efectoMuerte, transform.position, transform.rotation);
        }

        // Desactivar el GameObject si la opción está habilitada
        if (desactivarEnemigo)
        {
            gameObject.SetActive(false);
        }
        else
        {
            // Alternativamente, destruir el GameObject
            Destroy(gameObject);
        }
    }
}