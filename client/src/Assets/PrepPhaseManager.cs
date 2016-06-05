using UnityEngine;
using System.Collections;

public class PrepPhaseManager : Photon.MonoBehaviour {

    private MapManager mapManager;
    public float prepPhaseDuration;
    public GameObject prepPhaseCounter;
    private double startTime = 0;
    private bool initialized = false;
    private bool receivedStartTime = false;

	void Start ()
    {
        mapManager = FindObjectOfType<MapManager>();
        
	}
	
	void Update () {
        if (!mapManager.prepPhase)
        {
            return;
        }

        if (!initialized)
        {

            object prepStartTime = null;
            if (PhotonNetwork.room.customProperties.TryGetValue("prepStartTime", out prepStartTime))
            {
                startTime = (double)PhotonNetwork.room.customProperties["prepStartTime"];
                Debug.Log("Read start time: " + startTime);
            }
            else
            {
                if (PhotonNetwork.isMasterClient)
                {
                    ExitGames.Client.Photon.Hashtable startTimeHashTable = new ExitGames.Client.Photon.Hashtable();
                    startTime = PhotonNetwork.time;
                    startTimeHashTable.Add("prepStartTime", startTime);
                    PhotonNetwork.room.SetCustomProperties(startTimeHashTable);
                }
            }

            if (startTime == 0)
            {
                return;
            }

            prepPhaseCounter.SetActive(true);
            initialized = true;
            Debug.LogError("start time: " + startTime);
        }

        if (PhotonNetwork.time - startTime >= prepPhaseDuration)
        {
            prepPhaseCounter.SetActive(false);
            mapManager.advance();
        }
	}

    public double getStartTime()
    {
        return startTime;
    }

    //void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    Debug.Log("syncing start time");
    //    if (stream.isWriting)
    //    {
    //        // startTime
    //        stream.SendNext(startTime);
    //    }
    //    else {
    //        startTime = (double)stream.ReceiveNext();
    //        if (startTime != double.MaxValue)
    //        {
    //            receivedStartTime = true;
    //        }
    //    }
    //}
}
