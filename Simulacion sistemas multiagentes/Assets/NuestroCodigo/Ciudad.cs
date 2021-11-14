using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ciudad : MonoBehaviour
{   
    public GameObject carro;
    public CarroMov[] listadoCarros;
    private GameObject[] misObjetos; 
    // Start is called before the first frame update
    void Start()
    {   
        misObjetos = new GameObject[2];
        
        misObjetos[0] =  Instantiate(carro, new Vector3(8,1,7), Quaternion.identity);
        misObjetos[1] =  Instantiate(carro, new Vector3(-2,1,7), Quaternion.identity);

        listadoCarros = new CarroMov[2];

        listadoCarros[0] = misObjetos[0].GetComponent<CarroMov>();
        listadoCarros[1] = misObjetos[1].GetComponent<CarroMov>();

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
