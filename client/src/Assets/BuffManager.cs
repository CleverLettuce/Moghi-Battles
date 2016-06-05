using UnityEngine;
using System.Collections;

public class BuffManager : MonoBehaviour {

    public float attackModifier = 1.0f;
    public float defenseModifier = 1.0f;
    public float speedModifier = 1.0f;

    public PlayerManager player;
    public ISpeedProvider speedProvider;
    public bool selfControlled = false;
    public float duration;

	// Use this for initialization
	void Start () {

        player.attack *= attackModifier;
        player.defense *= defenseModifier;
        speedProvider.speed *= speedModifier;

        if (selfControlled)
        {
            Destroy(transform.gameObject, duration);
        }
    }
	
	public void stop()
    {
        player.attack /= attackModifier;
        player.defense /= defenseModifier;
        speedProvider.speed /= speedModifier;
    }

    void OnDestroy()
    {
        stop();
    }
}
