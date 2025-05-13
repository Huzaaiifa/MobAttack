using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulsating : MonoBehaviour
{
    class OriginalMaterialState
    {
        public Color color;
        public Color emissionColor;
    }

    List<OriginalMaterialState> originalMaterialStates = new List<OriginalMaterialState>();
    Renderer[] renderers;

    public float pulseSpeed = 0.1f;
    public float pulseAmplitude = 0.1f; // Usually, amplitude values are kept within a smaller range
    public Color glowColor = Color.white;
    private float timer = 0;

    public bool glow = true;

    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
        foreach (Renderer renderer in renderers)
        {
            Material mat = renderer.material;
            OriginalMaterialState originalState = new OriginalMaterialState
            {
                color = mat.color,
                emissionColor = mat.HasProperty("_EmissionColor") ? mat.GetColor("_EmissionColor") : Color.black
            };
            originalMaterialStates.Add(originalState);
        }

        InvokeRepeating("pulsate", 0f, 0.02f); // Increased the frequency for smoother pulsation
    }

    public void pulsate()
    {
        timer += Time.deltaTime * pulseSpeed;
        float alpha = Mathf.Sin(timer) * pulseAmplitude + 1f;

        foreach (Renderer renderer in renderers)
        {
            Material mat = renderer.material; // This creates a new instance of the material

            // Check if the material has an _EmissionColor property
            if (mat.HasProperty("_EmissionColor"))
            {
                // Ensure emission is enabled
                mat.EnableKeyword("_EMISSION");

                // Set the color and emission color with the new alpha
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, alpha);
                mat.SetColor("_EmissionColor", glowColor * alpha);
            }
            else
            {
                Debug.LogWarning("Material does not have an _EmissionColor property: " + mat.name);
            }
        }
    }

    public void cancelPulsate()
    {
        CancelInvoke("pulsate");
        ResetMaterials();
    }

    private void ResetMaterials()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer renderer = renderers[i];
            Material mat = renderer.material;

            OriginalMaterialState originalState = originalMaterialStates[i];

            // Reset the color and emission to the original state
            mat.color = originalState.color;

            if (mat.HasProperty("_EmissionColor"))
            {
                if (originalState.emissionColor != Color.black)
                {
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", originalState.emissionColor);
                }
                else
                {
                    mat.DisableKeyword("_EMISSION");
                }
            }
        }
    }
}
