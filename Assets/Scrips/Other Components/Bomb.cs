using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public AnimationCurve bombMovement;
    public float scaleIncrease;
    public float explosionRange;
    public float damage;
    public void StartMovement(Vector2 targetPos, float distance, float speed)
    {
        StartCoroutine(RunMovement(targetPos, distance/speed));
    }

    IEnumerator RunMovement(Vector2 targetPos, float moveTime)
    {
        Vector2 startPos = transform.position;
        Vector2 startScale = transform.localScale;
        float startTime = Time.time;
        
        while (Time.time - startTime < moveTime)
        {
            float percent = Mathf.Max(0, (Time.time - startTime) / moveTime);
            float curveEval = bombMovement.Evaluate(percent);
            transform.position = Vector2.Lerp(startPos, targetPos, curveEval);
            transform.localScale = startScale + startScale * (Mathf.Sin(percent*Mathf.PI)*scaleIncrease);
            yield return new WaitForEndOfFrame();
        }
        DamageEnemiesInRange();
        Explode();
    }

    private void Explode()
    {
        Destroy(gameObject);
    }
    
    private void DamageEnemiesInRange()
    {
        List<Health> enemies = new List<Health>();
        foreach (Transform enemy in Settings.instance.enemyContainer)
        {
            float dist = Vector2.Distance(transform.position, enemy.position);
            
            if (dist < explosionRange)
                enemy.GetComponent<Health>().Damage(damage);
        }
    }
}
