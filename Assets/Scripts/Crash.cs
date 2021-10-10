using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crash : MonoBehaviour
{
   
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Robot")//crash nesneleri ana karaktere çarpar ise
        {
            Player.Current.explosionParticleOn();//partlama efektinin etkinleþtirilmeszi
            //Player.Current.GetComponent<NavMeshAgent>().isStopped = true;
        }
    }
}
