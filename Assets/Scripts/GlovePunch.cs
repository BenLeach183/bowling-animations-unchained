using UnityEngine;

public class GlovePunch : MonoBehaviour
{
    public float punchSpeed = 20.0f;
    public GameObject punchPoint;

    private Vector3 initialPosition;
    private bool isPunching = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float step = punchSpeed;

        if (isPunching)
        {
            transform.position = Vector3.MoveTowards(transform.position, punchPoint.transform.position, step);

            if (Vector3.Distance(transform.position, punchPoint.transform.position) < 0.01f)
            {
                isPunching = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);

            if (Vector3.Distance(transform.position, initialPosition) < 0.01f)
            {
                isPunching = true;
            }
        }
    }
}
