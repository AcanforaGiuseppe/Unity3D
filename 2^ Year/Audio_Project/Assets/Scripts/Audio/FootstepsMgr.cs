using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FootstepsSurfaces { Grass, Dirt, Puddle, Last}

public class FootstepsMgr : MonoBehaviour
{
    public int NeededAudioSource;


    public List<AudioClip> GrassClips;
    public List<AudioClip> DirtClips;
    public List<AudioClip> PuddleClips;
    List<AudioClip>[] SurfacesClips;

    public TextureDetector textureDetector;

    int grassClipsLenght;

    AudioSource[] sources;
    FootstepsSurfaces currentSurface;
    float[] textureValues;

    // Start is called before the first frame update
    void Start()
    {
        sources = new AudioSource[NeededAudioSource];
        SurfacesClips = new List<AudioClip>[(int)FootstepsSurfaces.Last];

        for (int i = 0; i < sources.Length; i++)
        {
            sources[i] = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        }

        SurfacesClips[0] = GrassClips;
        SurfacesClips[1] = DirtClips;
        SurfacesClips[2] = PuddleClips;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootstep()
    {
        textureValues = textureDetector.GetTerrainTextureValues();

        int maxTextureIndex = -1;
        float textureValue = 0;

        for (int i = 0; i < textureValues.Length; i++)
        {
            if (textureValues[i] > textureValue)
            {
                maxTextureIndex = i;
            }
        }

        AudioClip footstepClip = null;

        if (maxTextureIndex > -1)
        {
            footstepClip = SurfacesClips[maxTextureIndex][Random.Range(0, SurfacesClips[maxTextureIndex].Count - 1)];
        }

        for (int i = 0; i < sources.Length; i++)
        {
            if (!sources[i].isPlaying)
            {
                sources[i].clip = footstepClip;

                if(maxTextureIndex == (int)FootstepsSurfaces.Puddle)
                {
                    sources[i].volume = 0.4f;
                }
                else
                {
                    sources[i].volume = 0.6f;
                }

                sources[i].Play();
            }
        }
    }

    public void SetSurface(FootstepsSurfaces surface)
    {
        currentSurface = surface;
    }
}
