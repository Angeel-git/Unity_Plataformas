using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Valores Configurables")]
    [SerializeField] private float      velocidad;
    [SerializeField] private float      fuerzaSalto;
    [SerializeField] private bool       saltoMejorado;
    [SerializeField] private float      saltoLargo = 1.5f; // Multiplicador para aumentar la gravedad cuando el jugador cae
    [SerializeField] private float      saltoCorto = 1f;   // Multiplicador para aumentar la gravedad si suelta el botón de salto antes de tiempo
    [SerializeField] private Transform  checkGround;
    [SerializeField] private float      checkGroundRadio;
    [SerializeField] private LayerMask  capaSuelo;
    [SerializeField] private float      addRayo;
    [SerializeField] private float      anguloMax;
    [SerializeField] private PhysicsMaterial2D      sinF;
    [SerializeField] private PhysicsMaterial2D      maxF;

    [Header("Valores Informativos")]
    [SerializeField] private bool                tocaSuelo = false;
    [SerializeField] private bool                enPendiente;
    [SerializeField] private bool                puedoAndar;
    [SerializeField] private float               anguloPendiente;
    [SerializeField] private float           h;



    private Rigidbody2D     rPlayer;
    private Animator        aPlayer;
    private bool                miraDerecha = true;
    private bool                saltando = false;
    private bool                puedoSaltar = false;
    private bool                enPlataforma = false;
    private Vector2             nuevaVelocidad;
    private CapsuleCollider2D   ccPlayer;
    private Vector2             ccSize;
    private float               anguloLateral;
    private float               anguloAnterior;
    private Vector2             anguloPer;
    private Vector3             posIni;


    void Start()
    {
        posIni = transform.position;
        rPlayer = GetComponent<Rigidbody2D>();
        aPlayer = GetComponent<Animator>();
        ccPlayer = GetComponent<CapsuleCollider2D>();
        ccSize = ccPlayer.size;
    }

    void Update()
    {
        recibePulsaciones();
        variablesAnimador();
    }

    void FixedUpdate()
    {
        checkTocaSuelo();
        checkPendiente();
        movimientoPlayer();
    }

    private void movimientoPlayer()
    {
        if (tocaSuelo && !saltando && !enPendiente)
        {
            nuevaVelocidad = new Vector2(velocidad * h, 0.0f); // Creando nueva instancia de Vector2
            rPlayer.linearVelocity = nuevaVelocidad; // Cambié linearVelocity por velocity
        }else if(tocaSuelo && !saltando && puedoAndar && enPendiente)
        {
            nuevaVelocidad.Set(velocidad * anguloPer.x * -h, velocidad * anguloPer.y * -h);
            rPlayer.linearVelocity = nuevaVelocidad;
        }
        else if(!tocaSuelo)
        {
            nuevaVelocidad = new Vector2(velocidad * h, rPlayer.linearVelocity.y); // Creando nueva instancia de Vector2
            rPlayer.linearVelocity = nuevaVelocidad; // Cambié linearVelocity por velocity
        }
    }

    private void recibePulsaciones()
    {
        if(Input.GetKey(KeyCode.R)) transform.position = posIni;    //VOLVER AL PLAYER A POS INICIAL
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

private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.tag == "PlataformaMovil")
    {
        rPlayer.linearVelocity = Vector3.zero;
        transform.parent = collision.transform;
        enPlataforma = true;
    }
}

private void OnCollisionExit2D(Collision2D collision)
{
    if (collision.gameObject.tag == "PlataformaMovil")
    {
        transform.parent = null;
        enPlataforma = false;
    }
}



        private void checkPendiente()
    {
        if(!enPlataforma)
        {
            Vector2 posPies = transform.position - (Vector3)(new Vector2(0.0f, ccSize.y / 2));
            checkPendHoriz(posPies);
            checkPendVerti(posPies);
        }

    }

    private void checkPendHoriz(Vector2 posPies)
    {
        RaycastHit2D hitDelante = Physics2D.Raycast(posPies, Vector2.right, addRayo, capaSuelo);
        RaycastHit2D hitDetras = Physics2D.Raycast(posPies, -Vector2.right, addRayo, capaSuelo);
        Debug.DrawRay(posPies, Vector2.right * addRayo, Color.cyan);
        Debug.DrawRay(posPies, -Vector2.right * addRayo, Color.red);

        if (hitDelante)
        {
            enPendiente = true;
            anguloLateral = Vector2.Angle(hitDelante.normal, Vector2.up);
        }
        else if (hitDetras)
        {
            enPendiente = true;
            anguloLateral = Vector2.Angle(hitDetras.normal, Vector2.up);
        }
        else
        {
            enPendiente = false;
            anguloLateral = 0.0f;
        }
    }

        private void checkPendVerti(Vector2 posPies)
    {
        RaycastHit2D hit = Physics2D.Raycast(ccPlayer.bounds.center, Vector2.down, ccPlayer.bounds.extents.y + addRayo, capaSuelo);

        if (hit)
        {
            anguloPendiente = Vector2.Angle(hit.normal, Vector2.up);
            anguloPer = Vector2.Perpendicular(hit.normal).normalized;

            if (anguloPendiente != anguloAnterior)
            {
                enPendiente = true;
                anguloAnterior = anguloPendiente;
            }

            Debug.DrawRay(hit.point, anguloPer, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

            if (anguloPendiente > anguloMax || anguloLateral > anguloMax)
            {
                puedoAndar = false;
            }
            else
            {
                puedoAndar = true;

                if (enPendiente && puedoAndar && h == 0.0f)
                {
                    rPlayer.sharedMaterial = maxF;
                }
                else
                {
                    rPlayer.sharedMaterial = sinF;
                }
            }
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
        if (checkGround != null)
        {
            Gizmos.DrawWireSphere(checkGround.position, checkGroundRadio);
        }
    }
}
