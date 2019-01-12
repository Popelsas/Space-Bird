﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geme_Manger : MonoBehaviour {

    public GameObject player;
    public GameObject[] aster;

    public float gup;
    public float complexityDelta;

    private Camera _cam;
    private readonly float _delta = 0.8f;

    private float _score = 0;
    private float _complexityFactor = 1;

    private bool _gOver = false;
    private float speedGame = 2f;

    private void Awake()
    {
        _cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Messenger<bool>.AddListener(GameEvent.GAMEOVER, GameOver);
    }

    void Start () {
        Instantiate(player);
        InvokeRepeating("InstatceAster", 0f, speedGame);
    }
	
    public void InstatceAster()
    {
        if (Score > complexityDelta) ComplexityFactor += _score / complexityDelta / 20;
        speedGame /= ComplexityFactor * 1.5f;

        float rPos = _cam.orthographicSize - (_cam.orthographicSize - _delta - gup) * Random.value;
        GameObject asterUP = Instantiate(aster[Random.Range(0, aster.Length)]);
        asterUP.transform.position = new Vector2(_cam.orthographicSize *3f, rPos);
        asterUP.GetComponent<Aster_Controller>().up = true;
        asterUP.GetComponent<Aster_Controller>().complexityFactor *= _complexityFactor;

        GameObject asterDown = Instantiate(aster[Random.Range(0, aster.Length)]);
        asterDown.transform.position = new Vector2(_cam.orthographicSize * 3f, rPos - gup - _cam.orthographicSize + _delta);
        asterDown.GetComponent<Aster_Controller>().up = false;

        asterDown.GetComponent<Aster_Controller>().complexityFactor *= _complexityFactor;

    }
    private void Update()
    {
        if (!_gOver) RenderSettings.skybox.SetFloat("_Rotation", Time.time * 10);
    }

    private void FixedUpdate()
    {
        if (!_gOver) Score += 1;
    }

    public float Score
    {
        get { return _score; }
        set
        {
            //_score = Mathf.Clamp(value, 0, 10000); // Если нужен финал игры
            _score = value;
            Messenger<float>.Broadcast(GameEvent.SCORE_CHANGE, _score);
        }
    }
    public float ComplexityFactor
    {
        get { return _complexityFactor; }
        set
        {
            _complexityFactor = value;
            Messenger<float>.Broadcast(GameEvent.LEVEL, _complexityFactor);
        }
    }

    private void GameOver(bool gOver)
    {
        _gOver = gOver;
        Messenger<float>.Broadcast(GameEvent.LEVEL, 1);
    }

    private void OnDestroy()
    {
        Messenger<bool>.RemoveListener(GameEvent.GAMEOVER, GameOver);
    }
}
