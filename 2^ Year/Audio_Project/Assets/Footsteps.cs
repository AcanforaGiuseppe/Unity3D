using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FootSurface { Grass, Dirt, Swamp, Last}

public class Footsteps : MonoBehaviour
{
    float[] textureValues;
    FootSurface currentSurface;

    public TextureDetector detector;

    List<AudioClip>[] audioClips;
    public List<AudioClip> GrassClips;
    public List<AudioClip> DirtClips;
    public List<AudioClip> SwampClips;

    public int NeededAudioSource;
    AudioSource[] sources;

    public float MinPitchValue;
    public float MaxPitchValue;

    public float VolumeMaxOffset;


    // Start is called before the first frame update
    void Start()
    {
        audioClips = new List<AudioClip>[(int)FootSurface.Last];

        audioClips[0] = GrassClips;
        audioClips[1] = DirtClips;
        audioClips[2] = SwampClips;

        sources = new AudioSource[NeededAudioSource];

        for (int i = 0; i < sources.Length; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].volume = 0.55f;
            sources[i].spatialBlend = 1;
            sources[i].spread = 40;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootsteps()
    {
        textureValues = detector.GetTerrainTextureValues();

        int maxTextureIndex = -1;
        float textureValue = 0;

        for (int i = 0; i < textureValues.Length; i++)
        {
            if(textureValues[i] > textureValue)
            {
                textureValue = textureValues[i];
                maxTextureIndex = i;
            }
        }

        for (int i = 0; i < sources.Length; i++)
        {
            if (!sources[i].isPlaying)
            {
                sources[i].clip = audioClips[maxTextureIndex][Random.Range(0, audioClips[maxTextureIndex].Count - 1)];

                if(maxTextureIndex == (int)FootSurface.Swamp)
                {
                    sources[i].volume = 0.3f;
                }

                sources[i].pitch = Random.Range(MinPitchValue, MaxPitchValue);

                sources[i].Play();
            }
        }
    }
}
