using UnityEngine;
using System.Collections;
using System;

public class PickUpSpawnManager : Photon.MonoBehaviour {

    public GameObject[] pickUpSpawns;
    public GameObject[] pickUpsToSpawn;

	// Use this for initialization
	void Start () {
	
        if (PhotonNetwork.player.isMasterClient)
        {
            foreach (GameObject spawn in pickUpSpawns)
            {
                GameObject pickUp = selectPickUpToSpawn();
                float selector = UnityEngine.Random.value;
                PickUpManager manager = pickUp.GetComponent<PickUpManager>();
                if (selector < manager.spawnChance)
                {
                    PhotonNetwork.Instantiate(pickUp.name, spawn.transform.position, spawn.transform.rotation, 0);
                }
            }
        }
	}

    private GameObject selectPickUpToSpawn()
    {
        return pickUpsToSpawn[UnityEngine.Random.Range(0, pickUpsToSpawn.Length)];
    }
}
