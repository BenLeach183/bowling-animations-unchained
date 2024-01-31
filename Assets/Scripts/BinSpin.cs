using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BinSpin : MonoBehaviour
{
    public float spinSpeed;
    public bool anticlockwise = false;

    private void Start()
    {
        if (anticlockwise)
        {
            spinSpeed = -spinSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float rotationAmount = spinSpeed * Time.deltaTime;
        gameObject.transform.Rotate(0, 0, rotationAmount);
    }
}
