using UnityEngine;

public class RedZone : MonoBehaviour
{
	public GameObject gameOver;

	private void OnTriggerStay(Collider other)
	{
		var cube = other.GetComponent<Cube>();
		if (cube != null)
			if (!cube.IsMainCube && cube.CubeRgidbody.velocity.magnitude < .1f)
			{
				gameOver.SetActive(true);
				Time.timeScale = 0f;
			}
	}
}