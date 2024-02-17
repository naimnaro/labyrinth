using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgCtrl : MonoBehaviour
{
	public float smoothTimeX, smoothTimeY;

	public Vector2 velocity;

	public GameObject background;

	public Vector2 minPos, maxPos;



	// ĳ���� �ʱ�ȭ

	void Start()
	{

		background = GameObject.FindGameObjectWithTag("Background");


	}



	// ĳ������ ���� ���� ī�޶� �̵��ϵ��� �ϴ� �޼���

	void FixedUpdate()
	{

		float posX = Mathf.SmoothDamp(transform.position.x, background.transform.position.x, ref velocity.x, smoothTimeX);

		// Mathf.SmoothDamp�� õõ�� ���� ������Ű�� �޼����̴�.

		float posY = Mathf.SmoothDamp(transform.position.y, background.transform.position.y, ref velocity.y, smoothTimeY);

		// ī�޷� �̵�

		transform.position = new Vector3(posX, 0, transform.position.z);



	}
}
