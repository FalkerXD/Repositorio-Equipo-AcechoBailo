using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Misil : MonoBehaviour
{
    private Animator animator;
    private bool isLaunched = false;

    // Tiempo en segundos para esperar antes de desactivar y activar los GameObjects
    public float tiempoDeEspera = 1f; // Ajusta este valor según la duración de tu animación

    // Listas de GameObjects a activar y desactivar
    public GameObject[] gameObjectsParaDesactivar;
    public GameObject[] gameObjectsParaActivar;

    void Start()
    {
        // Obtén el componente Animator del misil
        animator = GetComponent<Animator>();
    }

    // Método para lanzar el misil desde el script de movimiento
    public void LanzarMisil()
    {
        if (!isLaunched) // Evitar múltiples activaciones
        {
            // Reproduce la animación del misil
            animator.SetTrigger("Misil");
            isLaunched = true;

            // Usa una corrutina para esperar el tiempo especificado antes de activar y desactivar los GameObjects
            StartCoroutine(EsperarYActualizarGameObjects());
        }
    }

    // Corrutina para esperar el tiempo especificado antes de desactivar y activar los GameObjects
    IEnumerator EsperarYActualizarGameObjects()
    {
        // Espera el tiempo especificado en segundos
        yield return new WaitForSeconds(tiempoDeEspera);

        // Desactiva los GameObjects
        foreach (GameObject obj in gameObjectsParaDesactivar)
        {
            obj.SetActive(false);
        }

        // Activa los GameObjects después de desactivar
        foreach (GameObject obj in gameObjectsParaActivar)
        {
            obj.SetActive(true);
        }

        // Resetear el estado del misil para permitir lanzamientos futuros
        isLaunched = false;
    }
}