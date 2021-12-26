using System.Collections;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using UnityEngine;
using CodeControl;
[SkipRename]
public abstract class VoidMessageBase : Message
{
    public abstract void Send();
}
[SkipRename]
public abstract class StringMessageBase : Message
{
    public string val;
    public abstract void Send();
}
[SkipRename]
public abstract class BoolMessageBase : Message
{
    public bool val;
    public abstract void Send();
}