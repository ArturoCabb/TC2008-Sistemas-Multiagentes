using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class ControllerPB : MonoBehaviour
{
	public List <Transform> waypoints = new List <Transform>();
	public List <Transform> cars = new List <Transform>();
	
	private Transform targetWaypoint;
	private int targetWaypointIndex = 19;
	private int lastWaypointIndex;
	private float minDistance = 0.1f;
	
	//private float speed = 3.0f;
	public int[,] FSMC = new int[40, 40]; // Modelo de Markov - via la matriz Adjunta
	
	private float rotationSpeed = 4.0f;
	
	//private float speed = 0.0f;
	
	// Start is called before the first frame update
	void Start()
	{

		for (int i = 0; i < 40; i++)
		{
	  	for (int j = 0; j < 40; j++)
			{
	  		FSMC[i,j] = 0;
			}	
	  }

		FSMC[0,3] = 1;
		FSMC[0,5] = 1;
		FSMC[1,34] = 1;
		FSMC[2,1] = 1;
		FSMC[2,5] = 1;
		FSMC[3,31] = 1;
		FSMC[4,3] = 1;
		FSMC[4,1] = 1;
		FSMC[5,7] = 1;
		FSMC[6,4] = 1;
		FSMC[7,10] = 1;
		FSMC[8,13] = 1;
		FSMC[8,11] = 1;
		FSMC[9,36] = 1;
		FSMC[10,9] = 1;
		FSMC[10,13] = 1;
		FSMC[11,6] = 1;
		FSMC[12,11] = 1;
		FSMC[12,9] = 1;
		FSMC[13,15] = 1;
		FSMC[14,12] = 1;
		FSMC[15,18] = 1;
		FSMC[16,20] = 1;
		FSMC[16,19] = 1;
		FSMC[17,38] = 1;
		FSMC[18,17] = 1;
		FSMC[18,20] = 1;
		FSMC[19,14] = 1;
		FSMC[20,22] = 1;
		FSMC[21,19] = 1;
		FSMC[21,17] = 1;
		FSMC[22,26] = 1;
		FSMC[23,21] = 1;
		FSMC[24,29] = 1;
		FSMC[24,27] = 1;
		FSMC[25,32] = 1;
		FSMC[26,25] = 1;
		FSMC[26,29] = 1;
		FSMC[27,23] = 1;
		FSMC[28,25] = 1;
		FSMC[28,27] = 1;
		FSMC[29,30] = 1;
		FSMC[30,2] = 1;
		FSMC[31,28] = 1;
		// Interseccion
		FSMC[32,37] = 1;
		FSMC[32,39] = 1;
		FSMC[32,35] = 1;
		
		FSMC[33,24] = 1;
		
		FSMC[34,33] = 1;
		FSMC[34,39] = 1;
		FSMC[34,37] = 1;
		
		FSMC[35,0] = 1;

		FSMC[36,35] = 1;
		FSMC[36,33] = 1;
		FSMC[36,39] = 1;

		FSMC[37,8] = 1;
		
		FSMC[38,37] = 1;
		FSMC[38,35] = 1;
		FSMC[38,33] = 1;
		
		FSMC[39,16] = 1;
			
		lastWaypointIndex = waypoints.Count -1;
		targetWaypoint = waypoints[targetWaypointIndex];
		
	}

	// Update is called once per frame
	void Update()
	{
		float movementStep = UnityEngine.Random.Range(1.0f, 5.0f) * Time.deltaTime;
		float rotationStep = rotationSpeed * Time.deltaTime;
		
		Vector3 direction2target = targetWaypoint.position - transform.position;
		
		Quaternion rotation2target = Quaternion.LookRotation(direction2target);
		
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation2target, rotationStep);
		
		Debug.DrawRay(transform.position, transform.forward * 10f, Color.green, 0f);
		Debug.DrawRay(transform.position, direction2target, Color.red, 0f);
		
		float currentDistance = Vector3.Distance(transform.position, targetWaypoint.position);
		
		CheckDistance2Waypont(currentDistance);
		
		//Debug.Log("Distance: " + currentDistance);
		Debug.Log("Cars: " + cars.Count);
		
		transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);
	}
	
	void CheckDistance2Waypont(float currentDistance)
	{
		
		if( currentDistance <= minDistance)
		{
			int[] numbers = new [] {0, 1, 2, 3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39}; // Esta son las opciones de los nodos posibles
			int[] shuffled = numbers.OrderBy(n => Guid.NewGuid()).ToArray(); // Presento las opciones de manera aleatoria
			
			for(int i = 0; i < 40; i++) // itero las opciones del agente
			{
				if( FSMC[targetWaypointIndex, shuffled[i]] == 1){ // En el momento que una opcion sea valida, la tomo
					targetWaypointIndex = shuffled[i]; // Asignamiento
					break; // Rompo el proceso dado que ya acepte una
				}
			}

			UpdateTargetWaypoint();
		}
	}
	
	void UpdateTargetWaypoint()
	{
	
		
		if( targetWaypointIndex >  lastWaypointIndex ){
			targetWaypointIndex = 0;
		}
	
		targetWaypoint = waypoints[targetWaypointIndex];
	
	}
	
}
