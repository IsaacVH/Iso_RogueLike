using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public string FilePath = "/Map/Rooms/room1.room";
    public GameObject[] RoomPrefabs;
    public Material[] TileMaterials;

    public float BrickFallTime = 0.5f;

    private List<GameObject> _children = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Construct the map by loading the file from the FilePath
    /// </summary>
    public void BuildMapFromFilePath()
    {
        // First, remove any pre-existing tiles
        DestroyMapTiles();

        // Get the absolute file system path
        string absoluteFilePath = Application.dataPath + FilePath;

        // Load the byte file as an array of ints
        byte[] roomBytes = readByteFile(absoluteFilePath);

        // For some reason, each int is actually +48 more that it should be when converting
        // (0 = 48, 1 = 49, 2 = 50, etc.)
        int[] roomTiles = roomBytes.Select(x => ((int)x - 48)).ToArray();

        // Track which row + column we're on, for positioning
        var totalRows = roomTiles.Count(x => x == 2);
        var maxColumns = 0;
        var currentCol = 0;
        for (var i = 0; i < roomTiles.Length; i++)
        {
            currentCol++;
            if (roomTiles[i] == 2)
            {
                if (maxColumns < currentCol)
                {
                    maxColumns = currentCol;
                }
                currentCol = 0;
            }

        }

        var currentRow = 0;
        var currentColumn = 0;

        // Read each int as a different tile
        for (var i = 0; i < roomTiles.Length; i++)
        {
            var tileType = roomTiles[i];

            // Skip edge indicators
            if (tileType == 2)
            {
                currentColumn = 0;
                currentRow++;
                continue;
            }
            else
            {
                currentColumn++;
            }

            // Adding new tile for each type (except edges)
            GameObject newTile;
            float startingHeightPosition = 10f;
            float finalHeightPosition = 0f;

            // Depending on the Type, instantiate a tile
            switch(tileType)
            {
                case 0:
                    newTile = Instantiate(RoomPrefabs[tileType], Vector3.zero, Quaternion.identity);
                    startingHeightPosition = 10f;
                    finalHeightPosition = 0f;
                    break;
                case 3:
                    newTile = Instantiate(RoomPrefabs[tileType], Vector3.zero, Quaternion.identity);
                    startingHeightPosition = 9f;
                    finalHeightPosition = -1f;
                    break;
                default:
                    // make the new quad a child of this GameObject, and rotate around x axis by 90 degrees
                    newTile = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    newTile.transform.RotateAround(Vector3.up, Vector3.right, 90);

                    //Get the Renderer component from the new cube, and assign a material
                    var cubeRenderer = newTile.GetComponent<Renderer>();
                    cubeRenderer.material = TileMaterials[tileType];
                    finalHeightPosition = 0f;
                    break;
            }

            // Bind tile to the MapGenerator instance by setting "transform.parent" (makes "nested" menu in unity editor)
            newTile.transform.parent = this.transform;

            // Use column and row for current position
            var startingPosition = new Vector3(currentRow, startingHeightPosition, currentColumn);
            var finalPosition = new Vector3(currentRow, finalHeightPosition, currentColumn);

            if (Application.isPlaying)
            {
                newTile.transform.position = startingPosition;
                var fallingController = newTile.GetComponent<FallingBrickController>();
                if (fallingController != null)
                {
                    // Get X,Y coords that must be between 0 and 1, convert to float to get decimals
                    /*
                    var perlinX = (float)currentRow / totalRows;
                    var perlinY = (float)currentColumn / maxColumns;
                    var fallTime = BrickFallTime * Mathf.PerlinNoise(perlinX, perlinY);
                    */

                    var fallTime = BrickFallTime * Random.Range(0.5f, 1f);
                    // Make the bricks fall at different speeds
                    fallingController.SetDestination(finalPosition, fallTime);
                }
            }
            else
            {
                newTile.transform.position = finalPosition;
            }

            // Add the tile to list of children of MapGenerator, so that we can destroy it later
            _children.Add(newTile);
        }
    }

    /// <summary>
    /// Destroy the children of this gameobject
    /// </summary>
    public void DestroyMapTiles()
    {
        foreach(GameObject child in _children)
        {
            if(child != null)
            {
                SafeDestroy(child);
            }
        }
    }

    void SafeDestroy(GameObject obj)
    {
        if (Application.isEditor)
        {
            GameObject.DestroyImmediate(obj);
        }
        else
        {
            GameObject.Destroy(obj);
        }
    }

    byte[] readByteFile(string file_path)
    {
        //Read file to byte array
        FileStream stream = File.OpenRead(file_path);
        byte[] fileBytes = new byte[stream.Length];

        stream.Read(fileBytes, 0, fileBytes.Length);
        stream.Close();

        return fileBytes;
    }

    void writeByteFile(string file_path, byte[] fileBytes)
    {
        //Begins the process of writing the byte array back to a file
        using (Stream file = File.OpenWrite(file_path))
        {
            file.Write(fileBytes, 0, fileBytes.Length);
        }
    }
}
