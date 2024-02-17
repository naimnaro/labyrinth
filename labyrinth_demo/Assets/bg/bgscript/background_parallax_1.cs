using UnityEngine;

public class background_parallax_1 : MonoBehaviour
{
    [Range(-1f, 1f)]
    private float flowspeed = 0.15f;
    private float offset;
    private Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset += (Time.deltaTime * flowspeed) / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(offset,0));
    }
}
