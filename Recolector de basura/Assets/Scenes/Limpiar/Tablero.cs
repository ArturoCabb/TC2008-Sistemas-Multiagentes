using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablero : MonoBehaviour
{   
    public GameObject block;
    public GameObject basura;
    public GameObject robot;
    public int nRobots = 5;
    public int ancho = 10;
    public int largo = 20;
    public int celdasSucias = 20;
    //public int tiempoLimite = 20;
    private int contador = 0;
    private int pos;

    // Start is called before the first frame update
    void Start()
    {   
        int posibility = (ancho*largo)/celdasSucias;
        for (int z = 0; z < largo; z++){
            for (int x  = 0; x < ancho; x++){

                if (contador < celdasSucias){
                    pos = Random.Range(0, posibility);
                    if (pos == 1){
                        contador++;
                        Instantiate(basura, new Vector3(x,1,z), Quaternion.identity);
                    }
                }
                
                Instantiate(block, new Vector3(x,0,z), Quaternion.identity);
            }
        }

        for (int i = 0; i < nRobots; i++){
           Instantiate(robot, new Vector3(0,2,0), Quaternion.identity); 
        }
    }
    /*
    // Update is called once per frame
    void Update()
    {
        if (Time.time >= tiempoLimite){

        }
    }
    */
    
}
