using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarroMov : MonoBehaviour
{   
    public int ruta = 0;
    public int numeroSerie;
    private int final_x;
    private Vector3 posInicial;
    private Vector3 distanciaFaltante;
    private float t = 0; 
    private float distanciaMinima = 3f;
    private Ciudad miCiudad;
    // Start is called before the first frame update
    void Start()
    {   
        miCiudad = FindObjectOfType<Ciudad>();  
        if (ruta == 0){
            final_x = -2;
        }
        else{
            final_x = 8;
        }
        posInicial = transform.position;
        distanciaFaltante = new Vector3(final_x - posInicial.x, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        bool bandera = true;
        for (int i = 0; i < miCiudad.listadoCarros.Length; i++){
            if (numeroSerie != i){
                float x = (transform.position.x - miCiudad.listadoCarros[i].transform.position.x);
                float z = (transform.position.z - miCiudad.listadoCarros[i].transform.position.z);
                double distanciaEuclidiana = Math.Sqrt((x*x)+(z*z));
                //Debug.Log("Distancia Euclidiana: " + distanciaEuclidiana);

                if (distanciaEuclidiana <= distanciaMinima){
                    bandera = false;
                    break;
                }
            }
        }

        if (bandera == true){
            if (t <= 1){
                transform.position = posInicial + (distanciaFaltante * t);
                //Debug.Log("Coordenadas: " + transform.position);
                t += 0.001f;
            }
        }
    }
}
