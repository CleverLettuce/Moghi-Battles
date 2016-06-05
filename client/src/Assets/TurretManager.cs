using UnityEngine;
using System.Collections;

public class TurretManager : MonoBehaviour {

    public float attack;
    public float attackRate;
    private float lastFired = -100000000;
    public IDamagable parentDamagable;
    public GameObject triggerObject;
    public GameObject damagableObject;
    public TriggerManager triggerManager;
    public WallManager parentWallManager;
	
    void Start()
    {
        triggerManager = triggerObject.GetComponent<TriggerManager>();
        parentDamagable = damagableObject.GetComponent<IDamagable>();
        parentWallManager = damagableObject.GetComponent<WallManager>();
    }

	// Update is called once per frame
	void Update () {
	
        if (parentDamagable.isDead())
        {
            Destroy(transform.gameObject);
            return;
        }

        float timeElapsed = Time.time - lastFired;
        if (timeElapsed < attackRate)
        {
            return;
        }

        lastFired = Time.time;
        IDamagable[] toDealDamage = triggerManager.getObjectsInTrigger();
        foreach (IDamagable damagable in toDealDamage)
        {
            if (damagable.getTeamId() == parentWallManager.getTeamId())
            {
                continue;
            }

            damagable.getView().RPC("takeDamage", PhotonTargets.All, attack, "Fire Turret");
        }
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // rotation
            stream.SendNext(transform.rotation);

        }
        else {
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
