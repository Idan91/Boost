﻿using A20_EX02_Idan_203315098_Dolev_205811797.Model.DataClasses;
using A20_EX02_Idan_203315098_Dolev_205811797.Model.Design_Patterns.Decorator;
using FacebookWrapper.ObjectModel;

namespace A20_EX02_Idan_203315098_Dolev_205811797.Model.Design_Patterns.Factory
{
    public static class VideoDictionaryFactory
    {
        public static SortedValueDictionary<object, int> CreateVideosTimeAnalysisDictionary(
            User i_AnalysisUser,
            eTimeSelector i_TimeFrame)
        {
            return DictionaryFactory.CreateTimeAnalysisDictionary(
                i_AnalysisUser,
                i_TimeFrame,
                i_EType => Post.eType.video == i_EType);
        }

        public static SortedValueDictionary<object, int> CreateVideosBiggestFansAnalysisDictionary(
            User i_AnalysisUser,
            eTimeSelector i_TimeFrame)
        {
            return DictionaryFactory.CreateBiggestFanAnalysisDictionary(i_AnalysisUser, i_TimeFrame, i_EType => Post.eType.video == i_EType);
        }
    }
}
