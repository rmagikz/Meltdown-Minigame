using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode {None, Chase, Orbit};

    [SerializeField] private Transform lookAt;
    [SerializeField] private Transform follow;
    [SerializeField] private CameraMode cameraMode;
    [SerializeField] private float cameraHeight;
    [SerializeField] private float targetDistance;

    void LateUpdate()
    {
        if (cameraMode == CameraMode.Orbit)
        {
            Orbit();
        }
        else
        {
            Chase();
        }
    }

    private void Orbit()
    {
        gameObject.transform.LookAt(lookAt);

        float x = targetDistance * Mathf.Sin(Time.time/2f);
        float z = targetDistance * Mathf.Cos(Time.time/2f);

        gameObject.transform.position = new Vector3(x, cameraHeight, z);
    }

    private void Chase()
    {
        gameObject.transform.position = follow.position - follow.forward * targetDistance + new Vector3(0,cameraHeight,0);
        gameObject.transform.LookAt(lookAt);
    }

    public void BeginChase(Transform target)
    {
        cameraMode = CameraMode.None;
        
        lookAt = target;
        follow = target;
        cameraHeight = 7f;
        targetDistance = 12f;

        cameraMode = CameraMode.Chase;
    }
}
