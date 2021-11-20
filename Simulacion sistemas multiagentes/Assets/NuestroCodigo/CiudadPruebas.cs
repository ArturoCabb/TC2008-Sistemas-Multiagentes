using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiudadPruebas : MonoBehaviour
{   
    public Vector3[] waypoints;
    private int nIntersecciones;
    private float radio;
    private int[] indices;
    public int[,] posibilidades;
    public int[] nPosibilidades;
    public int[,] tipoRecorrido;
    private float posicionY;
    public Matrix4x4[,] matricesGiro;
    public GameObject unCarroCualquieraXD;
    public CarroPruebas[] listadoCarros;
    private GameObject[] misObjetos;
    public int nCarros = 4;

    // Start is called before the first frame update
    void Start()
    {   
        nPosibilidades = new int[48];
        indices = new int[4];
        radio = 2.5f;
        posicionY = 0;

        nIntersecciones = 0;
        waypoints = new Vector3[48];
        posibilidades = new int[48, 2];
        tipoRecorrido = new int[48, 2];

        matricesGiro = new Matrix4x4[9, 91];
        llenarMatrices();

        for (int i = 0; i < 48; i++){
            posibilidades[i, 0] = -1;
            posibilidades[i, 1] = -1;
        }

        for (int i = 0; i < 48; i++){
            tipoRecorrido[i, 0] = 0;
            tipoRecorrido[i, 1] = 0;
        }

        CrearWaypoints();

        IniciarPosibilidades();

        SpawnCarros();

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

        aux = 0;
        int aux2 = (nIntersecciones * 2) + 1;
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

            nPosibilidades[indices[0]] = 0;
            nPosibilidades[indices[1]] = 0;
            nPosibilidades[indices[2]] = aux;
            nPosibilidades[indices[3]] = aux;

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

            nPosibilidades[indices[0]] = aux;
            nPosibilidades[indices[1]] = 0;
            nPosibilidades[indices[2]] = 0;
            nPosibilidades[indices[3]] = aux;
        }
        else if (orientacion == "izquierdaArriba")
        {
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

            nPosibilidades[indices[0]] = 0;
            nPosibilidades[indices[1]] = aux;
            nPosibilidades[indices[2]] = aux;
            nPosibilidades[indices[3]] = 0;

        }
        else if (orientacion == "izquierdaAbajo")
        {

            if (abajo)
            {
                posibilidades[indices[0], aux] = indices[2];
                posibilidades[indices[1], aux] = indices[2];
                tipoRecorrido[indices[0], aux] = 0;
                tipoRecorrido[indices[1], aux] = 8;
                aux++;
            }
            if (izquierda)
            {
                posibilidades[indices[0], aux] = indices[3];
                posibilidades[indices[1], aux] = indices[3];
                tipoRecorrido[indices[0], aux] = 2;
                tipoRecorrido[indices[1], aux] = 0;
                aux++;
            }

            nPosibilidades[indices[0]] = aux;
            nPosibilidades[indices[1]] = aux;
            nPosibilidades[indices[2]] = 0;
            nPosibilidades[indices[3]] = 0;
        }

        nIntersecciones++;

    }

    void CrearWaypoints(){
        AgregarWaypoints(0, posicionY, 7.5f, "derechaArriba", false, true, true, false);
        AgregarWaypoints(-10, posicionY, 7.5f, "derechaAbajo", false, true, true, false); //Editado
        AgregarWaypoints(-25, posicionY, 7.5f, "derechaArriba", false, true, true, true);
        AgregarWaypoints(-35, posicionY, 7.5f, "derechaAbajo", false, true, false, true);
        AgregarWaypoints(0, posicionY, -2.5f, "izquierdaArriba", false, true, true, false);
        AgregarWaypoints(-10, posicionY, -2.5f, "izquierdaAbajo", true, true, true, true);
        AgregarWaypoints(-25, posicionY, -2.5f, "izquierdaArriba", true, false, true, true);
        AgregarWaypoints(-35, posicionY, -2.5f, "izquierdaAbajo", true, true, false, true);
        AgregarWaypoints(0, posicionY, -12.5f, "derechaArriba", true, true, false, false);
        AgregarWaypoints(0, posicionY, -22.5f, "derechaArriba", true, false, true, false);
        AgregarWaypoints(-10, posicionY, -22.5f, "derechaAbajo", true, false, true, true);
        AgregarWaypoints(-35, posicionY, -22.5f, "derechaAbajo", true, false, false, true);
    }

    void IniciarPosibilidades(){
        AgregarPosibilidad(13, 11);
        AgregarPosibilidad(9, 7);
        //AgregarPosibilidad(5, 3);
        AgregarPosibilidad(19, 21);
        AgregarPosibilidad(23, 25);
        AgregarPosibilidad(27, 29);
        AgregarPosibilidad(45, 43);
        AgregarPosibilidad(41, 39);
        AgregarPosibilidad(36, 34);
        AgregarPosibilidad(32, 18);
        //AgregarPosibilidad(16, 2);
        AgregarPosibilidad(6, 20);
        AgregarPosibilidad(22, 40);
        AgregarPosibilidad(24, 10);
        AgregarPosibilidad(14, 28);
        AgregarPosibilidad(30, 44);
    }

    void AgregarPosibilidad(int waypointInicio, int waypointFinal){

        posibilidades[waypointInicio, nPosibilidades[waypointInicio]] = waypointFinal;
        nPosibilidades[waypointInicio]++;

    }

    Matrix4x4 tipoGiro1(int tipoGiro){

        if (tipoGiro == 1){
            return Transformations.TranslateM(0f, 0.0f, radio);
        }
        else if(tipoGiro == 2){
            return Transformations.TranslateM(radio, 0.0f, 0.0f);
        }
        else if(tipoGiro == 3){
            return Transformations.TranslateM(-radio, 0.0f, 0.0f);
        }
        else if(tipoGiro == 4){
            return Transformations.TranslateM(0f, 0.0f, -radio);
        }
        else if(tipoGiro == 5){
            return Transformations.TranslateM(-radio, 0.0f, 5.0f);
        }
        else if(tipoGiro == 6){
            return Transformations.TranslateM(0f, 0.0f, -radio);
        }
        else if(tipoGiro == 7){
            return Transformations.TranslateM(radio, 0.0f, 5.0f);
        }
        else{
            return Transformations.TranslateM(0f, 0.0f, radio);
        }

    }

    Matrix4x4 tipoGiro2(int tipoGiro, int angulo){

        if (tipoGiro == 1){
            return Transformations.RotateM(-angulo, Transformations.AXIS.AX_Y);
        }
        else if(tipoGiro == 2){
            return Transformations.RotateM(-angulo - 90, Transformations.AXIS.AX_Y);
        }
        else if(tipoGiro == 3){
            return Transformations.RotateM(-angulo - 180, Transformations.AXIS.AX_Y);
        }
        else if(tipoGiro == 4){
            return Transformations.RotateM(-angulo - 270, Transformations.AXIS.AX_Y);
        }
        else if(tipoGiro == 5){
            return Transformations.RotateM(angulo, Transformations.AXIS.AX_Y);
        }
        else if(tipoGiro == 6){
            return Transformations.RotateM(angulo + 90, Transformations.AXIS.AX_Y);
        }
        else if(tipoGiro == 7){
            return Transformations.RotateM(angulo + 180, Transformations.AXIS.AX_Y);
        }
        else{
            return Transformations.RotateM(angulo + 270, Transformations.AXIS.AX_Y);
        }

    }

    void llenarMatrices(){
        Matrix4x4 transform1 = Transformations.TranslateM(radio, 0.0f, 0.0f);
        Matrix4x4 transform2;
        Matrix4x4 transform3;

        for (int i = 1; i < 9; i++){
            transform3 = tipoGiro1(i);
            for (int j = 0; j < 91; j++){
                transform2 = tipoGiro2(i, j);

                matricesGiro[i, j] = transform3 * transform2 * transform1;
            }
        }
    }

    void SpawnCarros(){

        misObjetos = new GameObject[nCarros];
        listadoCarros = new CarroPruebas[nCarros];

        for (int i = 0; i < nCarros; i++){
            misObjetos[i] = Instantiate(unCarroCualquieraXD, new Vector3(0,0,-28), Quaternion.identity);
            listadoCarros[i] = misObjetos[i].GetComponent<CarroPruebas>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
