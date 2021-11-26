using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Carga : MonoBehaviour
{   
    
    public float pesoCarga = 1;
    public UnityEvent OnCargaTaken;
    public UnityEvent OnOrden;
    
    void CargaTaken (float amount) {
        pesoCarga -= amount;

        OnCargaTaken.Invoke();
        if (pesoCarga <= 0){
            OnOrden.Invoke();
        }
    }
    
}
