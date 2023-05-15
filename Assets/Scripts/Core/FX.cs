using UnityEngine;

public class FX : MonoBehaviour
{
	[SerializeField] private ParticleSystem cubeExplosionFX;

	private ParticleSystem.MainModule cubeExplosionFXMainModule;

	private void Start()
	{
		cubeExplosionFXMainModule = cubeExplosionFX.main;
	}

	public void PlayCubeExplosionFX(Vector3 positon, Color color)
	{
		cubeExplosionFXMainModule.startColor = new ParticleSystem.MinMaxGradient(color);
		cubeExplosionFX.transform.position = positon;
		cubeExplosionFX.Play();
	}
}