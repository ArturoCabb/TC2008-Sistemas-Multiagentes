using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiudadPruebas : MonoBehaviour
{   
    private Vector3[] waypoints;
    private int nIntersecciones;
    private float radio;
    private int[] indices;
    private int[,] posibilidades;
    private int[] nPosibilidades;
    private int[,] tipoRecorrido;
    private float posicionY;
    // Start is called before the first frame update
    void Start()
    {   
        nIntersecciones = 0;
        waypoints = new Vector3[48];
        posibilidades = new int[48, 2];
        tipoRecorrido = new int[48, 2];

        for (int i = 0; i < 48; i++){
            posibilidades[i, 0] = -1;
            posibilidades[i, 1] = -1;
        }

        for (int i = 0; i < 48; i++){
            tipoRecorrido[i, 0] = 0;
            tipoRecorrido[i, 1] = 0;
        }

        nPosibilidades = new int[48];
        indices = new int[4];
        radio = 2.5f;
        posicionY = 0;

        AgregarWaypoints(0, posicionY, 7.5f, true, true);
        AgregarWaypoints(-10, posicionY, 7.5f, true, true);
        AgregarWaypoints(-25, posicionY, 7.5f, false, true);
        AgregarWaypoints(-35, posicionY, 7.5f, true, true);
        AgregarWaypoints(0, posicionY, -2.5f, true, true);
        AgregarWaypoints(-10, posicionY, -2.5f, true, true);
        AgregarWaypoints(-25, posicionY, -2.5f, true, true);
        AgregarWaypoints(-35, posicionY, -2.5f, true, true);
        AgregarWaypoints(0, posicionY, -12.5f, true, false);
        AgregarWaypoints(0, posicionY, -22.5f, true, true);
        AgregarWaypoints(-10, posicionY, -22.5f, true, true);
        AgregarWaypoints(-35, posicionY, -22.5f, true, true);

        AgregarPosibilidad(3, 5);
        AgregarPosibilidad(7, 9);
        AgregarPosibilidad(11, 13);
        AgregarPosibilidad(19, 21);
        AgregarPosibilidad(23, 25);
        AgregarPosibilidad(27, 29);
        AgregarPosibilidad(39, 41);
        AgregarPosibilidad(43, 45);
        AgregarPosibilidad(16, 2);
        AgregarPosibilidad(20, 6);
        AgregarPosibilidad(24, 10);
        AgregarPosibilidad(28, 14);
        AgregarPosibilidad(32, 18);
        AgregarPosibilidad(36, 34);
        /*
        AgregarPosibilidad();
        AgregarPosibilidad();
        AgregarPosibilidad();
        AgregarPosibilidad();
        AgregarPosibilidad();
        AgregarPosibilidad();
        AgregarPosibilidad();
        AgregarPosibilidad();
        AgregarPosibilidad();
        AgregarPosibilidad();
        AgregarPosibilidad();
        AgregarPosibilidad();
        AgregarPosibilidad();
        AgregarPosibilidad();
        AgregarPosibilidad();
        */

    }

    void AgregarWaypoints(float x, float y, float z, string orientacion, bool arriba, bool abajo, bool izquierda, bool derecha){
        
        int aux = nIntersecciones * 4;
        for (int i = 0; i < 4; i++){
            indices[i] = aux + i;
        }

        waypoints[indices[0]] = new Vector3(x, y, z + radio);
        waypoints[indices[1]] = new Vector3(x + radio, y, z);
        waypoints[indices[2]] = new Vector3(x, y, z - radio);
        waypoints[indices[3]] = new Vector3(x - radio, y, z);

        nIntersecciones++;

        aux = 0;
        if (orientacion == "derechaArriba")
        {
            if (arriba)
            {
                posibilidades[indices[2], aux] = indices[0];
                posibilidades[indices[3], aux] = indices[0];
                tipoRecorrido[indices[2], aux] = 0;
                tipoRecorrido[indices[3], aux] = 6;
                aux++;
            }
            if (derecha)
            {
                posibilidades[indices[2], aux] = indices[1];
                posibilidades[indices[3], aux] = indices[1];
                tipoRecorrido[indices[2], aux] = 4;
                tipoRecorrido[indices[3], aux] = 0;
                aux++;
            }

        }
        else if (orientacion == "derechaAbajo")
        {
            if (abajo)
            {
                posibilidades[indices[0], aux] = indices[2];
                posibilidades[indices[3], aux] = indices[2];
                tipoRecorrido[indices[0], aux] = 0;
                tipoRecorrido[indices[3], aux] = 1;
                aux++;
            }
            if (derecha)
            {
                posibilidades[indices[0], aux] = indices[1];
                posibilidades[indices[3], aux] = indices[1];
                tipoRecorrido[indices[0], aux] = 7;
                tipoRecorrido[indices[3], aux] = 0;
                aux++;
            }


        }
        else if (orientacion == "izquierdaArriba")
        {
            posibilidades[indices[1], 0] = indices[0];
            posibilidades[indices[1], 1] = indices[3];
            posibilidades[indices[2], 0] = indices[0];
            posibilidades[indices[2], 1] = indices[3];

            tipoRecorrido[indices[1], 0] = 3;
            tipoRecorrido[indices[1], 1] = 0;
            tipoRecorrido[indices[2], 0] = 0;
            tipoRecorrido[indices[2], 1] = 5;
            if (arriba)
            {
                posibilidades[indices[1], aux] = indices[0];
                posibilidades[indices[2], aux] = indices[0];
                tipoRecorrido[indices[1], aux] = 3;
                tipoRecorrido[indices[2], aux] = 0;
                aux++;
            }
            if (izquierda)
            {
                posibilidades[indices[1], aux] = indices[3];
                posibilidades[indices[2], aux] = indices[3];
                tipoRecorrido[indices[1], aux] = 0;
                tipoRecorrido[indices[2], aux] = 5;
                aux++;
            }


        }
        else if (orientacion == "izquierdaAbajo")
        {
            posibilidades[indices[0], 0] = indices[2];
            posibilidades[indices[0], 1] = indices[3];
            posibilidades[indices[1], 0] = indices[2];
            posibilidades[indices[1], 1] = indices[3];

            tipoRecorrido[indices[0], 0] = 0;
            tipoRecorrido[indices[0], 1] = 2;
            tipoRecorrido[indices[1], 0] = 8;
            tipoRecorrido[indices[1], 1] = 0;

        }

        //aux = 0;
        //if (Arriba){
        //    posibilidades[indices[1], aux] = indices[0];
        //    posibilidades[indices[2], aux] = indices[0];
        //    tipoRecorrido[indices[1], aux] = 1;
        //    tipoRecorrido[indices[2], aux] = 0;
        //    aux++;
        //}
        //if (Izquierda){
        //    posibilidades[indices[1], aux] = indices[3];
        //    posibilidades[indices[2], aux] = indices[3];
        //    tipoRecorrido[indices[1], aux] = 0;
        //    tipoRecorrido[indices[2], aux] = 2;
        //    aux++;
        //}

        nPosibilidades[indices[0]] = 0;
        nPosibilidades[indices[1]] = aux;
        nPosibilidades[indices[2]] = aux;
        nPosibilidades[indices[3]] = 0;
    }

    void AgregarPosibilidad(int waypointInicio, int waypointFinal){

        posibilidades[waypointInicio, nPosibilidades[waypointInicio]] = waypointFinal;
        nPosibilidades[waypointInicio]++;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
