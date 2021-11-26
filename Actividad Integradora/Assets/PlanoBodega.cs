using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanoBodega : MonoBehaviour
{   
    public GameObject piso;
    public GameObject caja;
    public GameObject robot;
    public GameObject cajaApilada;

    public int ancho = 10;
    public int largo = 10;
    public int nRobots = 5;
    public int nCajas = 20;

    private GameObject[] suelo;
    private GameObject[] misObjetosR;
    private GameObject[] misObjetosC;
    public GameObject[,] misObjetosCA;
    private RobotMovimiento[] misRobots;
    private CajaPropiedades[] misCajas;

    public int[,] estados;
    public int[,] visitados;
    public int[] cajasApiladas;

    private int nBloques = 0;
    private int anchoV;
    private int largoV;

    public int numeroMov = 0;
    public float cronometro = 0;
    public int nCajasApiladas = 0;
    public float alturaCaja = 0.5f;

    public bool finSimulacion = false;

    // Start is called before the first frame update
    void Start()
    {   
        CrearMatrices();
        CrearTablero();
        GenerarAgentes();
    }

    void CrearTablero(){

        suelo = new GameObject[anchoV * largoV];

        //Celdas centrales
        for (int i = 2; i < largo + 1; i++){
            for (int j = 1; j < ancho + 1; j++){
                AgregarBloque(0, j, i);
            }
        }

        //Celdas de pilas
        for (int i = 1; i < ancho + 1; i++){
            AgregarBloque(2, i, 1);
            estados[i, 1] = 3;
        }

        //Celdas contorno lateral
        for (int i = 0; i < largo + 2; i++){
            AgregarBloque(1, 0, i);
            AgregarBloque(1, ancho + 1, i);
            estados[0, i] = 2;
            estados[ancho, i] = 2;
        }

        //Celdas contorno inferior/superior
        for (int i = 1; i < ancho + 1; i++){
            AgregarBloque(1, i, 0);
            AgregarBloque(1, i, largo + 1);
            estados[i, 0] = 2;
            estados[i, largo] = 2;
        }
    }

    void CrearMatrices(){

        anchoV = ancho + 2;
        largoV = largo + 2;

        estados = new int[anchoV, largoV];
        visitados = new int[anchoV, largoV];
        cajasApiladas = new int[ancho];
        misObjetosC = new GameObject[nCajas];
        misObjetosR = new GameObject[nRobots];
        misObjetosCA = new GameObject[ancho, 5];
        misCajas = new CajaPropiedades[nCajas];
        misRobots = new RobotMovimiento[nRobots];

        for (int i = 0; i < anchoV; i++){
            for (int j = 0; j < largoV; j++){
                estados[i, j] = 0;
                visitados[i, j] = 0;
            }
        }

        for (int i = 0; i < ancho; i++){
            cajasApiladas[i] = 0;
        }
    }

    void GenerarAgentes(){
        int rx;
        int rz;

        for(int i = 0; i < nCajas; i++){
            rx = Random.Range(1, ancho + 1);
            rz = Random.Range(1, largo + 1);

            if (estados[rx, rz] == 0){
                estados[rx, rz] = 1;
                misObjetosC[i] = Instantiate(caja, new Vector3(rx, 1, rz), Quaternion.identity);
                misCajas[i] = misObjetosC[i].GetComponent<CajaPropiedades>();
            }
            else{
                i--;
            }
        }

        for(int i = 0; i < nRobots; i++){
            rx = Random.Range(1, ancho + 1);
            rz = Random.Range(1, largo + 1);

            if (estados[rx, rz] == 0){
                estados[rx, rz] = 2;
                visitados[rx, rz] = 1;
                misObjetosR[i] = Instantiate(robot, new Vector3(rx, 1, rz), Quaternion.identity);
                misRobots[i] = misObjetosR[i].GetComponent<RobotMovimiento>();
                misRobots[i].pActualX = rx;
                misRobots[i].pActualZ = rz;
                misRobots[i].nSerie = i;
            }
            else{
                i--;
            }
        }

        for(int i = 0; i < ancho; i++){
            for(int j = 0; j < 5; j++){
                misObjetosCA[i, j] = Instantiate(cajaApilada, new Vector3(i + 1, (alturaCaja * j) + 0.75f, 1), Quaternion.identity);
                misObjetosCA[i, j].GetComponent<Renderer>().material.color = Color.black;
            }
        }

    }

    void AgregarBloque(int color, int posicionX, int posicionZ){

        suelo[nBloques] = Instantiate(piso, new Vector3(posicionX, 0, posicionZ), Quaternion.identity);

        if (color == 0){
            suelo[nBloques].GetComponent<Renderer>().material.color = Color.white;
        }
        else if (color == 1){
            suelo[nBloques].GetComponent<Renderer>().material.color = Color.black;
        }
        else if (color == 2){
            suelo[nBloques].GetComponent<Renderer>().material.color = Color.red;
        }
        else{
            suelo[nBloques].GetComponent<Renderer>().material.color = Color.blue;
        }

        nBloques++;
    }

    // Update is called once per frame
    void Update()
    {   
        if (nCajasApiladas < nCajas){
            cronometro += Time.deltaTime;
            finSimulacion = true;
        }
    }
}
