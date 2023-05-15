using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeSpawner : MonoBehaviour
{
	// Singleton class
	public static CubeSpawner Instance;
	public readonly Queue<Cube> _cubesQueue = new Queue<Cube>();
	[SerializeField] private int cubesQueueCapacity = 20;
	[SerializeField] private bool autoQueueGrow = true;

	[SerializeField] private GameObject cubePrefab;
	[SerializeField] private Color[] cubeColors;


	[HideInInspector] public int maxCubeNumber;

	[SerializeField] private CubesList _placedCubes;
	[SerializeField] private GameObject _multyCubePrefab;
	[SerializeField] private GameObject _bombCubePrefab;
	[SerializeField] private GameObject _upgrateUi;
	[SerializeField] private TMP_Text _upgrateText;
	[SerializeField]  Cube[] cubesArray;

	public delegate void BonusNumberReached();
	public static event BonusNumberReached OnBonusNumberReached;

	int minRange = 1;
	int maxRange = 6;
	int bonusRange1 = 9;
	int bounusRange2 = 10;
	int upgrateRange = 12;

	private readonly int maxPower = 12;

	private Vector3 _defaultSpawnPosition;

	private void Awake()
	{
		Instance = this;
		_defaultSpawnPosition = transform.position;
		maxCubeNumber = int.MaxValue/*(int)Mathf.Pow(2, maxPower)*/;
		InitializeCubesQueue();
	}

	private void InitializeCubesQueue()
	{
		for (var i = 0; i < cubesQueueCapacity; i++) AddCubeToQueue();
	}

	private void AddCubeToQueue()
	{
		var cube = Instantiate(cubePrefab, _defaultSpawnPosition, Quaternion.identity, transform).GetComponent<Cube>();
		cube.gameObject.SetActive(false);
		cube.IsMainCube = false;
		_cubesQueue.Enqueue(cube);
		_placedCubes.Value.Add(cube);
	}

	public Cube Spawn(int number, Vector3 position)
	{
		if (_cubesQueue.Count == 0)
		{
			if (autoQueueGrow)
			{
				cubesQueueCapacity++;
				AddCubeToQueue();
			}
			else
			{
				Debug.LogError("[Cubes Queue] : no more cubes available in th pool");
				return null;
			}
		}
		if (number == (int)Mathf.Pow(2, bonusRange1) || number == (int)Mathf.Pow(2, bounusRange2))
		{
			OnBonusNumberReached();
		}
		if(number == (int)Mathf.Pow(2, upgrateRange))
        {
			_upgrateText.text = ((int)Mathf.Pow(2, minRange)).ToString() + " выходит из игры. Все " + ((int)Mathf.Pow(2, minRange)).ToString() + " превращаются в " + ((int)Mathf.Pow(2, minRange + 1)).ToString();
			_upgrateUi.SetActive(true);
			upgrateRange += 2;
			bonusRange1++;
			bounusRange2++;
			minRange++;
			maxRange++;
			cubesArray = GetComponentsInChildren<Cube>();


			for (int i = 0; i < cubesArray.Length; i++)
			{
				if (cubesArray[i].CubeNumber < (int)Mathf.Pow(2, minRange))
				{
					Vector3 pos = cubesArray[i].gameObject.transform.position;
					DestroyCube(cubesArray[i]);
					Spawn((int)Mathf.Pow(2, minRange), pos);
				}

			}

		}
		var cube = _cubesQueue.Dequeue();
		cube.transform.position = position;
		cube.SetNumber(number);
		cube.SetColor(GetColor(number));
		cube.gameObject.SetActive(true);

		return cube;
	}
	public GameObject SpawnMultyCube()
    {
		var multy =Instantiate(_multyCubePrefab, _defaultSpawnPosition, Quaternion.identity);
		multy.gameObject.GetComponent<Cube>().IsMainCube = true;
		return multy;
    }
	public GameObject SpawnBombCube()
	{
		var bomb = Instantiate(_bombCubePrefab, _defaultSpawnPosition, Quaternion.identity);
		bomb.gameObject.GetComponent<Cube>().IsMainCube = true;
		return bomb;
	}
	public Cube SpawnRandom()
	{
		return Spawn(GenerateRandomNumber(), _defaultSpawnPosition);
	}

	public void DestroyCube(Cube cube)
	{
		cube.CubeRgidbody.velocity = Vector3.zero;
		cube.CubeRgidbody.angularVelocity = Vector3.zero;
		cube.transform.rotation = Quaternion.identity;
		cube.IsMainCube = false;
		cube.gameObject.SetActive(false);
		_cubesQueue.Enqueue(cube);
	}
	
	public int GenerateRandomNumber()
	{
		return (int)Mathf.Pow(2, Random.Range(minRange, maxRange));
	}

	private Color GetColor(int number)
	{
		int index =((int)(Mathf.Log(number) / Mathf.Log(2)) - 1);
		while(index >= cubeColors.Length)
        {
			index -= cubeColors.Length;
		}
		Debug.Log(index);
		return cubeColors[index];
	}

    private void OnDisable()
    {
		_placedCubes.Value.Clear();
    }
}