using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSemaforo : MonoBehaviour
{
    public List<Transform> Lights = new List<Transform>();
    public float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < 5)
        {
            Lights[0].GetComponent<Renderer>().material.color = Color.red;
            Lights[1].GetComponent<Renderer>().material.color = Color.white;
            Lights[2].GetComponent<Renderer>().material.color = Color.white;
        }
        else if (timer >= 5 && timer < 10)
        {
            Lights[0].GetComponent<Renderer>().material.color = Color.white;
            Lights[1].GetComponent<Renderer>().material.color = Color.white;
            Lights[2].GetComponent<Renderer>().material.color = Color.green;
        }
        else if (timer >= 10 && timer < 15)
        {
            Lights[0].GetComponent<Renderer>().material.color = Color.white;
            Lights[1].GetComponent<Renderer>().material.color = Color.yellow;
            Lights[2].GetComponent<Renderer>().material.color = Color.white;
        }
        else
        {
            timer = 0f;
        }
    }
}
