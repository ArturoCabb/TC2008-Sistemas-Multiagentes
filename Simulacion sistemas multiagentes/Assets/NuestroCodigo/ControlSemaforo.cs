using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSemaforo : MonoBehaviour
{
    public List<Transform> Lights = new List<Transform>();
    public float timer = 0.0f;
    private CiudadPruebas colores;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (colores.estadoColor == 2)
        {
            Lights[0].GetComponent<Renderer>().material.color = Color.red;
            Lights[1].GetComponent<Renderer>().material.color = Color.white;
            Lights[2].GetComponent<Renderer>().material.color = Color.white;
        }
        else if (colores.estadoColor == 0)
        {
            Lights[0].GetComponent<Renderer>().material.color = Color.white;
            Lights[1].GetComponent<Renderer>().material.color = Color.white;
            Lights[2].GetComponent<Renderer>().material.color = Color.green;
        }
        else if (colores.estadoColor == 1)
        {
            Lights[0].GetComponent<Renderer>().material.color = Color.white;
            Lights[1].GetComponent<Renderer>().material.color = Color.yellow;
            Lights[2].GetComponent<Renderer>().material.color = Color.white;
        }
        else
        {
            timer = 0f;
        }
    }
}
