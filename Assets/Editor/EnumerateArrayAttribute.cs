/*
 * Used to enumaerate array fields by an enum in the inspector.
 */
using System;
using UnityEngine;

public class EnumerateArrayAttribute : PropertyAttribute
{
    public readonly Type enumType;


    public EnumerateArrayAttribute(Type enumType)
    {
        if (!enumType.IsEnum)
        {
            Debug.LogError("Invalid Enum Type: " + enumType.ToString());
        }
        this.enumType = enumType;
    }
}
