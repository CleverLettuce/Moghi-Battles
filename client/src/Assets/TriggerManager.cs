using UnityEngine;
using System.Collections.Generic;

public class TriggerManager : MonoBehaviour {

    private HashSet<IDamagable> objectsInTrigger = new HashSet<IDamagable>();

    public void removeFromTrigger(IDamagable damagable)
    {
        lock (objectsInTrigger) objectsInTrigger.Remove(damagable);
    }

    public void resetTrigger()
    {
        lock (objectsInTrigger) objectsInTrigger.Clear();
    }

    void OnTriggerStay(Collider other)
    {
        IDamagable otherManager = other.transform.GetComponent<IDamagable>();
        if (otherManager == null)
        {
            return;
        }
        
        if (otherManager.isDead())
        {
            lock (objectsInTrigger) objectsInTrigger.Remove(otherManager);
        }

        if (otherManager != null && !otherManager.isDead())
        {
            //Debug.Log("Found another damagable object in trigger: " + other.transform.ToString());
            lock (objectsInTrigger) objectsInTrigger.Add(otherManager);
        }
    }

    void OnTriggerExit(Collider other)
    {
        
        IDamagable otherManager = other.transform.GetComponent<IDamagable>();
        if (otherManager != null)
        {
            Debug.Log("Object exited trigger.");
            lock (objectsInTrigger) objectsInTrigger.Remove(otherManager);
        }
    }

    public IDamagable[] getObjectsInTrigger()
    {
        IDamagable[] objects;
        lock (objectsInTrigger) objects = objectsToArray();
        return objects;
    }

    IDamagable[] objectsToArray()
    {
        IDamagable[] objects = new IDamagable[objectsInTrigger.Count];
        int counter = 0;
        foreach(IDamagable obj in objectsInTrigger)
        {
            objects[counter] = obj;
            counter++;
        }

        return objects;
    }

}
