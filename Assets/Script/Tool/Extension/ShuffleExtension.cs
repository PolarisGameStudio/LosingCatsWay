using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShuffleExtension
{
    public static void Shuffle<T>(this T[] array, int shuffleAccuracy)
    {
        if (array.Length <= 1)
            return;
        
        for (int i = 0; i < shuffleAccuracy; i++)
        {
            int randomIndex = Random.Range(1, array.Length);

            T temp = array[randomIndex];
            array[randomIndex] = array[0];
            array[0] = temp;
        }
    }

    public static void Shuffle<T>(this List<T> list, int shuffleAccuracy)
    {
        if (list.Count <= 1)
            return;
        
        for (int i = 0; i < shuffleAccuracy; i++)
        {
            int randomIndex = Random.Range(1, list.Count);

            T temp = list[randomIndex];
            list[randomIndex] = list[0];
            list[0] = temp;
        }
    }

    //0524 //https://stackoverflow.com/questions/273313/randomize-a-listt
    public static System.Random rand = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int count = list.Count;

        while (count > 1)
        {
            count--;

            int index = rand.Next(count + 1);

            T value = list[index];
            list[index] = list[count];
            list[count] = value;
        }
    }
}