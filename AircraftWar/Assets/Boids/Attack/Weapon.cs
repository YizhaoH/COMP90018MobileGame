using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool attack;
    public float maxDetectRange;

    protected abstract void Shoot();
}
