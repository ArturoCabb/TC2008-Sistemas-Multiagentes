using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CiudadPruebas : MonoBehaviour
{   
    public Vector3[] waypoints;
    private int nIntersecciones;
    private float radio;
    private int[] indices;
    public int[,] posibilidades;
    public int[,] porcentajes;
    public int[] nPosibilidades;
    public int[,] tipoRecorrido;
    private float posicionY;
    public GameObject unCarroCualquieraXD;
    public CarroPruebas[] listadoCarros;
    private GameObject[] misObjetos;
    public int nCarros = 4;
    public bool[,] matrizEuclidiana;
    private float distanciaMinima = 2;
    public int[] estados;
    public int[,] paresdeSemaforos;
    private float timer;
    private bool auxSemaforo = false;
    public GameObject bolaSemaforo;
    public int idSemaforo = 0;
    public GameObject[] misSemaforos;
    public int[] relacionSemaforo;
    private int[,] indicesConGiro;
    public int[] indicesMatrizGiro;
    public Vector3[,,] matricesDeGiros;

    Mesh mesh;
    public Vector3[] vertices;
    int[] triangles;
    private int nCubos = 0;
    private int[] indicesAux = new int[8];
    private float anchoLlantas = 0.15f;
    private float anchoCarro = 0.5f;
    private float largoCarro = 1.2f;
    private float radioLlantas = 0.3f;
    private float espacioLlantas = 0.05f;
    private int cubos = 5;
    public int n;
    public int estadoColor;
    public int pasarIdSemaforo;

    void Start()
    {   
        nPosibilidades = new int[48];
        indices = new int[4];
        radio = 2.5f;
        posicionY = 0;
        timer = 0f;

        indicesConGiro = new int[24,3];
        indicesMatrizGiro = new int[48];

        misSemaforos = new GameObject[16];
        relacionSemaforo = new int[16];

        nIntersecciones = 0;
        waypoints = new Vector3[48];
        posibilidades = new int[48, 2];
        porcentajes = new int[48, 2];
        tipoRecorrido = new int[48, 2];

        paresdeSemaforos = new int[12,2];
        estados = new int[48];

        for ( int i = 0; i < 48; i++){
            estados[i] = 0;
        }

        for (int i = 0; i < 48; i++){
            posibilidades[i, 0] = -1;
            posibilidades[i, 1] = -1;
            porcentajes[i, 0] = 100;
            porcentajes[i, 1] = 100;
        }

        for (int i = 0; i < 48; i++){
            tipoRecorrido[i, 0] = 0;
            tipoRecorrido[i, 1] = 0;
        }

        CrearWaypoints();

        IniciarPosibilidades();

        AgregarPorcentajes();

        CrearCarroPruebas();

        matricesDeGiros = new Vector3[24, 91, n];

        llenarMatrizDeGiros();

        SpawnCarros();

        matrizEuclidiana = new bool[nCarros, nCarros];
        ObtenerMatrizEuclidiana();

        transform.position = new Vector3(-5, 0, -10);

    }

    void AgregarPorcentajes(){
        PonerPorcentaje(20, 30, 70);
        PonerPorcentaje(21, 20, 80);
        PonerPorcentaje(25, 60, 40);
    }

    void PonerPorcentaje(int waypointI, int porcentaje1, int porcentaje2){
        porcentajes[waypointI, 0] = porcentaje1;
        porcentajes[waypointI, 1] = porcentaje2;
    }

    void CreateShape()
    {
        vertices = new Vector3[cubos * 8];
        triangles = new int[cubos * 36];
        
        CreateCubo(-(anchoCarro/2) - anchoLlantas, 0, -(largoCarro/2) + espacioLlantas, anchoLlantas, radioLlantas, radioLlantas);
        CreateCubo(-(anchoCarro/2) - anchoLlantas, 0, (largoCarro/2) - espacioLlantas - radioLlantas, anchoLlantas, radioLlantas, radioLlantas);
        CreateCubo((anchoCarro/2), 0, -(largoCarro/2) + espacioLlantas, anchoLlantas, radioLlantas, radioLlantas);
        CreateCubo((anchoCarro/2), 0, (largoCarro/2) - espacioLlantas - radioLlantas, anchoLlantas, radioLlantas, radioLlantas);
        CreateCubo(-(anchoCarro/2), 0.1f, -(largoCarro/2), anchoCarro, largoCarro, anchoCarro);

    }

    void CreateCubo(float origenX, float origenY, float origenZ, float ancho, float largo, float alto)
    {
        int aux = nCubos * 8;

        for(int i = 0; i < 8; i++){
            indicesAux[i] = aux + i;
        }

        vertices[indicesAux[0]] = new Vector3(origenX, origenY, origenZ);
        vertices[indicesAux[1]] = new Vector3(origenX, origenY + alto, origenZ);
        vertices[indicesAux[2]] = new Vector3(origenX + ancho, origenY + alto, origenZ);
        vertices[indicesAux[3]] = new Vector3(origenX + ancho, origenY, origenZ);
        vertices[indicesAux[4]] = new Vector3(origenX, origenY, origenZ + largo);
        vertices[indicesAux[5]] = new Vector3(origenX, origenY + alto, origenZ + largo);
        vertices[indicesAux[6]] = new Vector3(origenX + ancho, origenY + alto, origenZ + largo);
        vertices[indicesAux[7]] = new Vector3(origenX + ancho, origenY, origenZ + largo);

        aux = nCubos * 36;
        triangles[aux] = indicesAux[0];
        triangles[aux + 1] = indicesAux[1];
        triangles[aux + 2] = indicesAux[2];
        triangles[aux + 3] = indicesAux[2];
        triangles[aux + 4] = indicesAux[3];
        triangles[aux + 5] = indicesAux[0];
        triangles[aux + 6] = indicesAux[4];
        triangles[aux + 7] = indicesAux[5];
        triangles[aux + 8] = indicesAux[1];
        triangles[aux + 9] = indicesAux[1];
        triangles[aux + 10] = indicesAux[0];
        triangles[aux + 11] = indicesAux[4];
        triangles[aux + 12] = indicesAux[1];
        triangles[aux + 13] = indicesAux[5];
        triangles[aux + 14] = indicesAux[6];
        triangles[aux + 15] = indicesAux[6];
        triangles[aux + 16] = indicesAux[2];
        triangles[aux + 17] = indicesAux[1];
        triangles[aux + 18] = indicesAux[2];
        triangles[aux + 19] = indicesAux[6];
        triangles[aux + 20] = indicesAux[7];
        triangles[aux + 21] = indicesAux[7];
        triangles[aux + 22] = indicesAux[3];
        triangles[aux + 23] = indicesAux[2];
        triangles[aux + 24] = indicesAux[7];
        triangles[aux + 25] = indicesAux[6];
        triangles[aux + 26] = indicesAux[5];
        triangles[aux + 27] = indicesAux[5];
        triangles[aux + 28] = indicesAux[4];
        triangles[aux + 29] = indicesAux[7];
        triangles[aux + 30] = indicesAux[0];
        triangles[aux + 31] = indicesAux[3];
        triangles[aux + 32] = indicesAux[4];
        triangles[aux + 33] = indicesAux[4];
        triangles[aux + 34] = indicesAux[3];
        triangles[aux + 35] = indicesAux[7];

        nCubos++;

    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void AgregarDatosGiro(int indice, int desde, int hasta, int tipoGiro){
        indicesConGiro[indice, 0] = desde;
        indicesConGiro[indice, 1] = hasta;
        indicesConGiro[indice, 2] = tipoGiro;
        indicesMatrizGiro[desde] = indice;
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
        int aux2 = nIntersecciones * 2;
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

            AgregarDatosGiro(aux2, indices[3], indices[0], 6);
            AgregarDatosGiro(aux2 + 1, indices[2], indices[1], 4);

            estados[indices[3]] = 2;
            paresdeSemaforos[nIntersecciones, 0] = indices[2];
            paresdeSemaforos[nIntersecciones, 1] = indices[3];

            if (abajo){
                CrearSemaforo(indices[2]);
            }
            if (izquierda){
                CrearSemaforo(indices[3]);
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

            AgregarDatosGiro(aux2, indices[3], indices[2], 1);
            AgregarDatosGiro(aux2 + 1, indices[0], indices[1], 7);

            estados[indices[3]] = 2;
            paresdeSemaforos[nIntersecciones, 0] = indices[0];
            paresdeSemaforos[nIntersecciones, 1] = indices[3];

            if (arriba){
                CrearSemaforo(indices[0]);
            }
            if (izquierda){
                CrearSemaforo(indices[3]);
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

            AgregarDatosGiro(aux2, indices[1], indices[0], 3);
            AgregarDatosGiro(aux2 + 1, indices[2], indices[3], 5);

            estados[indices[2]] = 2;
            paresdeSemaforos[nIntersecciones, 0] = indices[1];
            paresdeSemaforos[nIntersecciones, 1] = indices[2];

            if (abajo){
                CrearSemaforo(indices[2]);
            }
            if (derecha){
                CrearSemaforo(indices[1]);
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

            AgregarDatosGiro(aux2, indices[1], indices[2], 8);
            AgregarDatosGiro(aux2 + 1, indices[0], indices[3], 2);

            estados[indices[1]] = 2;
            paresdeSemaforos[nIntersecciones, 0] = indices[0];
            paresdeSemaforos[nIntersecciones, 1] = indices[1];

            if (arriba){
                CrearSemaforo(indices[0]);
            }
            if (derecha){
                CrearSemaforo(indices[1]);
            }

            nPosibilidades[indices[0]] = aux;
            nPosibilidades[indices[1]] = aux;
            nPosibilidades[indices[2]] = 0;
            nPosibilidades[indices[3]] = 0;
        }

        nIntersecciones++;

    }

    void CrearSemaforo(int numWaypoint){
        misSemaforos[idSemaforo] = Instantiate(bolaSemaforo, waypoints[numWaypoint], Quaternion.identity);
        relacionSemaforo[idSemaforo] = numWaypoint;
        idSemaforo++;
    }

    void Semaforo()
    {
        if (timer > 5 && timer < 10)
        {   
            for (int i = 0; i < 12; i++)
            {
                estados[paresdeSemaforos[i,0]] = 1;
                estados[paresdeSemaforos[i,1]] = 1;
            }
        }
        else if (timer > 10)
        {
            for (int i = 0; i < 12; i++)
            {
                if (!auxSemaforo)
                {
                    estados[paresdeSemaforos[i,0]] = 2;
                    estados[paresdeSemaforos[i,1]] = 0;
                }
                else
                {   
                    estados[paresdeSemaforos[i,0]] = 0;
                    estados[paresdeSemaforos[i,1]] = 2;
                }
            }
            if (!auxSemaforo)
            {
                auxSemaforo = true;
            }
            else
            {
                auxSemaforo = false;
            }
            timer = 0;
        }
    }

    void ActualizarColores(){
        for (int i = 0; i < idSemaforo; i++){
            int waypointRelacionado = relacionSemaforo[i];
            if (estados[waypointRelacionado] == 0){
                pasarIdSemaforo = i;
                misSemaforos[i].GetComponent<Renderer>().material.color = Color.green;
                estadoColor = 0;
            }
            else if (estados[waypointRelacionado] == 1){
                pasarIdSemaforo = i;
                misSemaforos[i].GetComponent<Renderer>().material.color = Color.yellow;
                estadoColor = 1;
            }
            else{
                pasarIdSemaforo = i;
                misSemaforos[i].GetComponent<Renderer>().material.color = Color.red;
                estadoColor = 2;
            }
        }
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

        if (tipoGiro % 2 == 1){
            return Transformations.TranslateM(radio, 0, 0);
        }
        else{
            return Transformations.TranslateM(-radio, 0, 0);
        }

    }

    Matrix4x4 tipoGiro3(int tipoGiro){

        if (tipoGiro % 2 == 1){
            return Transformations.TranslateM(-radio, 0, 0);
        }
        else{
            return Transformations.TranslateM(radio, 0, 0);
        }

    } 

    Matrix4x4 tipoGiro2(int tipoGiro, int angulo){

        if (tipoGiro < 5){
            return Transformations.RotateM(angulo, Transformations.AXIS.AX_Y);
        }
        else{
            return Transformations.RotateM(-angulo, Transformations.AXIS.AX_Y);
        }

    }

    void llenarMatrizDeGiros(){
        Vector3 pActual;
        int giroActual;
        int cantidadPuntos = n;
        Debug.Log("Numero de puntos: " + cantidadPuntos);
        Vector3[] puntos = new Vector3[cantidadPuntos];
        Vector4[] vs = new Vector4[cantidadPuntos];

        Matrix4x4 transform1;
        Matrix4x4 transform2;
        Matrix4x4 transform3;

        for (int i = 0; i < 24; i++){
            pActual = waypoints[indicesConGiro[i, 0]];
            transform.position = pActual;
            giroActual = indicesConGiro[i, 2];
            AcomodarCoche(giroActual);
            puntos = GetComponent<MeshFilter>().mesh.vertices;
            transform1 = tipoGiro1(giroActual);
            transform3 = tipoGiro3(giroActual);

            for(int j = 0; j < 91; j++){
                transform2 = tipoGiro2(giroActual, j);

                for (int k = 0; k < cantidadPuntos; k++){
                    vs[k] = puntos[k];
                    vs[k].w = 1.0f;
                }

                for (int k = 0; k < cantidadPuntos; k++){
                    vs[k] = transform3 * transform2 * transform1 * vs[k];
                    matricesDeGiros[i, j, k] = vs[k];
                }
            }

            ReacomodarCoche(giroActual);

        }

    }

    void AcomodarCoche(int tipoGiro){
        if (tipoGiro == 1){
            transform.Rotate(0.0f, -90.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 2){
            transform.Rotate(0.0f, -180.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 3){
            transform.Rotate(0.0f, -270.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 4){
            transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 5){
            transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 6){
            transform.Rotate(0.0f, -90.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 7){
            transform.Rotate(0.0f, -180.0f, 0.0f, Space.Self);
        }
        else{
            transform.Rotate(0.0f, -270.0f, 0.0f, Space.Self);
        }
    }

    void ReacomodarCoche(int tipoGiro){
        if (tipoGiro == 1){
            transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 2){
            transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 3){
            transform.Rotate(0.0f, 270.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 4){
            transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 5){
            transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 6){
            transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        }
        else if(tipoGiro == 7){
            transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        }
        else{
            transform.Rotate(0.0f, 270.0f, 0.0f, Space.Self);
        }

    }

    void SpawnCarros(){

        misObjetos = new GameObject[nCarros];
        listadoCarros = new CarroPruebas[nCarros];

        for (int i = 0; i < nCarros; i++){
            misObjetos[i] = Instantiate(unCarroCualquieraXD, new Vector3(0,0,-30 - (i*5)), Quaternion.identity);
            listadoCarros[i] = misObjetos[i].GetComponent<CarroPruebas>();
            listadoCarros[i].numeroSerie = i;
        }

    }

    void CrearCarroPruebas(){
        
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        n = vertices.Length;
        UpdateMesh();
        
    }

    void ObtenerMatrizEuclidiana(){
        for (int i = 0; i < nCarros; i++){
            Vector3 proyeccionI = listadoCarros[i].posicionActual + listadoCarros[i].vectorDireccionUnitario;
            for ( int j = 0; j < nCarros; j++){
                if (i == j){
                    matrizEuclidiana[i, j] = true;
                }
                else{

                    Vector3 proyeccionJ = listadoCarros[j].posicionActual;

                    float x = proyeccionI.x - proyeccionJ.x;
                    float z = proyeccionI.z - proyeccionJ.z;

                    float distanciaEuclidiana = (float) Math.Sqrt((x*x)+(z*z));

                    if (distanciaEuclidiana < distanciaMinima){
                        matrizEuclidiana[i,j] = false;
                    }
                    else{
                        matrizEuclidiana[i,j] = true;
                    }
                }
            }
        }
    }

    void Update()
    {
        ObtenerMatrizEuclidiana();

        Semaforo();
        ActualizarColores();
        timer += Time.deltaTime;
    }
}
