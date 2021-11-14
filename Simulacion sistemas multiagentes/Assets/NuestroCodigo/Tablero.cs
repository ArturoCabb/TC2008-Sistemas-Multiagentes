using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablero : MonoBehaviour
{
    public int noCoches;
    public GameObject carro;
    public Conduce[] listadoCarros;
    private GameObject[] misObjetos;
    private Vector3[] posibilidades;

    // Start is called before the first frame update
    void Start()
    {
        posibilidades = new Vector3[8];
        posibilidades[0] = new Vector3(15.43793f, 0, -21.45222f);
        posibilidades[1] = new Vector3(17.60793f, 0, -9.272224f);
        posibilidades[2] = new Vector3(15.36794f, 0, 13.01778f);
        posibilidades[3] = new Vector3(5.447935f, 0, 13.04778f);
        posibilidades[4] = new Vector3(-19.52207f, 0, 13.04778f);
        posibilidades[5] = new Vector3(-21.89207f, 0, 10.78778f);
        posibilidades[6] = new Vector3(-21.95207f, 0, 0.807776f);
        posibilidades[7] = new Vector3(-21.93207f, 0, -19.21222f);

        misObjetos = new GameObject[noCoches];
        for (int i = 0; i < noCoches; i++)
        {
            int pu = Random.Range(0, 7);
            misObjetos[i] = Instantiate(carro, posibilidades[pu], Quaternion.identity);
            listadoCarros[i] = misObjetos[i].GetComponent<Conduce>();
            listadoCarros[i].ruta = pu;
            listadoCarros[i].numeroSerie = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
