using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private static bool speedUp;
    private static bool canSpin;

    void Update()
    {
        if (speedUp)
            rotationSpeed += Time.deltaTime;

        rotationSpeed = Mathf.Clamp(rotationSpeed, 0, 100);

        if (canSpin)
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }

    public static void ToggleSpin()
    {
        speedUp = !speedUp;
        canSpin = !canSpin;
    }
}
