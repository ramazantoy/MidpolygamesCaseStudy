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
        if (other.gameObject.name == "Robot")//crash nesneleri ana karaktere �arpar ise
        {
            Player.Current.explosionParticleOn();//partlama efektinin etkinle�tirilmeszi
            //Player.Current.GetComponent<NavMeshAgent>().isStopped = true;
        }
    }
}
