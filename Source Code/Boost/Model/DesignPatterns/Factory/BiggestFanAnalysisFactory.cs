﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using A20_EX02_Idan_203315098_Dolev_205811797.Model.DataClasses;
using A20_EX02_Idan_203315098_Dolev_205811797.Model.Design_Patterns.Decorator;
using FacebookWrapper.ObjectModel;

namespace A20_EX02_Idan_203315098_Dolev_205811797.Model.Design_Patterns.Factory
{
    public class BiggestFanAnalysiserFactory : AnalysisFactory
    {
        public  SortedValueDictionary<object, int> CreateAnalysis(
            Post.eType i_Type,
            User i_User,
            eTimeSelector i_TimeSelector)
        {
            return base.CreateAnalysis(i_Type, i_User, i_TimeSelector, typeof(BiggestFanAnalysis));
        }
    }
}