﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainAllyGunController : MonoBehaviour
{
	public GameObject bullet;
	public int health;
	public float coolDownPeriodInSeconds;

	public Text allyHealtText;
	private AudioManager audioManager;

	private float timeStamp;
	// Start is called before the first frame update
	void Start()
    {
		audioManager = FindObjectOfType<AudioManager>();
		timeStamp = Time.time;

	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "bullet" && collision.gameObject.GetComponent<BulletController>().allyTag != this.tag)
		{
			health -= collision.GetComponent<BulletController>().damage;
		}

	}

	// Update is called once per frame
	void Update()
    {
		if (health <= 0)
		{
			GameOver();
		}
		allyHealtText.text = "Your HP: " + health.ToString();
		// convert mouse position into world coordinates
		Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// get direction you want to point at
		Vector2 direction = (mouseWorldPosition - (Vector2)transform.position).normalized;

		// set vector of transform directly
		transform.up = direction;

		if (Input.GetMouseButtonDown(0))
		{
			Fire();
		}
	}

	private void GameOver()
	{
		SceneManager.LoadScene("GameOverScreen");
	}

	private void Fire()
	{
		if (timeStamp > Time.time)
		{
			return;
		}
		GameObject bul = Instantiate(bullet, transform.GetChild(1).position, transform.GetChild(1).rotation);
		bul.GetComponent<Rigidbody2D>().velocity = transform.GetChild(1).up * 10;
		bul.GetComponent<BulletController>().allyTag = this.tag;
		audioManager.Play("Fire");
		timeStamp = Time.time + coolDownPeriodInSeconds;
	}
}
