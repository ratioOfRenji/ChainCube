using UnityEngine;

public class BombCubeCollision : MonoBehaviour
{
	private FX _FX;
	private AudioSource audioSource;

	private void Awake()
	{
		audioSource = FindObjectOfType<AudioSource>();
		_FX = FindObjectOfType<FX>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		var otherCube = collision.gameObject.GetComponent<Cube>();

		// check if contacted with other cube
		var hasOtherCube = otherCube != null;

		if (hasOtherCube)
		{
			// check if both cubes have same number
			if (true)
			{

				var contactPoint = collision.contacts[0].point;

				// check if cubes number less than max number in CubeSpawner
				if (otherCube.CubeNumber < CubeSpawner.Instance.maxCubeNumber)
				{
					// spawn a new ube as a reslt

					audioSource.Play();

				}
				// the explosion should affect surrounded cubes too:
				//var clsurroundedCubes = Physics.OverlapSphere(contactPoint, 0.7f);
				//foreach (Collider coll in clsurroundedCubes)
				//	if (coll.GetComponent<Cube>() != null)
    //                {
				//		MyCubeSpawner.Instance.DestroyCube(coll.GetComponent<Cube>());

				//	}
						
				var surroundedCubes = Physics.OverlapSphere(contactPoint, 2f);
				var explosionForce = 400f;
				var explosionRadius = 1.5f;
				foreach (var coll in surroundedCubes)
					if (coll.attachedRigidbody != null)
						coll.attachedRigidbody.AddExplosionForce(explosionForce, contactPoint, explosionRadius);


                //_FX.PlayCubeExplosionFX(contactPoint, otherCube.CubeColor);


                //Destroy the two cubes:

                CubeSpawner.Instance.DestroyCube(otherCube);
                this.gameObject.SetActive(false);
				/*Destroy(this.gameObject)*/;
			}
		}
	}
}
