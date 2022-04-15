using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;


/// <summary>
/// A class of useful functions to help with debugging 
/// </summary>
public class Bobby
{

    private static System.Random random = new System.Random();

    private static bool _debug = true;

    private static string    _white = "<color=white>",
                             _green = "<color=green>",
                             _red = "<color=red>",
                             _blue = "<color=blue>",
                             _magenta = "<color=magenta>",
                             _purple = "<color=purple>",
                             _cyan = "<color=cyan>",
                             _yellow = "<color=yellow>",
                             _lime = "<color=lime>",
                             _aqua = "<color=aqua>",
                             _orange = "<color=orange>",
                             _gray = "<color=black>",
                             _end = "</color>";

    public static string White(string s)
    {
        return _white + s + _end;
    }
    public static string Green(string s)
    {
        return _green + s + _end;
    }
    public static string Red(string s)
    {
        return _red + s + _end;
    }
    public static string Blue(string s)
    {
        return _blue + s + _end;
    }
    public static string Orange(string s)
    {
        return _orange + s + _end;
    }
    public static string Magenta(string s)
    {
        return _magenta + s + _end;
    }
    public static string Purple(string s)
    {
        return _purple + s + _end;
    }
    public static string Cyan(string s)
    {
        return _cyan + s + _end;
    }
    public static string Yellow(string s)
    {
        return _yellow + s + _end;
    }
    public static string Lime(string s)
    {
        return _lime + s + _end;
    }
    public static string Aqua(string s)
    {
        return _aqua + s + _end;
    }
    public static string Gray(string s)
    {
        return _gray + s + _end;
    }

    /// <summary>
    /// Returns a string with the specified hex color. ex: "FABBCC"
    /// </summary>
    /// <param name="s"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string Color(string s, string color)
    {
        return "<color=#" + color + ">" + s + _end;
    }

    /// <summary>
    /// Returns a string with the specified unity color.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string Color(string s, Color color)
    {
        string hex = "";
        hex += ((int)(color.r * 255)).ToString("X2");
        hex += ((int)(color.g * 255)).ToString("X2");
        hex += ((int)(color.b * 255)).ToString("X2");

        return Color(s, hex);
    }

    /// <summary>
    /// Returns a string with the specified RGB values (0-255)
    /// </summary>
    /// <param name="s"></param>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static string Color(string s, int r, int g, int b)
    {
        return Color(s, new Color(r / 255f, g / 255f, b / 255f));
    }

   

    /// <summary>
    /// Returns a string with rainbow colors
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string Rainbow(string s)
    {

        return Bobby.MultiGradient(s, new Color[] { UnityEngine.Color.red,
                                                    UnityEngine.Color.magenta,
                                                    UnityEngine.Color.blue,
                                                    UnityEngine.Color.cyan,
                                                    UnityEngine.Color.green,
                                                    UnityEngine.Color.yellow,
                                                    UnityEngine.Color.red });

    }

    /// <summary>
    /// Returns a string with a gradient between two unity colors
    /// </summary>
    /// <param name="s"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static string Gradient(string s, Color a, Color b)
    {
        string final = "";
        float step = 1f / s.Length;
        for (int i = 0; i < s.Length; i++)
        {
            final += Color(s[i].ToString(), UnityEngine.Color.Lerp(a, b, i * step));
        }
        return final;
    }

    /// <summary>
    /// Returns a string with a gradient of multiple unity colors
    /// </summary>
    /// <param name="s"></param>
    /// <param name="colors"></param>
    /// <returns></returns>
    public static string MultiGradient(string s, Color[] colors)
    {
        if (colors.Length == 0 || s.Length == 0)
        {
            return "";
        }

        int step = s.Length / colors.Length;
        string final = "";

        final += Color(s.Substring(0, step / 2), colors[0]);
        int i = 0;
        for (; i < colors.Length - 1; i++)
        {
            final += Gradient(s.Substring(i * step + (step / 2), step), colors[i], colors[i + 1]);
        }
        final += Color(s.Substring(i * step + (step / 2)), colors[i]);

        return final;
    }

    /// <summary>
    /// Logs a string with timestamp
    /// </summary>
    /// <param name="s"></param>
    public static void Log(string s)
    {
        if (_debug)
            Debug.LogWarning(Color("[", 171, 157, 242) + Color(DateTime.Now.ToString("h:mm:ss"), 94, 246, 133) + Color("] - ", 171, 157, 242) + Cyan(s));


    }

    /// <summary>
    /// Logs a string with rainbow colors
    /// </summary>
    /// <param name="s"></param>
    public static void RainbowLog(string s)
    {
        Log(Rainbow(s));
    }



    /// <summary>
    /// Converts a unity symbol into a colored ascii string.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    
   

}