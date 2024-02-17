using UnityEngine;

public class NICKNAME_text : MonoBehaviour
{
    public GameObject playerObject;
    private Transform textTransform;

    // Offset to position the text above the player's head
    public Vector3 offset = new Vector3(0f, 0f, 0f);

    void Start()
    {
        textTransform = transform;
    }

    void Update()
    {
        // Update the position of the empty GameObject to match the player's position with the offset
        textTransform.position = playerObject.transform.position + offset;
    }
}