using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveAndSnapObject : MonoBehaviour
{
    public float gridSize = 0.4f; // Größe der Zellen im Grid

    private void SnapToGrid()
    {
        Debug.Log("SnapToGrid wurde aufgerufen!");
        // Runden der Position auf das nächste Grid
        float snapX = Mathf.Round(transform.position.x / gridSize) * gridSize;
        float snapY = Mathf.Round(transform.position.y / gridSize) * gridSize;
        float snapZ = Mathf.Round(transform.position.z / gridSize) * gridSize;

        // Setze die Position des Objekts auf das Grid
        transform.position = new Vector3(snapX, snapY, snapZ);
        Quaternion currentRotation = transform.rotation;
        Vector3 eulerRotation = currentRotation.eulerAngles;

        /*
        if (eulerRotation.x >= -360 && eulerRotation.x < -135)
        {
            transform.rotation = Quaternion.Euler(-360, -eulerRotation.y, -eulerRotation.z);
        }
        else if (eulerRotation.x >= -225 && eulerRotation.x < -135)
        {
            transform.rotation = Quaternion.Euler(-180, -eulerRotation.y, -eulerRotation.z);
        }
        else if (eulerRotation.x >= -135 && eulerRotation.x < -45)
        {
            transform.rotation = Quaternion.Euler(-90, -eulerRotation.y, -eulerRotation.z);
        }
        else if (eulerRotation.x >= -45 && eulerRotation.x < 45)
        {
            transform.rotation = Quaternion.Euler(0, -eulerRotation.y, -eulerRotation.z);
        } else if (eulerRotation.x >= 45 && eulerRotation.x < 135)
        {
            transform.rotation = Quaternion.Euler(90, -eulerRotation.y, -eulerRotation.z);
        }
        else if (eulerRotation.x >= 135 && eulerRotation.x < 225)
        {
            transform.rotation = Quaternion.Euler(180, -eulerRotation.y, -eulerRotation.z);
        }
        else if (eulerRotation.x >= 225 && eulerRotation.x < 315)
        {
            transform.rotation = Quaternion.Euler(180, -eulerRotation.y, -eulerRotation.z);
        }
        else if (eulerRotation.x >= 315 && eulerRotation.x <= 360)
        {
            transform.rotation = Quaternion.Euler(360, -eulerRotation.y, -eulerRotation.z);
        }*/

        float snappedX = Mathf.Round(eulerRotation.x / 90) * 90;
        float snappedY = Mathf.Round(eulerRotation.y / 90) * 90;
        float snappedZ = Mathf.Round(eulerRotation.z / 90) * 90;

        transform.rotation = Quaternion.Euler(snappedX, snappedY, snappedZ);


        // Ausgabe für Debugging
        Debug.Log($"Snapping to: {transform.position}");
        Debug.Log($"Current rotation: {currentRotation.eulerAngles}");
    }

    // Diese Methode wird aufgerufen, wenn das Objekt losgelassen wird
    public void OnRelease()
    {
        SnapToGrid(); // Raste das Objekt auf das Grid
    }
}
