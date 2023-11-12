using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    void Start()
    {
        var shapeModule = GetComponent<ParticleSystem>().shape;
        shapeModule.scale = new Vector3(shapeModule.scale.x * (Camera.main.orthographicSize / 5),shapeModule.scale.y, shapeModule.scale.z);

        transform.position *= (Camera.main.orthographicSize / 5);
        GameManager.OnPlayerWon.AddListener(GetComponent<ParticleSystem>().Play);
    }

    void Update()
    {
        
    }
}
