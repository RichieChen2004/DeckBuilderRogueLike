using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public abstract void Use(PlayerControl player);
    public abstract void Description();
}