using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{
    public Vector2 minCamPos, maxCampPos;
    public GameObject seguir;
    public float movSuave;

    private Vector2 velocidad;

    void Start()
    {

    }

    void Update()
    {
        float posx = Mathf.SmoothDamp(transform.position.x, seguir.transform.position.x, ref velocidad.x, movSuave);
        float posy = Mathf.SmoothDamp(transform.position.y, seguir.transform.position.y, ref velocidad.y, movSuave);

        transform.position = new Vector3(
            Mathf.Clamp(posx, minCamPos.x, maxCampPos.x),
            Mathf.Clamp(posy, minCamPos.y, maxCampPos.y),
            transform.position.z);
    }
}
