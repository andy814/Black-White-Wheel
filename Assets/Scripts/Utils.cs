using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
public static class Utils
{
    public static List<float> RandX(int x, float sum, float min, float max)
    {
        List<float> randomNumbers = new List<float>();
        Random random = new Random();

        if (x * min > sum || x * max < sum)
        {
            Console.WriteLine("Constraints are not possible.");
            return randomNumbers;
        }

        float remainingSum = sum;

        for (int i = 0; i < x - 1; i++)
        {
            float currentMax = Math.Min(max, remainingSum - (x - i - 1) * min);
            float currentMin = Math.Max(min, remainingSum - (x - i - 1) * max);
            float randomValue = (float)(random.NextDouble() * (currentMax - currentMin) + currentMin);

            randomNumbers.Add(randomValue);
            remainingSum -= randomValue;
        }

        // Ensure the last number makes the sum equal to the target value.
        randomNumbers.Add(remainingSum);

        return randomNumbers;
    }
}
