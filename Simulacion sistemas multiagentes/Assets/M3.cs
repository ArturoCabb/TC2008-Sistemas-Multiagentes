using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3 : MonoBehaviour
{
    public GameObject carro;
    public GameObject carroPolicia;
    public Frenado[] listadoCarros;
    private GameObject[] misObjetos; 
    // Start is called before the first frame update
    void Start()
    {   
        misObjetos = new GameObject[2];
        
        misObjetos[0] =  Instantiate(carro, new Vector3(0,0,8), Quaternion.identity);
        misObjetos[1] =  Instantiate(carroPolicia, new Vector3(0,0,-22), Quaternion.identity);

        listadoCarros = new Frenado[2];

        listadoCarros[0] = misObjetos[0].GetComponent<Frenado>();
        listadoCarros[1] = misObjetos[1].GetComponent<Frenado>();

        listadoCarros[0].ruta = 0;
        listadoCarros[0].numeroSerie = 0;
        listadoCarros[1].ruta = 1;
        listadoCarros[1].numeroSerie = 1;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
