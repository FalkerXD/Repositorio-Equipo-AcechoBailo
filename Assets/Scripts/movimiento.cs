using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float speed = 5f;              // Velocidad de movimiento horizontal
    public float jumpHeight = 2f;        // Altura del salto
    public float gravity = 9.81f;        // Magnitud de la gravedad
    public float rotationSpeed = 700f;   // Velocidad de rotaci�n
    public GameObject nuevaBalaPrefab;   // Prefab de la nueva bala
    public Transform disparoPunto;        // Punto desde el cual se dispara la bala
    public float intervaloDisparo = 1f;   // Intervalo de tiempo entre disparos
    public GameObject animMisilObject;    // Referencia al GameObject que contiene la animaci�n del misil

    public GolpeNormal golpeNormal;       // Referencia al �rea de golpe normal
    public GolpeFuerteArea golpeFuerteArea; // Referencia al �rea de golpe fuerte
    private Misil misilScript;            // Referencia al script Misil

    private CharacterController controller;
    private Animator animator;
    private float tiempoUltimoDisparo = -1f;

    private Vector3 velocity;
    private bool isGrounded;

    private bool golpeNormalActivo = false; // Estado del golpe normal

    void Start()
    {
        // Obt�n el componente CharacterController
        controller = GetComponent<CharacterController>();
        // Obt�n el componente Animator
        animator = GetComponent<Animator>();
        // Obt�n el componente Misil del GameObject que contiene la animaci�n
        if (animMisilObject != null)
        {
            misilScript = animMisilObject.GetComponent<Misil>();
        }
    }

    void Update()
    {
        // Verifica si el jugador est� en el suelo
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Un valor peque�o para mantener el personaje en el suelo
        }

        // Verifica el movimiento del jugador
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Verifica si el jugador est� movi�ndose
        bool isMoving = move != Vector3.zero;
        animator.SetBool("correr", isMoving);

        // Maneja el salto y la animaci�n de salto
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Aplica una velocidad vertical para el salto
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            animator.SetTrigger("salto");
        }

        // Aplica la gravedad
        velocity.y -= gravity * Time.deltaTime;

        // Mueve al jugador
        controller.Move(move * speed * Time.deltaTime + velocity * Time.deltaTime);

        // Calcula la rotaci�n deseada en funci�n del movimiento
        if (isMoving)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Controles de combate
        HandleCombat();

        // Dispara la nueva bala cuando se presiona la tecla "Q" y se respeta el intervalo de disparo
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= tiempoUltimoDisparo + intervaloDisparo && nuevaBalaPrefab != null && disparoPunto != null)
        {
            DispararNuevaBala();
            tiempoUltimoDisparo = Time.time;
        }

        // Activa la animaci�n "Misil" cuando se presiona la tecla "E"
        if (Input.GetKeyDown(KeyCode.E) && misilScript != null)
        {
            misilScript.LanzarMisil();
        }
    }

    void HandleCombat()
    {
        // Golpe normal (click izquierdo)
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            animator.SetBool("golpear", true);
            golpeNormalActivo = true;
            if (golpeNormal != null)
            {
                golpeNormal.ActivarGolpeNormal();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("golpear", false);
            golpeNormalActivo = false;
            if (golpeNormal != null)
            {
                golpeNormal.DesactivarGolpeNormal();
            }
        }

        // Golpe especial (click derecho)
        if (Input.GetMouseButtonDown(1)) // Click derecho
        {
            animator.SetTrigger("preparando golpe");

            // Iniciar el golpe fuerte
            if (golpeFuerteArea != null)
            {
                golpeFuerteArea.IniciarGolpeFuerte();
            }
        }
    }

    void DispararNuevaBala()
    {
        if (nuevaBalaPrefab != null && disparoPunto != null)
        {
            GameObject bala = Instantiate(nuevaBalaPrefab, disparoPunto.position, disparoPunto.rotation);
            Debug.Log("Bala instanciada en: " + disparoPunto.position);

            // Aseg�rate de que la bala tenga un Rigidbody y est� configurado correctamente
            Rigidbody balaRb = bala.GetComponent<Rigidbody>();
            if (balaRb != null)
            {
                balaRb.velocity = bala.transform.forward * 10f; // Ajusta la velocidad como necesites
            }

            // Activa la animaci�n "Lanzo"
            animator.SetTrigger("Lanzo");
        }
    }
}