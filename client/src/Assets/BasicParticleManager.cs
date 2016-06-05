using UnityEngine;
using System.Collections;
using System;

public class BasicParticleManager : MonoBehaviour, IParticle {

    public ParticleSystem ps;

    public void run()
    {
        if (ps != null)
        {
            ps.Play();
        }
    }
	
}
