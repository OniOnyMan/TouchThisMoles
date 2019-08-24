using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleController : MonoBehaviour
{
    public int Price = 1;
    public float RiseSpeed = 10;
    public float UpLimitMole = 4.34f;
    public float DeclaySpeed = 10;
    public float DownLimitMole = -2.2f;

    public bool IsRose { get; private set; }

    private bool _rise;
    private bool _declay;

    private void Update()
    {
        if (_rise)
            if (transform.position.y < UpLimitMole)
                transform.Translate(transform.up * Time.deltaTime * RiseSpeed);
            else _rise = false;

        if (_declay)
            if (transform.position.y > DownLimitMole)
                transform.Translate(-transform.up * Time.deltaTime * DeclaySpeed);
            else _declay = false;
    }

    public void Declay()
    {
        IsRose = false;
        _rise = false;
        _declay = true;
    }

    public void Rise(float delayTime)
    {
        IsRose = true;
        _rise = true;
        StartCoroutine(WaitForDeclay(delayTime));
    }

    IEnumerator WaitForDeclay(float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        Declay();
    }
}
