using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeControl;

public abstract class VoidMessageBase : Message
{
    public abstract void Send();
}
public abstract class StringMessageBase : Message
{
    public string val;
    public abstract void Send();
}
public abstract class BoolMessageBase : Message
{
    public bool val;
    public abstract void Send();
}