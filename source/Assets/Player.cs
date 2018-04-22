using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	private Animator animator;
	private Transform grabPoint;

	void Start()
	{
		//Disable the camera if it's not local
		if (!this.isLocalPlayer)
		{
			this.GetComponentInChildren<Camera>().enabled = false;
		}

		this.animator = this.GetComponent<Animator>();
	}

	void Update()
	{
		//Don't run the update code if we're not local
		if (!this.isLocalPlayer)
		{
			return;
		}

		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		this.transform.Rotate(0, x, 0);
		this.transform.Translate(0, 0, z);
	}

	void OnTriggerEnter(Collider col)
	{
		this.grabPoint = col.transform.GetChild(0);
		col.transform.parent = this.transform;
	}

	void OnAnimatorIK()
	{
		if (this.grabPoint != null)
		{
			this.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
			this.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
			this.animator.SetIKPosition(AvatarIKGoal.RightHand, this.grabPoint.position);
			this.animator.SetIKRotation(AvatarIKGoal.RightHand, this.grabPoint.rotation);
		}
		else
		{
			this.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
			this.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
			this.animator.SetLookAtWeight(0);
		}
	}
}