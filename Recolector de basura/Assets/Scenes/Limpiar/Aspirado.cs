using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Aspirado : MonoBehaviour
{
    // Start is called before the first frame update
    public float tamaAspirado = 1;
    public UnityEvent OnAspiradoTaken;
    public UnityEvent OnLimpieza;

    void AspiradoTaken (float amount) {
        tamaAspirado -= amount;
        //Debug.Log("Basura por aspirar: " + tamaAspirado);
        OnAspiradoTaken.Invoke();
        if (tamaAspirado <= 0){
            OnLimpieza.Invoke();
        }
    }
}
