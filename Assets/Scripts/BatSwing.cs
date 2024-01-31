using System.Collections;
using UnityEngine;

public class BatSwing : MonoBehaviour
{
    public float swingSpeed = 1.0f; //lower is faster
    public bool horizontal = false;
    public bool anticlockwise = false;

    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float t = 0;

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.rotation;

        if (horizontal)
        {
            if (anticlockwise)
            {
                targetRotation = Quaternion.Euler(initialRotation.eulerAngles + new Vector3(0, -90.0f, 0));
            }
            else
            {
                targetRotation = Quaternion.Euler(initialRotation.eulerAngles + new Vector3(0, 90.0f, 0));
            }
        }
        else
        {
            if (anticlockwise)
            {
                targetRotation = Quaternion.Euler(initialRotation.eulerAngles + new Vector3(0, 0, -90.0f));
            }
            else
            {
                targetRotation = Quaternion.Euler(initialRotation.eulerAngles + new Vector3(0, 0, 90.0f));
            }
        }

        StartCoroutine(SwingCoroutine());
    }

    IEnumerator SwingCoroutine()
    {
        while (true)
        {
            float elapsedTime = 0f;

            // Swing to the target rotation
            while (elapsedTime < swingSpeed)
            {
                float rotationAmount = Mathf.SmoothStep(0, 1, elapsedTime / swingSpeed);
                transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, rotationAmount);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Swing back to the initial rotation
            elapsedTime = 0f;
            while (elapsedTime < swingSpeed)
            {
                float rotationAmount = Mathf.SmoothStep(0, 1, elapsedTime / swingSpeed);
                transform.rotation = Quaternion.Slerp(targetRotation, initialRotation, rotationAmount);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
