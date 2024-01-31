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
        //ability to change direction of the spin in editor, useful when chaining multiple together
        if (anticlockwise)
        {
            spinSpeed = -spinSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //using time.deltatime proved difficult when tring to use it in the rotate function itself
        //unity doesn't seem to like that, so made it a separate variable
        float rotationAmount = spinSpeed * Time.deltaTime;
        gameObject.transform.Rotate(0, 0, rotationAmount);
    }
}
