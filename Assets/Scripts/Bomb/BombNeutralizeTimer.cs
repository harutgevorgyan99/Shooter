using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BombNeutralizeTimer : MonoBehaviour
{
    public bool isKeyHolded;
    [SerializeField] private float tiimeForBombNeutralizeInSeconds;
    Coroutine cor;
    public Image timerIcon;
    public bool isNeutralizeting;
    public bool CheckKeyholded()
    {
        if (Input.GetKey(KeyCode.E))
            return true;
        return false;
    }
    public void StartTimer()
    {
        StopTimer();
        cor = StartCoroutine(TimerForNeutralize());
        isNeutralizeting = true;
    }
    public void StopTimer()
    {
        if (cor != null)
            StopCoroutine(cor);
        cor = null;
        timerIcon.fillAmount = 0;
        isNeutralizeting = false;
    }
    private IEnumerator TimerForNeutralize()
    {
        float secForEveryStep = 1 / tiimeForBombNeutralizeInSeconds;
        float k = 0;
        Debug.Log("start Neutralize");
        while (k<=1){
            k += secForEveryStep;
            timerIcon.fillAmount = k;
            yield return new WaitForSeconds(secForEveryStep);
            yield return new WaitForEndOfFrame();
        }
        timerIcon.fillAmount = 1;
        BombsManager.Instance.currentBomb.neutralizeMessage.text = "The bomb has already neutralized";
        BombsManager.Instance.currentBomb.isBombNeutralized = true;
        BombsManager.Instance.neutralizeBombsCount++;
        BombsManager.Instance.FinishingGameIfAllBombsNeutralized();
    }
}
