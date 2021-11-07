using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public List<Transform> Nodos = new List<Transform>();

    private Transform targetNodoPoint; // Punto en el espacio
    public int targetNodoPointIndex = 0; // Indice en la lista de NODOS 0 - 15
    private int lastNodoIndex; // Indice del ultimo nodo que se visitó
    private float minDistance = 0.3f; // La minima distancia para considerar que el robot ya llegó al nodo
    private float speed = 3.0f; // Velocidad del robot
    public bool limpinado = true;

    // Start is called before the first frame update
    void Start()
    {
        lastNodoIndex = Nodos.Count - 1;
        targetNodoPoint = Nodos[targetNodoPointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        float movementStep = speed * Time.deltaTime; // Muevo mi robot la elocidad por el incremento del tiempo
        Vector3 direction2target = targetNodoPoint.position - transform.position; // La direccion hacia donde se mueve el robot
        float currentDistance = Vector3.Distance(transform.position, targetNodoPoint.position); // Distancia al nodo
        checkDistance(currentDistance);
        transform.position = Vector3.MoveTowards(transform.position, targetNodoPoint.position, movementStep);
    }

    void checkDistance(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            targetNodoPointIndex++;
            UpdateTargetNodoPoint();
        }
    }

    void UpdateTargetNodoPoint()
    {
        Debug.Log("Current Nodo: " + targetNodoPointIndex);
        if (targetNodoPointIndex > lastNodoIndex)
        {
            targetNodoPointIndex = 0;
        }

        targetNodoPoint = Nodos[targetNodoPointIndex];
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.CompareTag("Suciedad"))
        {
            Debug.Log("Limpiando");
            other.SendMessage("Cleaning", limpinado);
        }
    }
}
