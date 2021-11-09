using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{   
    public Tablero miTablero;
    Vector3 targetBlock;
	private float minDistance = 0f;
	private float speed = 3.0f;
    private float capacidadAspirado = 1f;
    // Start is called before the first frame update
    void Start()
    {
        miTablero = FindObjectOfType<Tablero>();  
        targetBlock = new Vector3(0, 2, 0);
    }

    Vector3 checkPosibility(Vector3 posicion, int maximox, int maximoz){
        int p1 = Random.Range(0,3);
        int p2 = Random.Range(0,3);
        int[] coordenadas = new int [3] {-1, 0, 1};
        Vector3 destino;
        float coordx = (posicion.x + coordenadas[p1]);
        float coordz = (posicion.z + coordenadas[p2]);

        if((coordx < 0) || (coordz < 0) || (coordx >= (maximox - 1)) || (coordz >= (maximoz - 1)))
        {
            destino = posicion;
        }
        else
        {
            destino = new Vector3(coordenadas[p1] + posicion.x, posicion.y, coordenadas[p2] + posicion.z);
        }

        return destino;

    }

    // Update is called once per frame
    void Update()
	{
		float movementStep = speed * Time.deltaTime;
		
		Vector3 direction2target = targetBlock - transform.position;
		
		float currentDistance = Vector3.Distance(transform.position, targetBlock);

        if( currentDistance <= minDistance)
		{
			targetBlock = checkPosibility(transform.position, miTablero.ancho, miTablero.largo);
		}

		transform.position = Vector3.MoveTowards(transform.position, targetBlock, movementStep);
	}

    void OnTriggerEnter (Collider other){
        if (other.CompareTag ("Suciedad")){
            other.SendMessage("AspiradoTaken", capacidadAspirado);
        }
    }
}
