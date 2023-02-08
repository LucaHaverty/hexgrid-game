using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimatedCoin : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float startVelocity;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float velAnimTime;

    private Transform coinTarget;
    private Vector2 startVelVector;

    private int value = 1;
    
    private void Start()
    {
        coinTarget = MoneyManager.instance.coinTarget;

        startVelVector = Utils.AngleToVectorDegrees(Random.Range(0, 360))*startVelocity;

        StartCoroutine(AnimateVelocity());
    }

    public void SetValue(int value)
    {
        this.value = value;
    }

    IEnumerator AnimateVelocity()
    {
        float startTime = Time.time;
        while (Time.time - startTime < velAnimTime)
        {
            float percent = (Time.time - startTime) / velAnimTime;
            Vector2 targetVelVector = (coinTarget.transform.position - transform.position).normalized * (moveSpeed);

            rb.velocity = Vector2.Lerp(startVelVector, targetVelVector, percent);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CoinTarget"))
        {
            MoneyManager.instance.AttemptAddMoney(value);
            Destroy(gameObject);
        }
    }
}
