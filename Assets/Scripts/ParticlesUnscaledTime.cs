using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesUnscaledTime : MonoBehaviour {

     
	public ParticleSystem ps;
     void Update()
     {
         if (Time.timeScale < 0.01f)
         {
             ps.Simulate(Time.unscaledDeltaTime, true, false);
         }
     }
}
