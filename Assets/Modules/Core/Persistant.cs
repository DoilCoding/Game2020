using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistant : MonoBehaviour
{
    private void Start()
    {
        foreach (var obj in FindObjectsOfType<Persistant>())
        {
            if (obj.name == transform.name && this.gameObject != obj.gameObject)
            {
                Destroy(gameObject);
                return;
            }
        }
        DontDestroyOnLoad(this);
    }
}
