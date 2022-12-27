using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
	public static UserType userTypeFromString(string value)
	{
		if (value == "gummyBear")
		{
			return UserType.gummyBear;
		}
		else if (value == "gingerBreadHouse")
		{
			return UserType.gingerBreadHouse;
		}
		else
		{
			return UserType.none;
		}
	}

	public static string userTypeToString(UserType value)
	{
		if (value == UserType.gummyBear)
		{
			return "gummyBear";
		}
		else if (value == UserType.gingerBreadHouse)
		{
			return "gingerBreadHouse";
		}
		else
		{
			return "none";
		}
	}

	public static Color HexToColor(string code)
	{
		if (code.StartsWith("#"))
		{
			code = code.Substring(1);
		}

		if (code.Length != 8)
		{
			throw new System.ArgumentException("Invalid hex code: " + code);
		}

		byte a = byte.Parse(code.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte r = byte.Parse(code.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(code.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(code.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

		return new Color32(r, g, b, a);
	}

	public static string ColorToHex(Color color)
	{
		byte a = (byte)(color.a * 255);
		byte r = (byte)(color.r * 255);
		byte g = (byte)(color.g * 255);
		byte b = (byte)(color.b * 255);

		return $"{a:X2}{r:X2}{g:X2}{b:X2}";
	}
}
