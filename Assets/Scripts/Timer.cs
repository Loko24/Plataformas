using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    [SerializeField]
    private TMP_Text _timeInfo;
    private TimeSpan _timeCount;
    private bool _isTiming;
    private float _elapsedTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _timeInfo.text = "T 00:00";
        _isTiming = false;

        StartTimer();
    }

    /// <summary>
    /// Start time counter
    /// </summary>
    public void StartTimer()
    {
        _isTiming = true;
        _elapsedTime = 0f;

        StartCoroutine(TimerUpdate());
    }

    /// <summary>
    /// End time counter
    /// </summary>
    public void EndTimer()
    {
        _isTiming = false;
    }

    /// <summary>
    /// Act like update, to increase time by secs
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimerUpdate()
    {
        while (_isTiming)
        {
            _elapsedTime += Time.deltaTime;
            _timeCount = TimeSpan.FromSeconds(_elapsedTime);
            _timeInfo.text = string.Format($"T {_timeCount.ToString("mm':'ss")}");

            yield return null;
        }
    }
}
