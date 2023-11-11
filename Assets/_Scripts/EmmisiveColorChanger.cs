using UnityEngine;

public class EmissiveColorChanger : MonoBehaviour
{
    public float colorChangeSpeed = 1.0f; // Speed of color change, adjustable in the Inspector
    public float emissiveIntensity = 1.0f; // Intensity of emissiveness, adjustable in the Inspector
    private Material material;

    void Start()
    {
        // Get the shared material of the Renderer component attached to this GameObject
        material = GetComponent<Renderer>().sharedMaterial;

        // Ensure the material is set to use emission
        material.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        // Calculate the hue shift over time
        float hueShift = Mathf.PingPong(Time.time * colorChangeSpeed, 1);

        // Convert hue shift to RGB color
        Color baseColor = Color.HSVToRGB(hueShift, 1f, 1f);

        // Set the color's alpha channel to control the intensity of the emissive color
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emissiveIntensity);

        // Set the emissive color of the material
        material.SetColor("_EmissionColor", finalColor);

        // Update the Global Illumination for dynamic changes
        DynamicGI.SetEmissive(GetComponent<Renderer>(), finalColor);
    }
}
