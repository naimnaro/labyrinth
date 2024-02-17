using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgCtrl : MonoBehaviour
{
	public float smoothTimeX, smoothTimeY;

	public Vector2 velocity;

	public GameObject background;

	public Vector2 minPos, maxPos;



	// 캐릭터 초기화

	void Start()
	{

		background = GameObject.FindGameObjectWithTag("Background");


	}



	// 캐릭터의 위에 따라 카메라가 이동하도록 하는 메서드

	void FixedUpdate()
	{

		float posX = Mathf.SmoothDamp(transform.position.x, background.transform.position.x, ref velocity.x, smoothTimeX);

		// Mathf.SmoothDamp는 천천히 값을 증가시키는 메서드이다.

		float posY = Mathf.SmoothDamp(transform.position.y, background.transform.position.y, ref velocity.y, smoothTimeY);

		// 카메로 이동

		transform.position = new Vector3(posX, 0, transform.position.z);



	}
}
