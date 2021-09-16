using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace YizhaoCode.TerrainExtension
{
    public class TerrainGenerator : MonoBehaviour
    {
        //Terrain---------------------------------------------------
        public Terrain terrain;
        public TerrainData terrainData;
        //public TerrainLayer[] tLayers;
        public Material material;

        [Range(-1.5f, 0.0f)]public float minHeight = -0.5f;
        [Range(0.0f, 1.5f)]public float maxHeight = 1.15f;
        [Range(1.0f, 2f)]public float heightDampenerPower = 1.5f;
        [Range(0.5f, 2f)]public float roughness=2.0f;

        bool generated = false;

        //Splatmaps-------------------------------------------------
        [System.Serializable]
        public class SplatHeights
        {
            public Texture2D texture = null;
            public Texture2D normalTexture = null;
            public float minHeight = 0.1f;
            public float maxHeight = 0.2f;
            public float minSlope = 0;
            public float maxSlope = 1.5f;
            public Vector2 tileOffset = new Vector2(0, 0);
            public Vector2 tileSize = new Vector2(50, 50);
            public float splatOffset = 0.1f;
            public float splatNoiseXScale = 0.01f;
            public float splatNoiseYScale = 0.01f;
            public float splatNoiseScaler = 0.1f;
            public bool remove = false;
        }
        public List<SplatHeights> splatHeights = new List<SplatHeights>()
        {
            new SplatHeights()
        };

        private void Awake()
        {
            terrain = this.GetComponent<Terrain>();
            terrainData = Terrain.activeTerrain.terrainData;
            //tLayers = terrainData.terrainLayers;    
        }

        private void Start() 
        {
            TerrainMeshGenerate();
            SplatMaps();
        }


        private void OnApplicationQuit() 
        {
            ResetTerrain();    
        }

        float[,] GetHeightMap()
        {
            return terrainData.GetHeights(0,0, terrainData.heightmapResolution,
                                                terrainData.heightmapResolution);                    
        }

        float GetAverage(float h1, float h2, float h3, float h4, float heightMin, float heightMax)
        {
            return (float)((h1+h2+h3+h4)/4.0f + UnityEngine.Random.Range(heightMin, heightMax));
        }

        void TerrainMeshGenerate()
        {
            if(generated) ResetTerrain();

            float[,] heightMap = GetHeightMap();
            int terrainSize = terrainData.heightmapResolution-1;
            int squareSize = terrainSize;
            float heightMin = minHeight;
            float heightMax = maxHeight;
            float heightDampener = (float)Mathf.Pow(heightDampenerPower, -1*roughness);

            int diagonalX, diagonalY;                           //the coordinate of the diagonal corner of the square
            int midX, midY;                                     //the coordinate of the mid point of the square
            int midXLeft, midXRight,                            //the coordinate of the neighbor of the midpoint
                midYUp, midYDown;  

            //diamond-square Algorithm     
            while(squareSize>0)
            {
                for(int x=0; x<terrainSize; x+=squareSize)
                {
                    for(int y=0; y<terrainSize; y+=squareSize)
                    {
                        diagonalX = x + squareSize;
                        diagonalY = y + squareSize;

                        midX = (int)((diagonalX + x)/2.0f);
                        midY = (int)((diagonalY + y)/2.0f);

                        midXLeft = (int)(midX-squareSize);
                        midXRight = (int)(midX+squareSize);
                        midYUp = (int)(midY+squareSize);
                        midYDown = (int)(midY-squareSize);

                        //calculate the height value for mid point of the square
                        heightMap[midX,midY] = GetAverage(heightMap[x,y],
                                                            heightMap[x,diagonalY],
                                                            heightMap[diagonalX,y],
                                                            heightMap[diagonalX,diagonalY],
                                                            heightMin,heightMax);

                        //ignore the point outside the terrain
                        if(midXLeft<=0 || midXRight>=terrainSize
                            || midYDown<=0 || midYUp>=terrainSize) continue;
                        
                        //calculate the value for left side
                        heightMap[x,midY] = GetAverage(heightMap[x,y],
                                                        heightMap[x,diagonalY],
                                                        heightMap[midX,midY],
                                                        heightMap[midXLeft,midY],
                                                        heightMin, heightMax);
                        
                        //calculate the value for bottom side
                        heightMap[midX,y] = GetAverage(heightMap[x,y],
                                                        heightMap[midX,midY],
                                                        heightMap[diagonalX,y],
                                                        heightMap[midX,midYDown],
                                                        heightMin, heightMax);
                        
                        //calculate the value for right side
                        heightMap[diagonalX,midY] = GetAverage(heightMap[midX,midY],
                                                            heightMap[x,diagonalY],
                                                            heightMap[diagonalX,diagonalY],
                                                            heightMap[midXRight,midY],
                                                            heightMin, heightMax);

                        //calculate the value for top side
                        heightMap[midX,diagonalY] = GetAverage(heightMap[midX,midY],
                                                            heightMap[x,diagonalY],
                                                            heightMap[diagonalX,diagonalY],
                                                            heightMap[midX,midYUp],
                                                            heightMin, heightMax);
                    }
                }
                squareSize /= 2;
                heightMin *= heightDampener;
                heightMax *= heightDampener;
            } 
            terrainData.SetHeights(0,0,heightMap);
            generated = true;
        }

        void ResetTerrain()
        {
            float[,] heightMap = new float[terrainData.heightmapResolution,terrainData.heightmapResolution];
            for (int x = 0; x<terrainData.heightmapResolution; x++)
            {
                for (int y = 0; y<terrainData.heightmapResolution; y++)
                {
                    heightMap[x, y] = 0;
                }
            }
            terrainData.SetHeights(0, 0, heightMap);

            terrainData.SetAlphamaps(0,0,new float[terrainData.alphamapWidth,
                                                terrainData.alphamapHeight,
                                                terrainData.alphamapLayers]);
        }

        void Normalization(float[] v)
        {
            float total=0;
            for(int i=0; i<v.Length; i++)
            {
                total += v[i];
            }
            for(int i=0; i<v.Length; i++)
            {
                v[i] /= total;
            }
        }

        public Vector3 GetTerrainSize()
        {
            return terrainData.size;
        }

        public Vector3 GetTerrainXCenter(){
            return new Vector3 (GetTerrainSize().x * 0.5f, 0.0f, 0.0f);
        }


        public void AddNewSplatHeight()
        {
            splatHeights.Add(new SplatHeights());
        }

        public void RemoveSplatHeight()
        {
            List<SplatHeights> keptSplatHeights = new List<SplatHeights>();
            for (int i = 0; i < splatHeights.Count; i++)
            {
                if (!splatHeights[i].remove)
                {
                    keptSplatHeights.Add(splatHeights[i]);
                }
            }
            if (keptSplatHeights.Count == 0) //don't want to keep any
            {
                keptSplatHeights.Add(splatHeights[0]); //add at least 1
            }
            splatHeights = keptSplatHeights;
        }

        float GetSteepness(float[,] heightmap, int x, int y, int width, int height)
        {
            float h = heightmap[x, y];
            int nx = x + 1;
            int ny = y + 1;

            //if on the upper edge of the map find gradient by going backward.
            if (nx > width - 1) nx = x - 1;
            if (ny > height - 1) ny = y - 1;

            float dx = heightmap[nx, y] - h;
            float dy = heightmap[x, ny] - h;
            Vector2 gradient = new Vector2(dx, dy);

            float steep = gradient.magnitude;

            return steep;
        }

        public void SplatMaps()
        {
            TerrainLayer[] newSplatPrototypes;
            newSplatPrototypes = new TerrainLayer[splatHeights.Count];
            int spindex = 0;
            foreach (SplatHeights sh in splatHeights)
            {
                newSplatPrototypes[spindex] = new TerrainLayer();
                newSplatPrototypes[spindex].diffuseTexture = sh.texture;
                newSplatPrototypes[spindex].normalMapTexture = sh.normalTexture;
                newSplatPrototypes[spindex].tileOffset = sh.tileOffset;
                newSplatPrototypes[spindex].tileSize = sh.tileSize;
                newSplatPrototypes[spindex].diffuseTexture.Apply(true);
                newSplatPrototypes[spindex].normalMapTexture.Apply(true);
                spindex++;
            }
            terrainData.terrainLayers = newSplatPrototypes;

            float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, 
                                                            terrainData.heightmapResolution);
            float[,,] splatmapData = new float[terrainData.alphamapWidth,
                                                terrainData.alphamapHeight,
                                                terrainData.alphamapLayers];

            for (int y = 0; y < terrainData.alphamapHeight; y++)
            {
                for (int x = 0; x < terrainData.alphamapWidth; x++)
                {
                    float[] splat = new float[terrainData.alphamapLayers];
                    for (int i = 0; i < splatHeights.Count; i++)
                    {
                        float noise = Mathf.PerlinNoise(x * splatHeights[i].splatNoiseXScale, 
                                                        y * splatHeights[i].splatNoiseYScale) 
                                        * splatHeights[i].splatNoiseScaler;
                        float offset = splatHeights[i].splatOffset + noise;
                        float thisHeightStart = splatHeights[i].minHeight - offset;
                        float thisHeightStop = splatHeights[i].maxHeight + offset;
                        //float steepness = GetSteepness(heightMap, x, y, 
                        //                               terrainData.heightmapWidth, 
                        //                               terrainData.heightmapHeight);

                        float steepness = terrainData.GetSteepness(y / (float)terrainData.alphamapHeight,
                                            x / (float)terrainData.alphamapWidth);

                        if ((heightMap[x, y] >= thisHeightStart && heightMap[x, y] <= thisHeightStop) &&
                            (steepness >= splatHeights[i].minSlope && steepness <= splatHeights[i].maxSlope))
                        {
                            splat[i] = 1;
                        }
                    }
                    NormalizeVector(splat);
                    for (int j = 0; j < splatHeights.Count; j++)
                    {
                        splatmapData[x, y, j] = splat[j];
                    }
                }
            }
            terrainData.SetAlphamaps(0, 0, splatmapData); 
        }

        void NormalizeVector(float[] v)
        {
            float total = 0;
            for (int i = 0; i < v.Length; i++)
            {
                total += v[i];
            }

            for (int i = 0; i < v.Length; i++)
            {
                v[i] /= total;
            }
        }

    }

    
}

