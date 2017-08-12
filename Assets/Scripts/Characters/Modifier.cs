using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A modifier encapsulates the idea of a buff or debuff. It can be multiplicative or additive (or both).
 */
public class Modifier {
    public float additive;
    public float multiplicative;
    public Stat stat;

    public Modifier(Stat s, float add_me, float multiply_me) {
        this.stat = s;
        this.additive = add_me;
        this.multiplicative = multiply_me;
    }

    public override bool Equals(object obj)
    {
        var item = obj as Modifier;

        if (item == null)
        {
            return false;
        }

        return this.stat == item.stat
            && this.additive == item.additive
            && this.multiplicative == item.multiplicative;
    }

    public override int GetHashCode()
    {
        return stat.GetHashCode() ^ additive.GetHashCode() ^ multiplicative.GetHashCode();
    }
}
