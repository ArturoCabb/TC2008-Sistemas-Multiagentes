using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovimiento : MonoBehaviour
{   
    public int pActualX;
    public int pActualZ;
    public int pDestinoX;
    public int pDestinoZ;
    public int nSerie;

    private float tamaPaso = 0.04f;
    private float t;
    private float capacidadCarga = 1f;

    public Vector3 distanciaFaltante;
    public Vector3 posicionInicio;
    public Vector3 posicionDestino;

    private PlanoBodega miBodega;

    public bool conCarga;
    public bool destinoHaciaCarga;
    public bool destinoHaciaPila;

    private int[,] posibilidades;
    private int[] posibilidadesConCarga;

    void Start()
    {
        miBodega = FindObjectOfType<PlanoBodega>();
        IniciarValores();

        t = 0; 

        conCarga = false;
        destinoHaciaCarga = false;
        destinoHaciaPila = false;

        IniciarPosibilidades();
        ActualizarValores(false);
    }

    void IniciarValores(){
        posicionInicio = transform.position;
        pDestinoX = (int) posicionInicio.x;
        pDestinoZ = (int) posicionInicio.z;
        posicionDestino = posicionInicio;
    }

    void IniciarPosibilidades(){
        posibilidades = new int[8, 2];
        int cont = 0;
        
        for (int i = -1; i < 2; i++){
            for (int j = -1; j < 2; j++){
                if (!(i == 0 && j == 0)){
                    posibilidades[cont, 0] = i; 
                    posibilidades[cont, 1] = j; 
                    cont++;
                }
            }
        }

        posibilidadesConCarga = new int[] {3, 0, 5, 1, 6, 2, 7, 4};
    }

    void Avanzar(){

        if (posicionInicio == posicionDestino){
            //ActualizarValores(false);
            BuscarOptimo();
        }
        else{
            if (t < 1){
                transform.position = posicionInicio + (distanciaFaltante * t);
                t += tamaPaso;
            }
            else{
                if (conCarga == true){
                    if (destinoHaciaPila == true){
                        miBodega.misObjetosCA[pDestinoX - 1, miBodega.cajasApiladas[pDestinoX - 1]].GetComponent<Renderer>().material.color = Color.blue;
                        miBodega.cajasApiladas[pDestinoX - 1]++;
                        destinoHaciaPila = false;
                        conCarga = false;
                        ActualizarValores(false);
                        miBodega.nCajasApiladas++;
                    }
                    else{
                        ActualizarValores(true);
                    }
                }
                else{
                    if (destinoHaciaCarga == true){
                        destinoHaciaCarga = false;
                        conCarga = true;
                        ActualizarValores(true);
                    }
                    else{
                        ActualizarValores(false);
                    }
                }
                t = 0;
                miBodega.numeroMov++;
            }
        }
    }

    void ActualizarValores(bool conCarga){
        posicionInicio = posicionDestino;
        pActualX = pDestinoX;
        pActualZ = pDestinoZ;
        ChecarDestino(conCarga);
        posicionDestino = new Vector3(pDestinoX, 1, pDestinoZ);
        distanciaFaltante = posicionDestino - posicionInicio;
    }

    void ChecarDestino(bool cargado){
        
        int posibleX;
        int posibleZ;
        bool encontrado = false;
        
        if (cargado == false){
            bool hayNoVisitados = false;
            int noVisitadoX = 0;
            int noVisitadoZ = 0;

            for (int i = 0; i < 8; i++){
                posibleX = pActualX + posibilidades[i, 0];
                posibleZ = pActualZ + posibilidades[i, 1];

                if (miBodega.estados[posibleX, posibleZ] == 1){
                    encontrado = true;
                    ChequeoPila(pActualX, pActualZ, 0);
                    pDestinoX = posibleX;
                    pDestinoZ = posibleZ;
                    ChequeoPila(posibleX, posibleZ, 2);
                    miBodega.visitados[posibleX, posibleZ] = 1;
                    destinoHaciaCarga = true;
                    break;
                }
                else if (miBodega.visitados[posibleX, posibleZ] == 0 && miBodega.estados[posibleX, posibleZ] == 0 && hayNoVisitados == false){
                    noVisitadoX = posibleX;
                    noVisitadoZ = posibleZ;
                    hayNoVisitados = true;
                }
            }

            if (encontrado == false){
                if (hayNoVisitados){
                    ChequeoPila(pActualX, pActualZ, 0);
                    pDestinoX = noVisitadoX;
                    pDestinoZ = noVisitadoZ;
                    ChequeoPila(pDestinoX, pDestinoZ, 2);
                    miBodega.visitados[pDestinoX, pDestinoZ] = 1;
                }
                else{
                    BuscarOptimo();
                }
            }
        }
        else{
            posibleZ = pActualZ - 1;
            for (int i = -1; i < 2; i++){
                posibleX = pActualX + i;

                if (miBodega.estados[posibleX, posibleZ] == 3){
                    //Debug.Log("Hay posible pila en " + nSerie);
                    if (miBodega.cajasApiladas[posibleX - 1] < 5){
                        //Debug.Log("HayPila uwu en " + nSerie);
                        ChequeoPila(pActualX, pActualZ, 0);
                        encontrado = true;
                        pDestinoX = posibleX;
                        pDestinoZ = posibleZ;
                        destinoHaciaPila = true;
                        miBodega.visitados[pDestinoX, pDestinoZ] = 1;
                        break;
                    }
                }
            }

            if (encontrado == false){
                BuscarOptimo();
            }
        }
        
    }

    void ChequeoPila(int posicionX, int posicionZ, int estado){
        if (miBodega.estados[posicionX, posicionZ] != 3){
            miBodega.estados[posicionX, posicionZ] = estado;
        } 
    }

    void BuscarOptimo(){
        int[,] posiblesVacios = new int[8, 2];
        int posibles = 0;
        int posibleX;
        int posibleZ;

        for (int i = 0; i < 8; i++){
            posibleX = pActualX + posibilidades[i, 0];
            posibleZ = pActualZ + posibilidades[i, 1];
            if (miBodega.estados[posibleX, posibleZ] == 0){
                posiblesVacios[posibles, 0] = posibleX;
                posiblesVacios[posibles, 1] = posibleZ;
                posibles++;
            }
        }

        if (posibles > 0){
            int rd = Random.Range(0, posibles);
            ChequeoPila(pActualX, pActualZ, 0);
            pDestinoX = posiblesVacios[rd, 0];
            pDestinoZ = posiblesVacios[rd, 1];
            ChequeoPila(pDestinoX, pDestinoZ, 2);
            miBodega.visitados[pDestinoX, pDestinoZ] = 1;
        }
    }

    void OnTriggerEnter (Collider other){
        if (other.CompareTag ("Carga")){
            other.SendMessage("CargaTaken", capacidadCarga);
        }
        else if (other.CompareTag ("Camarada")){
            ActualizarValores(conCarga);
        }

    }

    // Update is called once per frame
    void Update()
    {   
        if (miBodega.nCajasApiladas < miBodega.nCajas){
            Avanzar();
        }

    }
}
