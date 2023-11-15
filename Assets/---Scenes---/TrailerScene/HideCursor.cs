using UnityEngine;

public class HideCursor : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // Press 'H' to hide/show the cursor
        {
            Cursor.visible = !Cursor.visible;
        }
    }
}
