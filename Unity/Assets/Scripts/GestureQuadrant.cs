using UnityEngine;
using System.Collections;

public class GestureQuadrant : MonoBehaviour {

    public string identifier;

    void OnTriggerEnter(Collider col) {
        Debug.Log(identifier);
        Debug.Log(string.Format("You've entered {0}", identifier));
    }
}
