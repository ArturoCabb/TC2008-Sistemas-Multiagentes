using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Robot : MonoBehaviour
{
    public int m;
    public int n;
    public int noCajas;
    public int noRobots;

    public GameObject piso;
    public GameObject pisoPilas;
    public GameObject caja;
    public GameObject robot;

    public int[,] matrizEstados;
    public int[,] matrizVisitado;
    public int[] noCajasAplilados;

    // Start is called before the first frame update
    void Start()
    {
        CreaPiso();
        AuxiliarAgregaCajas();
        AuxiliarAgregaRobot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreaPiso()
    {
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Instantiate(piso, new Vector3(i, 0, j), Quaternion.identity);
            }
        }
        for (int i = 0; i < m; i++)
        {
            Instantiate(pisoPilas, new Vector3(i, 0, n), Quaternion.identity);
        }

        matrizEstados = new int[m, n+1];
        matrizVisitado = new int[m, n+1];
        noCajasAplilados = new int[m];

        for (int i = 0;i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matrizEstados[i,j] = 0;
                matrizVisitado[i,j] = 0;
            }

            noCajasAplilados[m] = 0;
        }
    }

    void AgregaCajas()
    {
        int R1 = Random.Range(0, m);
        int R2 = Random.Range(0, n);

        if (matrizEstados[R1, R2] == 0)
        {
            matrizEstados[R1, R2] = 1;
            Instatiate(caja, new Vector3(R1, 0, R2), Quaternion.identity);
        }
        else
        {
            AgregaCajas();
        }
    }

    void AuxiliarAgregaCajas()
    {
        for (int i = 0; i < noCajas; i++)
        {
            AgregaCajas();
        }
    }

    void AgregaRobot()
    {
        int R1 = Random.Range(0, m);
        int R2 = Random.Range(0, n);

        if (matrizEstados[R1, R2] == 0)
        {
            matrizEstados[R1, R2] = 2;
            Instatiate(robot, new Vector3(R1, 0, R2), Quaternion.identity);
        }
        else
        {
            AgregaCajas();
        }
    }

    void AuxiliarAgregaRobot()
    {
        for (int i = 0; i < noRobots; i++)
        {
            AgregaRobot();
        }
    }
}
