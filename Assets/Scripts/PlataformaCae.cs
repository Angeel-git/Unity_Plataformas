using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaOscilante : MonoBehaviour
{
    [SerializeField] private float tiempoEspera;
    [SerializeField] private float tiempoReaparece;
    [SerializeField] private float margen;
    [SerializeField] private GameObject sprite1;
    [SerializeField] private GameObject sprite2;

    private Rigidbody2D rBody;
    private Vector3 posIni;
    private SpriteRenderer spr1, spr2;

    private bool menea = false;
    private float meneaDer = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        posIni = transform.position;
        spr1 = sprite1.GetComponent<SpriteRenderer>();
        spr2 = sprite2.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (menea)
        {
            transform.position = new Vector3(transform.position.x + meneaDer, transform.position.y, transform.position.z);
            if ((transform.position.x >= posIni.x + margen) || (transform.position.x <= posIni.x - margen)) meneaDer *= -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("Cae", tiempoEspera);
            Invoke("Reaparece", tiempoReaparece);
            menea = true;
        }
    }

    private void Cae()
    {
        rBody.isKinematic = false;
    }

    private void Reaparece()
    {
        menea = false;
        rBody.linearVelocity = Vector3.zero;
        rBody.isKinematic = true;
        transform.position = posIni;
        cambiaAlpha(spr1, 0.0f);
        cambiaAlpha(spr2, 0.0f);
        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn()
    {
        for (float f = 0.0f; f <= 1.0f; f += 0.1f)
        {
            cambiaAlpha(spr1, f);
            cambiaAlpha(spr2, f);
            yield return new WaitForSeconds(0.025f);
        }
        cambiaAlpha(spr1, 1f);
        cambiaAlpha(spr2, 1f);
    }

    private void cambiaAlpha(SpriteRenderer spr, float A)
    {
        Color c = spr.material.color;
        c.a = A;
        spr.material.color = c;
    }
}
