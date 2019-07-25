using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
	public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;
	
	[Range(0, 100)]
	public int randomFillPercent;
	
	private int[,] map;

	private void Start()
	{
		GenerateMap();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			GenerateMap();
		}
	}

	private void GenerateMap()
	{
		map = new int[width, height];
		RandomFillmap();

		for (int i = 0; i < 5; i++)
		{
			map = SmoothMap();
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator>();
		meshGen.GenerateMesh(map, 1f);
	}

	private void RandomFillmap()
	{
		if (useRandomSeed)
		{
			seed = (DateTime.Now.Millisecond * Random.value).ToString();
		}
		
		System.Random prng = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
				{
					map[x, y] = 1;
				}
				else
				{
					map[x, y] = (prng.Next(0, 100) < randomFillPercent) ? 1 : 0;
				}
			}
		}
	}

	private int[,] SmoothMap()
	{
		int[,] mapCopy = (int[,]) map.Clone();
		
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int neighborWallTiles = GetSurroundingWallCount(x, y);

				if (neighborWallTiles > 4)
				{
					mapCopy[x, y] = 1;
				}

				if (neighborWallTiles < 4)
				{
					mapCopy[x, y] = 0;
				}
			}
		}

		return mapCopy;
	}

	private int GetSurroundingWallCount(int gridX, int gridY)
	{
		int wallCount = 0;
		for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++)
		{
			for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
			{
				if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
				{
					if (neighborX != gridX || neighborY != gridY)
					{
						wallCount += map[neighborX, neighborY];
					}	
				}
				else
				{
					wallCount++;
				}
			}
		}

		return wallCount;
	}

	private void OnDrawGizmos()
	{
		/*if (map != null)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
					Vector3 pos = new Vector3(-width/2 + x + 0.5f, 0, -height/2 + y + 0.5f);
					Gizmos.DrawCube(pos, Vector3.one);
				}
			}
		}*/
	}
}
