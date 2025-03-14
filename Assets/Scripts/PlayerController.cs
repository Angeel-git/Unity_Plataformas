using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Valores Configurables")]
    [SerializeField] private float velocidad;
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private bool saltoMejorado;
    [SerializeField] private float saltoLargo = 1.5f; // Multiplicador para aumentar la gravedad cuando el jugador cae
    [SerializeField] private float saltoCorto = 1f;   // Multiplicador para aumentar la gravedad si suelta el botón de salto antes de tiempo
    [SerializeField] private Transform checkGround;
    [SerializeField] private float checkGroundRadio;
    [SerializeField] private LayerMask capaSuelo;

    [Header("Valores Informativos")]

    private Rigidbody2D rPlayer;
    private Animator aPlayer;
    private float h;

    private bool tocaSuelo = false;
    private bool miraDerecha = true;
    private bool saltando = false;
    private bool puedoSaltar = false;
    private Vector2 nuevaVelocidad;

    void Start()
    {
        rPlayer = GetComponent<Rigidbody2D>();
        aPlayer = GetComponent<Animator>();
    }

    void Update()
    {
        recibePulsaciones();
        variablesAnimador();
    }

    void FixedUpdate()
    {
        checkTocaSuelo();
        movimientoPlayer();
    }

    private void movimientoPlayer()
    {
        if (tocaSuelo && !saltando)
        {
            nuevaVelocidad = new Vector2(velocidad * h, 0.0f); // Creando nueva instancia de Vector2
            rPlayer.linearVelocity = nuevaVelocidad; // Cambié linearVelocity por velocity
        }
        else
        {
            if (!tocaSuelo)
            {
                nuevaVelocidad = new Vector2(velocidad * h, rPlayer.linearVelocity.y); // Creando nueva instancia de Vector2
                rPlayer.linearVelocity = nuevaVelocidad; // Cambié linearVelocity por velocity
            }
        }
    }

    private void recibePulsaciones()
    {
        h = Input.GetAxisRaw("Horizontal");
        if ((h < 0 && miraDerecha) || (h > 0 && !miraDerecha)) giraPlayer();
        if (Input.GetButtonDown("Jump") && puedoSaltar) Salto();
        if (saltoMejorado) SaltoMejorado();
    }

    private void Salto()
    {
        saltando = true;
        puedoSaltar = false;
        rPlayer.linearVelocity = new Vector2(rPlayer.linearVelocity.x, 0f); // Cambié linearVelocity por velocity
        rPlayer.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
    }


    private void SaltoMejorado()
    {
        // Si el jugador está cayendo (velocidad negativa en Y)
        if (rPlayer.linearVelocity.y < 0)
        {
            // Aumenta la gravedad para caer más rápido
            rPlayer.linearVelocity += Vector2.up * Physics2D.gravity.y * saltoLargo * Time.deltaTime;
        } 
        // Si el jugador está subiendo pero suelta el botón de salto
        else if (rPlayer.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Aumenta la gravedad para que el salto sea más corto
            rPlayer.linearVelocity += Vector2.up * Physics2D.gravity.y * saltoCorto * Time.deltaTime;
        }
    }


    private void checkTocaSuelo()
    {
        tocaSuelo = Physics2D.OverlapCircle(checkGround.position, checkGroundRadio, capaSuelo); // Corregido el nombre de OverlapCircle
        if (rPlayer.linearVelocity.y == 0f) // Corregido el operador de comparación
        {
            saltando = false;
        }
        if (tocaSuelo && !saltando)
        {
            puedoSaltar = true;
        }
    }

    private void variablesAnimador()
    {
        aPlayer.SetFloat("velocidadX", Mathf.Abs(rPlayer.linearVelocity.x)); // Cambié linearVelocity por velocity
        aPlayer.SetFloat("velocidadY", rPlayer.linearVelocity.y); // Cambié linearVelocity por velocity
        aPlayer.SetBool("Saltando", saltando);
    }

    void giraPlayer()
    {
        miraDerecha = !miraDerecha;
        Vector3 escalaGiro = transform.localScale;
        escalaGiro.x *= -1;
        transform.localScale = escalaGiro;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkGround.position, checkGroundRadio);
    }
}
