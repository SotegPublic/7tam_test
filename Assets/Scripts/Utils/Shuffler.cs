using System;
using System.Collections.Generic;

public static class Shuffler
{
    public static void ShuffleArray<T>(T[] array)
    {
        var rnd = new Random();

        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            var tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }
    }

    public static void ShuffleList<T>(List<T> list)
    {
        var rnd = new Random();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            var tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }
}
