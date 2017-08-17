using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GridMapManager : MonoBehaviour {

	public static GridMapManager instance;

	private Cell[,] map;

	[SerializeField]
	private const int cellPointsCount = 4;

	[SerializeField]
	private float cellSize = 1f;

	[SerializeField]
	private float firstFloorHeightLvl = 0f;
	[SerializeField]
	private float secondFloorHeightLvl = 5f;

	private Terrain terrain;

	public float CellSize { get { return cellSize; } }


	void Awake()
	{
		if (instance == null)
			instance = this;
		terrain = Terrain.activeTerrain;
		GenerateMap ();
	}

	void GenerateMap()
	{
		float terrainWidht = terrain.terrainData.size.x;
		float terrainLength = terrain.terrainData.size.z;

		int widht = (int) (terrainWidht / cellSize);
		int lenght = (int) (terrainLength / cellSize);

		//Debug.Log (widht + " ; " + lenght);

		map = new Cell[widht, lenght];

		Vector2 startPoint = Vector2.zero;
		Vector2 currentPoint = startPoint;
		NavMeshHit mHit = new NavMeshHit ();

		for (int i = 1; i <= widht; i++)
		{
			if (i != 1)
				currentPoint = map [i - 2, 0].ThirdPoint;

			for (int j = 1; j <= lenght; j++)
			{
				if (j != 1)
					currentPoint = map [i - 1, j - 2].SecondPoint;

				Cell cell = new Cell (currentPoint,
					new Vector2 (currentPoint.x, startPoint.y + cellSize * j),
					new Vector2 (startPoint.x + cellSize * i, currentPoint.y),
					new Vector2 (startPoint.x + cellSize * i, startPoint.y + cellSize * j));


				int numberOfPoint = 0;
				if(j != 1)
				if (map [i - 1 , j - 2].Status == CellStatus.Idle)
					numberOfPoint = 2;

				while (numberOfPoint < cellPointsCount) 
				{
					var yValue = GetHeight (cell.points [numberOfPoint]);

					Vector3 worldPoint = new Vector3 (cell.points [numberOfPoint].x, yValue, cell.points [numberOfPoint].y);

					if (NavMesh.SamplePosition (worldPoint, out mHit, cellSize/4f, NavMesh.AllAreas) &&
						( (mHit.position.x <= worldPoint.x + float.Epsilon) && (mHit.position.x >= worldPoint.x - float.Epsilon) )
						&& ( (mHit.position.z <= worldPoint.z + float.Epsilon) && (mHit.position.z >= worldPoint.z - float.Epsilon) ))
					{
						if (numberOfPoint == cellPointsCount - 1) 
						{
							cell.Status = CellStatus.Idle;
							//BuildGridElement(cell.FirstPoint);
						}
					}
					else 
					{
						cell.Status = CellStatus.Occupied;
						break;
					}
					numberOfPoint++;
				}
				map [i - 1, j - 1] = cell;
			}
		}
	}

	float GetHeight(Vector2 position)
	{
		Vector3 samplePos = new Vector3 (position.x, 0f, position.y);

		var sampleHeight = terrain.SampleHeight (samplePos);
		if (sampleHeight <= firstFloorHeightLvl + 1f && sampleHeight >= firstFloorHeightLvl - 1f)
			return sampleHeight;
		if (sampleHeight <= secondFloorHeightLvl + 1f && sampleHeight >= secondFloorHeightLvl - 1f)
			return sampleHeight;
		return 0f;
	}

	public void BuildGridElement(Vector2 position)
	{
		Color Green = new Color (0f, 0.9f, 0f, 0.4f);
		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

		Vector3 sampleposition = new Vector3 (position.x, 0f, position.y);
		Vector3 objectPosition = new Vector3 (sampleposition.x + (cellSize/2f), terrain.SampleHeight (sampleposition), sampleposition.z + (cellSize/2f));

		go.GetComponent<Renderer> ().material.shader = Shader.Find ("Transparent/Diffuse");
		go.GetComponent<Renderer> ().material.color = Green;

		go.transform.localScale = new Vector3(cellSize /2, cellSize /4, cellSize/2);
		go.transform.position = objectPosition;
		go.transform.parent = transform;
		go.GetComponent<BoxCollider> ().enabled = false;
	}

	public bool MapArrayCheck(int i, int j, int iMax, int jMax)
	{
		if (i < 0 || j < 0 || iMax > map.GetLength (0) || jMax > map.GetLength (1))
			return false;
		for (int I = i; I < iMax; I++)
			for (int J = j; J < jMax; J++)
				if (map [I, J].Status == CellStatus.Occupied)
					return false;
		return true;
	}

	public struct Cell
	{
		private Vector2 firstPoint;
		private Vector2 secondPoint;
		private Vector2 thirdPoint;
		private Vector2 fourthPoint;

		public Dictionary<int, Vector2> points;

		public Vector2 FirstPoint { get { return firstPoint; } }
		public Vector2 SecondPoint { get { return secondPoint; } }
		public Vector2 ThirdPoint { get { return thirdPoint; } }
		public Vector2 FourthPoint { get { return fourthPoint; } }

		private CellStatus status;

		public CellStatus Status { get { return status; } set { status = value; } }

		public Cell(Vector2 first, Vector2 second, Vector2 third, Vector2 fourth)
		{
			points = new Dictionary<int, Vector2>();

			firstPoint = first;
			secondPoint = second;
			thirdPoint = third;
			fourthPoint = fourth;

			status = CellStatus.NULL;

			points.Add(0, firstPoint);
			points.Add(1, secondPoint);
			points.Add(2, thirdPoint);
			points.Add(3, fourthPoint);
		}
	}

	public enum CellStatus
	{
		NULL = -1,
		Idle = 0,
		Occupied = 1,
	}
}
