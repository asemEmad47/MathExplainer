using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathTricks : MonoBehaviour
{
    public static int GetGCF(int FNum,int SNum)
    {
        for (int i = Math.Min(FNum, SNum); i >= 2; i--) {
            if(FNum %i ==0 && SNum % i == 0)
            {
                return i;
            }
        }
        return 1;
    }

    public static int GetNextPrime(int num)
    {
        num++;
        while (!IsPrime(num))
        {
            num++;
        }
        return num;
    }
    public static int GetPrevPrime(int num)
    {
        num--;
        while (num > 1 && !IsPrime(num))
        {
            num--;
        }
        return num > 1 ? num : -1; 
    }

    private static bool IsPrime(int num)
    {
        if (num < 2) return false;
        for (int i = 2; i * i <= num; i++)
        {
            if (num % i == 0)
            {
                return false;
            }
        }
        return true;
    }
}
