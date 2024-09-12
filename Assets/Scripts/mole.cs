using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Mole : MonoBehaviour
{
    public Transform jugador;  // Arrastra aquí el jugador desde el inspector
    public float radioPersecucion = 10f;  // Radio en el que el enemigo comenzará a perseguir
    public float radioAtaque = 1.5f;      // Radio en el que el enemigo atacará al jugador
    public float velocidadCaminando = 2f; // Velocidad de movimiento cuando el enemigo camina
    public float velocidadCorriendo = 6f; // Velocidad de movimiento durante el ataque
    public float intervaloAtaque = 5f;    // Intervalo entre ataques
    public float tiempoCongelado = 2f;    // Tiempo durante el cual el enemigo estará congelado
    public float correccionVertical = 0.1f; // Valor de corrección vertical para evitar deslizamientos

    private Animator animador;       // Referencia al Animator
    private Rigidbody rb;            // Referencia al Rigidbody del enemigo
    private bool preparandoAtaque = false; // Estado de preparación del ataque
    private Vector3 ultimaPosicion;  // Última posición registrada antes de iniciar la animación de ataque
    private float tiempoCongeladoRestante; // Tiempo restante para el congelamiento

    private void Start()
    {
        animador = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();  // Referencia al Rigidbody

        // Configurar el Rigidbody para bloquear el movimiento en el eje Y
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        StartCoroutine(RutinaAtaque());
    }

    private void Update()
    {
        if (preparandoAtaque)
        {
            // Mantener la posición exacta durante "PreparandoGolpe"
            transform.position = new Vector3(ultimaPosicion.x, transform.position.y, ultimaPosicion.z);

            // Decrementar el temporizador del tiempo de congelación
            tiempoCongeladoRestante -= Time.deltaTime;

            // Verificar si ha pasado el tiempo de congelación
            if (tiempoCongeladoRestante <= 0)
            {
                // Descongelar el movimiento
                rb.isKinematic = false;
                animador.SetBool("Corriendo", true); // Empezar a correr si se ha terminado la congelación
                preparandoAtaque = false; // Terminar la preparación del ataque
            }
            return; // Salir del Update para evitar cualquier movimiento
        }

        // Calcula la distancia entre el enemigo y el jugador
        float distanciaJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaJugador <= radioPersecucion)
        {
            if (distanciaJugador > radioAtaque)
            {
                PerseguirJugador();
            }
            else
            {
                // El jugador está dentro del radio de ataque
                if (!preparandoAtaque)
                {
                    animador.SetBool("Caminando", false);
                    animador.SetBool("Corriendo", false);
                    animador.SetTrigger("Golpear");
                }
            }
        }
        else
        {
            // El jugador está fuera del radio de detección
            animador.SetBool("Caminando", false);
            animador.SetBool("Corriendo", false);
            animador.SetBool("Quieto", true);
        }
    }

    private void PerseguirJugador()
    {
        Vector3 direccion = (jugador.position - transform.position).normalized;

        // Mantener la posición Y fija
        Vector3 nuevaPosicion = transform.position + direccion * velocidadCaminando * Time.deltaTime;
        nuevaPosicion.y = transform.position.y; // Mantener la altura original

        rb.MovePosition(nuevaPosicion);
        transform.LookAt(new Vector3(jugador.position.x, transform.position.y, jugador.position.z));

        animador.SetBool("Caminando", true);
        animador.SetBool("Quieto", false);
    }

    private IEnumerator RutinaAtaque()
    {
        while (true)
        {
            // Esperar el intervalo de tiempo antes de iniciar "PreparandoGolpe"
            yield return new WaitForSeconds(intervaloAtaque);

            // Verificar si el jugador está en el radio de persecución
            float distanciaJugador = Vector3.Distance(transform.position, jugador.position);

            if (distanciaJugador <= radioPersecucion)
            {
                // Iniciar "PreparandoGolpe"
                preparandoAtaque = true;
                ultimaPosicion = transform.position;  // Guardar la posición actual
                animador.SetTrigger("PreparandoGolpe");

                // Congelar el movimiento
                rb.isKinematic = true;

                // Establecer el tiempo restante para la congelación
                tiempoCongeladoRestante = tiempoCongelado;

                // Esperar a que termine la animación "PreparandoGolpe"
                yield return new WaitUntil(() => !animador.GetCurrentAnimatorStateInfo(0).IsName("PreparandoGolpe"));

                // Iniciar "Corriendo" hacia el jugador
                animador.SetBool("Corriendo", true);

                // Moverse hacia el jugador hasta el radio de ataque
                while (Vector3.Distance(transform.position, jugador.position) > radioAtaque)
                {
                    Vector3 direccion = (jugador.position - transform.position).normalized;

                    // Mantener la altura fija mientras corre
                    Vector3 nuevaPosicion = transform.position + direccion * velocidadCorriendo * Time.deltaTime;
                    nuevaPosicion.y = transform.position.y; // Mantener la altura original

                    rb.MovePosition(nuevaPosicion);
                    transform.LookAt(new Vector3(jugador.position.x, transform.position.y, jugador.position.z));
                    
                    // Corregir la posición vertical si es necesario
                    if (Mathf.Abs(transform.position.y - ultimaPosicion.y) > correccionVertical)
                    {
                        transform.position = new Vector3(transform.position.x, ultimaPosicion.y, transform.position.z);
                    }
                    
                    yield return null;
                }

                // Detener "Corriendo" y ejecutar "GolpeListo"
                animador.SetBool("Corriendo", false);
                animador.SetTrigger("GolpeListo");

                // Esperar a que termine la animación "GolpeListo"
                yield return new WaitUntil(() => !animador.GetCurrentAnimatorStateInfo(0).IsName("GolpeListo"));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioPersecucion);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radioAtaque);
    }
}