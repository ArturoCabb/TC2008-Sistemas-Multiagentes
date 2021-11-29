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
        colores = FindObjectOfType<CiudadPruebas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (colores.estados[id] == 2)
        {
            luz.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (colores.estados[id] == 0)
        {
            luz.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (colores.estados[id] == 1)
        {
            luz.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
}
