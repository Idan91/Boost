﻿using Boost.Model.DataClasses;
using Boost.Model.DesignPatterns.Decorator;
using FacebookWrapper.ObjectModel;

namespace Boost.Model.DesignPatterns.Factory.FactoryMethod
{
    public static class PhotoDictionaryFactory
    {
        private static readonly DictionaryFactory sr_BasicFactory = new DictionaryFactory();

        public static SortedValueDictionary<object, int> CreatePhotoTimeAnalysisDictionary(
            User i_AnalysisUser,
            eTimeFrame i_TimeFrame)
        {
            return sr_BasicFactory.CreateTimeAnalysisDictionary(
                i_AnalysisUser,
                i_TimeFrame,
                i_EType => Post.eType.photo == i_EType);
        }

        public static SortedValueDictionary<object, int> CreatePhotoBiggestFansAnalysisDictionary(
            User i_AnalysisUser,
            eTimeFrame i_TimeFrame)
        {
            return sr_BasicFactory.CreateBiggestFanAnalysisDictionary(i_AnalysisUser, i_TimeFrame, i_EType => Post.eType.photo == i_EType);
        }
    }   
}