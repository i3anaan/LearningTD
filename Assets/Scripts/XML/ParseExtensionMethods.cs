using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ExtensionMethods
{
    public static class ParseExtensionMethods
    {
        public static Int32[] Parse(this Int32[] ints, string[] arr)
        {
            Int32[] output = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                output[i] = Int32.Parse(arr[i]);
            }
            return output;
        }
    }
}