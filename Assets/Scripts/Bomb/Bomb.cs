using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    public BombNeutralizeTimer bombNeutralize;
    [HideInInspector]public bool isBombNeutralized;
    public GameObject neutralizeMessageObj;
    public Animator anim;
    public Text neutralizeMessage;
    public UnityEvent OnBombNeutralize; // actions attached from ins[pector
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Player>())
        {
            Debug.Log("OnCollisionEnter");
            BombsManager.Instance.currentBomb = this;
            if (isBombNeutralized)
            {
                neutralizeMessage.text = "The bomb has already neutralized";
                bombNeutralize.timerIcon.fillAmount = 1; 
            }
            else
            {
                neutralizeMessage.text= "Hold down \"E\" ";
            }
            neutralizeMessageObj.SetActive(true);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Player>())
        {
            Debug.Log("OnCollisionExit");
            bombNeutralize.StopTimer();
            neutralizeMessageObj.SetActive(false);
        }
    }
    public void PlayNeutralizeAnim()
    {
        anim.SetBool("neutralized", true);
    }
    public void ResetAnim()
    {
        anim.SetBool("neutralized", false);
    }
}
