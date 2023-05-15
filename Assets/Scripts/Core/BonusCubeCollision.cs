using UnityEngine;

public class BonusCubeCollision : MonoBehaviour
{
	private FX _FX;
	private AudioSource audioSource;
	[SerializeField]
	private CubesList _placedCubes;

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
					var newCube = CubeSpawner.Instance.Spawn(otherCube.CubeNumber * 2, contactPoint + Vector3.up * 1.6f);
					audioSource.Play();

					Cube cubeToMergeWith = null;
					float Distance = Mathf.Infinity;
					Vector3 pushDirection;
					for (int i = 0; i < _placedCubes.Value.Count; i++)
					{
						if (_placedCubes.Value[i].gameObject.activeInHierarchy && newCube.CubeNumber == _placedCubes.Value[i].CubeNumber && !_placedCubes.Value[i].IsMainCube && _placedCubes.Value[i] != newCube)
						{
							float tempDistance = Vector3.Distance(newCube.gameObject.transform.position, _placedCubes.Value[i].gameObject.transform.position);
							if (tempDistance < Distance)
							{
								cubeToMergeWith = _placedCubes.Value[i];
								Debug.Log(cubeToMergeWith.name);
								Distance = tempDistance;
							}
						}
					}
					// push the new cube up and forward: 
					var pushForce = 2.5f;
					if (cubeToMergeWith != null)
					{
						pushDirection = (cubeToMergeWith.gameObject.transform.position - newCube.gameObject.transform.position);
						newCube.CubeRgidbody.AddForce(new Vector3(pushDirection.x, .3f, pushDirection.z) * pushForce, ForceMode.Impulse);
						Debug.Log(new Vector3(pushDirection.x, .3f, pushDirection.z));
					}
					else
					{
						newCube.CubeRgidbody.AddForce(new Vector3(0, .3f, 1f) * pushForce, ForceMode.Impulse);
					}


					// add some torque
					var randomValue = Random.Range(-20f, 20f);
					var randomDirection = Vector3.one * randomValue;
					newCube.CubeRgidbody.AddTorque(randomDirection);
				}

				// the explosion should affect surrounded cubes too:
				var surroundedCubes = Physics.OverlapSphere(contactPoint, 2f);
				var explosionForce = 400f;
				var explosionRadius = 1.5f;
				foreach (var coll in surroundedCubes)
					if (coll.attachedRigidbody != null)
						coll.attachedRigidbody.AddExplosionForce(explosionForce, contactPoint, explosionRadius);


				//_FX.PlayCubeExplosionFX(contactPoint, otherCube.CubeColor);


				//Destroy the two cubes:
				
				CubeSpawner.Instance.DestroyCube(otherCube);
				Destroy(this.gameObject);
			}
		}
	}
}
