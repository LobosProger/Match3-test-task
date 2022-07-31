using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
	[SerializeField]
	private Bubble bubblePrefab;
	[SerializeField]
	private int gridSize = 6;

	private Dictionary<Vector2Int, Bubble> bubblesOnGrid = new Dictionary<Vector2Int, Bubble>();
	private Dictionary<Vector2Int, Transform> spawnPointOnGridForBubbles = new Dictionary<Vector2Int, Transform>();

	private Stack<Bubble> destroyedBubles = new Stack<Bubble>();

	public static BoardController singletonBoardController;

	private bool isBoardRefreshing = false;
	private bool isBoardClickable = true;
	private void Start()
	{
		singletonBoardController = this;
		SetupBoard();
	}

	private void SetupBoard()
	{
		Transform[] transforms = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			transforms[i] = transform.GetChild(i);
		}

		int indexOfChildTransformSpawnPoints = 0;
		for (int y = 0; y < gridSize; y++)
		{
			for (int x = 0; x < gridSize; x++)
			{
				spawnPointOnGridForBubbles.Add(new Vector2Int(x, y), transforms[indexOfChildTransformSpawnPoints]);
				indexOfChildTransformSpawnPoints++;
			}
		}

		foreach (Vector2Int everyCell in spawnPointOnGridForBubbles.Keys)
		{
			Bubble createdBubble = Instantiate(bubblePrefab);
			CreateBubbleOnCell(createdBubble, everyCell, GetRandomColor());
		}
	}

	private Color GetRandomColor()
	{
		Color randomColor = Color.white;
		switch (Random.Range(0, 6))
		{
			case 0: randomColor = Color.red; break;
			case 1: randomColor = Color.green; break;
			case 2: randomColor = Color.blue; break;
			case 3: randomColor = Color.cyan; break;
			case 4: randomColor = Color.magenta; break;
			case 5: randomColor = Color.yellow; break;
			case 6: randomColor = Color.white; break;
		}
		return randomColor;
	}

	private void CreateBubbleOnCell(Bubble creatingBubble, Vector2Int gridPosition, Color colorOfBubble)
	{
		creatingBubble.transform.SetParent(transform.parent, false);
		creatingBubble.transform.position = spawnPointOnGridForBubbles[gridPosition].transform.position;
		creatingBubble.InitializeBubble(gridPosition, colorOfBubble);
		if (!bubblesOnGrid.ContainsKey(gridPosition))
			bubblesOnGrid.Add(gridPosition, creatingBubble);
		else
			bubblesOnGrid[gridPosition] = creatingBubble;
	}

	public void RemoveBubbleFromBoard(Vector2Int key)
	{
		if (!IsAnyBubblesOnHorizontalLine(key))
		{
			if (!IsAnyBubblesOnVerticalLine(key))
			{
				GameController.singletonGameController.DestroyBubbles(1);
				RemoveBubble(key);
			}
		}
	}

	private bool IsAnyBubblesOnHorizontalLine(Vector2Int keyOfTouchedBubble)
	{
		List<Vector2Int> destroyingBubbles = new List<Vector2Int>();
		destroyingBubbles.Add(keyOfTouchedBubble);

		Vector2Int checkingBubbleOnRightSideHorizontalLine = new Vector2Int(keyOfTouchedBubble.x + 1, keyOfTouchedBubble.y);
		Vector2Int checkingBubbleOnLeftSideHorizontalLine = new Vector2Int(keyOfTouchedBubble.x - 1, keyOfTouchedBubble.y);
		while (true)
		{
			//* Здесь идем вдоль правой стороны - то есть проверяем шарики справа относительного нажатого шарика
			if (bubblesOnGrid.ContainsKey(checkingBubbleOnRightSideHorizontalLine))
			{
				if (bubblesOnGrid[keyOfTouchedBubble].IsBubbleSimilar(bubblesOnGrid[checkingBubbleOnRightSideHorizontalLine]))
				{
					destroyingBubbles.Add(checkingBubbleOnRightSideHorizontalLine);
					checkingBubbleOnRightSideHorizontalLine = new Vector2Int(checkingBubbleOnRightSideHorizontalLine.x + 1, checkingBubbleOnRightSideHorizontalLine.y);
				}
			}

			if (bubblesOnGrid.ContainsKey(checkingBubbleOnLeftSideHorizontalLine))
			{
				if (bubblesOnGrid[keyOfTouchedBubble].IsBubbleSimilar(bubblesOnGrid[checkingBubbleOnLeftSideHorizontalLine]))
				{
					destroyingBubbles.Add(checkingBubbleOnLeftSideHorizontalLine);
					checkingBubbleOnLeftSideHorizontalLine = new Vector2Int(checkingBubbleOnLeftSideHorizontalLine.x - 1, checkingBubbleOnLeftSideHorizontalLine.y);
				}
			}

			bool hasAnySimilarBubblesOnLeftSide = bubblesOnGrid.ContainsKey(checkingBubbleOnLeftSideHorizontalLine) && bubblesOnGrid[keyOfTouchedBubble].IsBubbleSimilar(bubblesOnGrid[checkingBubbleOnLeftSideHorizontalLine]);
			bool hasAnySimilarBubblesOnRightSide = bubblesOnGrid.ContainsKey(checkingBubbleOnRightSideHorizontalLine) && bubblesOnGrid[keyOfTouchedBubble].IsBubbleSimilar(bubblesOnGrid[checkingBubbleOnRightSideHorizontalLine]);

			if (!hasAnySimilarBubblesOnLeftSide && !hasAnySimilarBubblesOnRightSide)
				break;
		}

		if (destroyingBubbles.Count >= 3)
		{
			GameController.singletonGameController.DestroyBubbles(destroyingBubbles.Count);
			foreach (Vector2Int everyKey in destroyingBubbles)
				RemoveBubble(everyKey);

			return true;
		}
		else
			return false;
	}

	private bool IsAnyBubblesOnVerticalLine(Vector2Int keyOfTouchedBubble)
	{
		List<Vector2Int> destroyingBubbles = new List<Vector2Int>();
		destroyingBubbles.Add(keyOfTouchedBubble);

		Vector2Int checkingBubbleOnUpSideVerticalLine = new Vector2Int(keyOfTouchedBubble.x, keyOfTouchedBubble.y + 1);
		Vector2Int checkingBubbleOnDownSideVerticalLine = new Vector2Int(keyOfTouchedBubble.x, keyOfTouchedBubble.y - 1);
		while (true)
		{
			//* Здесь идем вдоль правой стороны - то есть проверяем шарики справа относительного нажатого шарика
			if (bubblesOnGrid.ContainsKey(checkingBubbleOnUpSideVerticalLine))
			{
				if (bubblesOnGrid[keyOfTouchedBubble].IsBubbleSimilar(bubblesOnGrid[checkingBubbleOnUpSideVerticalLine]))
				{
					destroyingBubbles.Add(checkingBubbleOnUpSideVerticalLine);
					checkingBubbleOnUpSideVerticalLine = new Vector2Int(checkingBubbleOnUpSideVerticalLine.x, checkingBubbleOnUpSideVerticalLine.y + 1);
				}
			}

			if (bubblesOnGrid.ContainsKey(checkingBubbleOnDownSideVerticalLine))
			{
				if (bubblesOnGrid[keyOfTouchedBubble].IsBubbleSimilar(bubblesOnGrid[checkingBubbleOnDownSideVerticalLine]))
				{
					destroyingBubbles.Add(checkingBubbleOnDownSideVerticalLine);
					checkingBubbleOnDownSideVerticalLine = new Vector2Int(checkingBubbleOnDownSideVerticalLine.x, checkingBubbleOnDownSideVerticalLine.y - 1);
				}
			}

			bool hasAnySimilarBubblesOnLeftSide = bubblesOnGrid.ContainsKey(checkingBubbleOnDownSideVerticalLine) && bubblesOnGrid[keyOfTouchedBubble].IsBubbleSimilar(bubblesOnGrid[checkingBubbleOnDownSideVerticalLine]);
			bool hasAnySimilarBubblesOnRightSide = bubblesOnGrid.ContainsKey(checkingBubbleOnUpSideVerticalLine) && bubblesOnGrid[keyOfTouchedBubble].IsBubbleSimilar(bubblesOnGrid[checkingBubbleOnUpSideVerticalLine]);

			if (!hasAnySimilarBubblesOnLeftSide && !hasAnySimilarBubblesOnRightSide)
				break;
		}

		if (destroyingBubbles.Count >= 3)
		{
			GameController.singletonGameController.DestroyBubbles(destroyingBubbles.Count);
			foreach (Vector2Int everyKey in destroyingBubbles)
				RemoveBubble(everyKey);

			return true;
		}
		else
			return false;
	}

	private void RemoveBubble(Vector2Int key)
	{
		destroyedBubles.Push(bubblesOnGrid[key]);
		bubblesOnGrid[key].DestroyBubble();
		bubblesOnGrid[key] = null;

		if (!IsInvoking(nameof(MoveBubblesAndFillBoard)))
		{
			isBoardRefreshing = true;
			Invoke(nameof(MoveBubblesAndFillBoard), 0.4f);
		}
	}

	private void MoveBubblesAndFillBoard()
	{
		for (int y = gridSize - 1; y >= 1; y--)
		{
			for (int x = gridSize - 1; x >= 0; x--)
			{
				if (bubblesOnGrid[new Vector2Int(x, y)] is null)
				{
					for (int detectedBubbleOnY = y - 1; detectedBubbleOnY >= 0; detectedBubbleOnY--)
					{
						Vector2Int keyOfDetectedBubble = new Vector2Int(x, detectedBubbleOnY);
						Vector2Int keyOfNullCellInGridOfBubbles = new Vector2Int(x, y);
						if (bubblesOnGrid[keyOfDetectedBubble] != null)
						{
							bubblesOnGrid[keyOfDetectedBubble].UpdateBubblePositionOnGrid(keyOfNullCellInGridOfBubbles, spawnPointOnGridForBubbles[keyOfNullCellInGridOfBubbles]);
							bubblesOnGrid[keyOfNullCellInGridOfBubbles] = bubblesOnGrid[keyOfDetectedBubble];
							bubblesOnGrid[keyOfDetectedBubble] = null;
							break;
						}
					}
				}
			}
		}

		for (int y = gridSize - 1; y >= 0; y--)
		{
			for (int x = gridSize - 1; x >= 0; x--)
			{
				if (bubblesOnGrid[new Vector2Int(x, y)] is null)
				{
					Bubble creatingBubble = destroyedBubles.Pop();
					CreateBubbleOnCell(creatingBubble, new Vector2Int(x, y), GetRandomColor());
				}
			}
		}

		isBoardRefreshing = false;
	}

	public bool IsBoardRefreshing() => isBoardRefreshing;
	public bool IsBoardClickable() => isBoardClickable;

	public void SetBoardClickable(bool clickable) => isBoardClickable = clickable;
}
