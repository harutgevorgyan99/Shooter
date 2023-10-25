using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCmessages : MonoBehaviour
{
    [SerializeField] private UnityEvent OnTrigerEnter;
    bool isPlayerInColider;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Player>() && !isPlayerInColider)
        {
            isPlayerInColider = true;
            OnTrigerEnter?.Invoke();
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Player>() && isPlayerInColider)
        {
            isPlayerInColider = false;
        }
    }
}
