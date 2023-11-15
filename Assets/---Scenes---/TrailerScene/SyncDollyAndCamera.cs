using Cinemachine;
using UnityEngine;

public class SyncDollyAndCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineDollyCart dollyCart;
    public bool isMoving = false;

    void Update()
    {
        // Toggle movement when space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = !isMoving;
        }

        if (virtualCamera != null && dollyCart != null && isMoving)
        {
            var trackedDolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
            if (trackedDolly != null)
            {
                trackedDolly.m_PathPosition = dollyCart.m_Position;
            }
        }
    }
}
