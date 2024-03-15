using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Aberration : MonoBehaviour
{

    private VolumeProfile profile;
    private ChromaticAberration MyChromaticAberration;

    // Start is called before the first frame update
    void Start()
    {
        profile = GetComponent<Volume>().profile;
        profile.TryGet(out MyChromaticAberration);

    }

    // Update is called once per frame
    void Update()
    {
        MyChromaticAberration.intensity.Override(5);
    }
}
