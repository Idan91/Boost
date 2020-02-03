﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Boost.Model.DesignPatterns.Decorator
{
    public class SortedValueDictionary<TK, TV> : DictionaryDecorator<TK, TV>
        where TV : IComparable
    {
        public KeyValuePair<TK, TV>[] SortByValue()
        {
            KeyValuePair<TK, TV>[] o_SortedDictionaryValues = r_Dictionary.ToArray();

            Array.Sort(o_SortedDictionaryValues, (pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            return o_SortedDictionaryValues;
        }

        public SortedValueDictionary(IDictionary<TK, TV> i_Dictionary)
            : base(i_Dictionary)
        {
        }
    }
}