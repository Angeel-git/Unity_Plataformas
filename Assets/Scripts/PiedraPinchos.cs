using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiedraPinchos : MonoBehaviour
{
    [SerializeField] private Transform destino;
    [SerializeField] private float velocidad;
    [SerializeField] private float tiempoQuieto;

    private Vector3 posIni, posFin;
    private bool enMovi;
    private float tiempo;

    // Start is called before the first frame update
    void Start()
    {
        destino.parent = null;
        posIni = transform.position;
        posFin = destino.position;

        enMovi = true;
        tiempo = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enMovi)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);
            if (transform.position == destino.position)
            {
                destino.position = (destino.position == posFin) ? posIni : posFin;
                enMovi = false;
            }
        }
        else
        {
            tiempo += Time.deltaTime;
            if (tiempo >= tiempoQuieto)
            {
                tiempo = 0.0f;
                enMovi = true;
            }
        }
    }
}
