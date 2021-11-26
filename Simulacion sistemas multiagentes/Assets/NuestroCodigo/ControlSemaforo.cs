using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSemaforo : MonoBehaviour
{
    public GameObject luz;
    private CiudadPruebas colores;
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        luz.GetComponent<ControlSemaforo>().id = id;
    }

    // Update is called once per frame
    void Update()
    {
        if ((colores.estadoColor == 2) && (id == colores.pasarIdSemaforo))
        {
            luz.GetComponent<Renderer>().material.color = Color.red;
        }
        else if ((colores.estadoColor == 0) && (id == colores.pasarIdSemaforo))
        {
            luz.GetComponent<Renderer>().material.color = Color.green;
        }
        else if ((colores.estadoColor == 1) && (id == colores.pasarIdSemaforo))
        {
            luz.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
}
