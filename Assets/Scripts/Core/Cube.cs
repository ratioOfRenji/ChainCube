using TMPro;
using UnityEngine;
using DG.Tweening;

public class Cube : MonoBehaviour
{
	private static int staticID;
	[SerializeField] private TMP_Text[] numbersText;

	[HideInInspector] public int CubeID;
	[HideInInspector] public Color CubeColor;
	[HideInInspector] public int CubeNumber;
	[HideInInspector] public Rigidbody CubeRgidbody;
	[HideInInspector] public bool IsMainCube;
	public bool isBinusCube;
	private MeshRenderer _cubeMeshRenderer;
	[SerializeField] private GameObject _cubeMesh;
	private void Awake()
	{
		CubeID = staticID++;
		_cubeMeshRenderer = GetComponentInChildren<MeshRenderer>();
		CubeRgidbody = GetComponent<Rigidbody>();
	}
    private void OnEnable()
    {
		_cubeMesh.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		_cubeMesh.transform.DOScale(new Vector3(1f, 1f, 1f), 0.6f)
			.SetEase(Ease.OutBounce);
	}
    public void SetColor(Color color)
	{
		CubeColor = color;
		_cubeMeshRenderer.material.color = color;
	}

	public void SetNumber(int number)
	{
		CubeNumber = number;
		for (var i = 0; i < 6; i++)
		{
			if(number >= 10000 && number < 1000000)
            {
				string simplifiedScore = (number / 1000).ToString() + "k";
				numbersText[i].text = simplifiedScore;
			}
			else if (number >=1000000 && number < 1000000000)
            {
				string simplifiedScore = (number / 1000000).ToString() + "m";
				numbersText[i].text = simplifiedScore;
			}
			else if (number >= 1000000000)
            {
				string simplifiedScore = (number / 1000000000).ToString() + "b";
				numbersText[i].text = simplifiedScore;
			}
			else
			numbersText[i].text = number.ToString();
		}
	}
}