using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureDetector : MonoBehaviour
{
    Terrain t;
    float coordX;
    float coordY;
    float[,,] aMap;
    public float[] textureValues;
    public int TextureNumber;

    // Start is called before the first frame update
    void Start()
    {
        t = Terrain.activeTerrain;
        textureValues = new float[TextureNumber];
    }

    public float[] GetTerrainTextureValues()
    {
        return textureValues;
    }

    // Update is called once per frame
    void Update()
    {
        GetAlphaPosition();

        aMap = t.terrainData.GetAlphamaps((int)coordX, (int)coordY, 1, 1);

        for (int i = 0; i < textureValues.Length; i++)
        {
            textureValues[i] = aMap[0, 0, i];
        }
    }

    void GetAlphaPosition()
    {
        Vector3 terrainPosition = transform.position - t.transform.position;

        Vector3 mapPosition = new Vector3(
            terrainPosition.x / t.terrainData.size.x,
            0,
            terrainPosition.z / t.terrainData.size.z);

        coordX = mapPosition.x * t.terrainData.alphamapWidth;
        coordY = mapPosition.z * t.terrainData.alphamapHeight;
    }
}
