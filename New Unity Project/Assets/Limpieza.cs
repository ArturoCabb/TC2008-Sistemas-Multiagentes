using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Limpieza : MonoBehaviour
{
    public bool isCleaning = false;
    public UnityEvent OnDead;

    void Cleaning(bool clean)
    {
        isCleaning = clean;
        Debug.Log("Se est� limpiando: " + isCleaning);
        if (isCleaning)
        {
            OnDead.Invoke();
        }
    }
}
