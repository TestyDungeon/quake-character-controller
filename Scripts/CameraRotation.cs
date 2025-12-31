using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private Transform cameraPivot;
    [SerializeField] private float xMouseSensitivity = 3;
    [SerializeField] private float yMouseSensitivity = 3;
    [SerializeField] private float sideTilt = 3;
    [SerializeField] private float sideTiltSpeed = 5;
    private float targetSideTilt = 0;
    private float currentSideTilt = 0;

    private float rotX, rotY;

    void Start()
    {
        cameraPivot = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        Mouse();
        CameraTilt();
    }

    private void Mouse()
    {
        rotX -= Input.GetAxisRaw("Mouse Y") * xMouseSensitivity;
        rotY += Input.GetAxisRaw("Mouse X") * yMouseSensitivity;

        if (rotX > 90)
            rotX = 90;
        if (rotX < -90)
            rotX = -90;

        transform.localRotation = Quaternion.Euler(0f, rotY, 0f);
        cameraPivot.transform.localRotation = Quaternion.Euler(rotX, 0f, currentSideTilt);
    }

    private void CameraTilt()
    {
        float side = Input.GetAxisRaw("Horizontal");
        if (side == 1)
        {
            targetSideTilt = -sideTilt;
        }
        else if (side == -1)
            targetSideTilt = sideTilt;
        else
            targetSideTilt = 0;

        currentSideTilt = Mathf.Lerp(currentSideTilt, targetSideTilt, sideTiltSpeed * Time.deltaTime);

    }
}
