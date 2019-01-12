﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour {

    public Vector2 UpForce = new Vector2();
    public GameObject explosion;

    private Camera _cam;
    private bool _noGaming = false;

    private void Awake()
    {
        _cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Use this for initialization
    void Start () {
        transform.position = new Vector2(-2f, 0);
        _noGaming = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && !_noGaming)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce(UpForce);
        }
        Vector2 position = _cam.WorldToScreenPoint(transform.position);

        if (position.y > Screen.height || position.y < 0)
        {
            if (!_noGaming) GameOver();
        }
    }

    void OnCollisionEnter2D()
    {
        if (!_noGaming) GameOver();
    }

    private void GameOver()
    {
        _noGaming = false;
        Instantiate(explosion, transform.position, transform.rotation);
        Messenger<bool>.Broadcast(GameEvent.GAMEOVER, true);
    }
}
