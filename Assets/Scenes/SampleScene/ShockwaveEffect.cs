using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Test_Shadres : MonoBehaviour
{
    // The material that contains the shockwave shader
    public Material shockwaveMaterial;

    // The speed of the shockwave expansion
    public float speed = 1f;

    // The intensity of the distortion
    public float intensity = 0.1f;

    // The noise scale for the distortion
    public float noiseScale = 1f;

    // The center of the shockwave in screen space
    private Vector2 center;

    // The radius of the shockwave
    private float radius;

    // The camera sorting layer texture
    private RenderTexture cameraTexture;

    // The camera data
    private UniversalAdditionalCameraData cameraData;

    // The 2D renderer data
    private UniversalRenderPipelineAsset rendererData;

    // The 2D renderer feature settings
    private Renderer2DData renderer2DData;

    // The 2D renderer
    private ScriptableRenderer renderer;

    // The 2D renderer feature
   // public Render2DLightingPass render2DLightingPass;

    // The camera sorting layer texture id
    private int cameraSortingLayerTextureId;

    // Start is called before the first frame update
    void Start()
    {
        // Get the camera data
        cameraData = Camera.main.GetUniversalAdditionalCameraData();

        // Get the 2D renderer data
        //rendererData = cameraData.scriptableRendererData;
        // Get the 2D renderer feature settings
        //renderer2DData = rendererData.GetRenderer(0).GetRenderer2DData();

        // Get the 2D renderer
        //renderer = rendererData.GetRenderer(0).Create();

        // Get the 2D renderer feature
        //render2DLightingPass = renderer.GetFeature(0) as Render2DLightingPass;

        // Get the camera sorting layer texture id
        cameraSortingLayerTextureId = Shader.PropertyToID("_CameraSortingLayerTexture");

        // Create a new render texture for the camera sorting layer texture
        cameraTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);

        // Set the camera sorting layer texture to the shockwave material
        shockwaveMaterial.SetTexture(cameraSortingLayerTextureId, cameraTexture);
    }


    // Update is called once per frame
    void Update()
    {
        // If the space key is pressed, start a new shockwave at the mouse position
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Convert the mouse position to screen space
            center = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            // Reset the radius
            radius = 0f;
        }

        // Increase the radius by the speed
        radius += speed * Time.deltaTime;

        // Set the center and radius to the shockwave material
        shockwaveMaterial.SetVector("_Center", center);
        shockwaveMaterial.SetFloat("_Radius", radius);

        // Set the intensity and noise scale to the shockwave material
        shockwaveMaterial.SetFloat("_Intensity", intensity);
        shockwaveMaterial.SetFloat("_NoiseScale", noiseScale);
    }

    // OnRenderImage is called after all rendering is complete to render image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Copy the source texture to the destination texture
        Graphics.Blit(source, destination);

        // Get the current render target
        RenderTargetIdentifier currentTarget = new RenderTargetIdentifier(BuiltinRenderTextureType.CameraTarget);

        // Set the camera sorting layer texture as the render target
        Graphics.SetRenderTarget(cameraTexture);

        // Clear the render target
        GL.Clear(true, true, Color.clear);

        // Render the camera sorting layer texture using the 2D renderer feature
        //render2DLightingPass.Render(cameraData, ref renderingData);

        // Restore the current render target
        //Graphics.SetRenderTarget(currentTarget);

        // Apply the shockwave effect to the destination texture
        Graphics.Blit(destination, destination, shockwaveMaterial);
    }

}
