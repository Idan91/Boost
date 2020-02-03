﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using Facebook;
using Boost.Model.DataClasses;
using Boost.Model.Design_Patterns;
using Boost.Model.DesignPatterns.Factory.AnalysisFactory;

namespace Boost.Model
{
    public class BoostEngine
    {
        #region Data Members & Properties

        #region Instance

        public static BoostEngine Instance => Singleton<BoostEngine>.Instance;

        #endregion

        #region Data Members

        private const int k_CollectionLimit = 20;

        public const int k_NumOfBiggestFans = 3;

        public const int k_NumOfPostsForEngagement = 10;

        private const string k_AppId = "748532218946260";

        public BoostSettings m_BoostSettings; // Default null

        public const string k_PostErrorMessage = "Could not get post!";

        public static readonly string SR_CurrentVersion = "0.3.0";

        public eTimeFrame m_CurrentAnalysisTimeFrame;

        public eAnalysisDataBasis m_CurrentAnalysisDataBasis;

        public IAnalysisFactory m_AnalysisFactory = typeof(TimeAnalysisFactory).CreateFactory(); // default with TimeAnalysis

        private static string s_AnalysisFactoryLock = "LockyLockyAnalysisLock";

        private static string s_FactorySwitchLock = "LockyLockySwitchLock";

        #endregion

        #region Properties

        public int FriendChange { get; set; }

        public User LoggedInUser { get; set; }

        public LoginResult LoginResult { get; set; }

        #endregion

        #endregion

        #region Ctor

        private BoostEngine()
        {
            try
            {
                m_BoostSettings = BoostSettings.LoadAppSettingsFromFile();
                m_CurrentAnalysisTimeFrame = m_BoostSettings.DefaultAnalysisTimeFrame;
                m_CurrentAnalysisDataBasis = m_BoostSettings.DefaultAnalysisDataBasis;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

            FriendChange = 0;
        }

        #endregion

        #region Methods
        public void FacebookLogin(string i_AccessToken, bool i_RememberUser)
        {
            FacebookService.s_CollectionLimit = k_CollectionLimit;
            User o_LoggedInUser = null;
            if(i_AccessToken != null && i_RememberUser)
            {
                try
                {
                    LoginResult = FacebookService.Connect(i_AccessToken);
                }
                catch(Exception e)
                {
                    throw new FacebookApiException("Connection failed!", e);
                }
            }
            else
            {
                try
                {
                    LoginResult = FacebookService.Login(
                        k_AppId,
                        "public_profile",
                        "email",
                        "publish_to_groups",
                        "user_birthday",
                        "user_age_range",
                        "user_gender",
                        "user_link",
                        "user_tagged_places",
                        "user_videos",
                        "publish_to_groups",
                        "groups_access_member_info",
                        "user_friends",
                        "user_events",
                        "user_likes",
                        "user_location",
                        "user_photos",
                        "user_posts",
                        "user_hometown");
                }
                catch(Exception e)
                {
                    throw new FacebookApiException("Login failed!", e);
                }
            }

            if(!string.IsNullOrEmpty(LoginResult.AccessToken))
            {
                o_LoggedInUser = LoginResult.LoggedInUser;
            }

            LoggedInUser = o_LoggedInUser;
        }
        
        public void FacebookLogout()
        {
            try
            {
                FacebookService.Logout(null);
                LoggedInUser = null;
            }
            catch(Exception e)
            {
                throw new FacebookApiException("Logout failed!", e);
            }
        }

        public void SelectAnalysisFactoryType(Type i_AnalysisFactory)
        {
            lock (s_FactorySwitchLock)
            {
                m_AnalysisFactory = i_AnalysisFactory.CreateFactory();
            }
        }

        public List<eTimeFrame> GetAnalysisTimeFrames()
        {
            List<eTimeFrame> analysisTimeFrames = new List<eTimeFrame>();

            foreach(eTimeFrame val in Enum.GetValues(typeof(eTimeFrame)))
            {
                analysisTimeFrames.Add(val);
            }

            return analysisTimeFrames;
        }

        public List<eAnalysisDataBasis> GetAnalysisDataBases()
        {
            List<eAnalysisDataBasis> analysisDataBases = new List<eAnalysisDataBasis>();

            foreach(eAnalysisDataBasis val in Enum.GetValues(typeof(eAnalysisDataBasis)))
            {
                analysisDataBases.Add(val);
            }

            return analysisDataBases;
        }

        public Analysis CreateAnalysisUsingFactory(eTimeFrame i_TimeFrame, eAnalysisDataBasis i_AnalysisDataBasis)
        {
            Analysis analysis;

            lock(s_AnalysisFactoryLock)
            {
                analysis = m_AnalysisFactory.CreateAnalysis(i_AnalysisDataBasis, LoggedInUser, i_TimeFrame);
            }

            return analysis;
        }
        
        public void SaveAnalysisSettings()
        {
            try
            {
                m_BoostSettings.DefaultAnalysisTimeFrame = m_CurrentAnalysisTimeFrame;
                m_BoostSettings.DefaultAnalysisDataBasis = m_CurrentAnalysisDataBasis;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public Post GetLastStatus()
        {
            Post o_LastStatus = null;
            foreach(Post postToSearch in LoggedInUser.Posts)
            {
                if(postToSearch == null)
                {
                    continue;
                }

                if(postToSearch.Type == Post.eType.status && postToSearch.Message != null)
                {
                    o_LastStatus = postToSearch;
                    break;
                }
            }

            return o_LastStatus;
        }

        public Post GetTopPost()
        {
            Post mostLikedPost = null;
            const int k_PostMaxCount = 25;
            int topLikes = 0, postSearchLimiter = 0; ////Due to Facebook not showing likes

            if(LoggedInUser.Posts[0] != null)
            {
                topLikes = LoggedInUser.Posts[0].LikedBy.Count;
                mostLikedPost = LoggedInUser.Posts[0]; ////Due to Facebook not showing likes
            }

            foreach(Post post in LoggedInUser.Posts)
            {
                if(post == null || k_PostMaxCount < postSearchLimiter++)
                {
                    break;
                }
                else
                {
                    if(post.LikedBy.Count < topLikes)
                    {
                        continue;
                    }
                }

                topLikes = post.LikedBy.Count;
                mostLikedPost = post;
            }

            if(mostLikedPost == null)
            {
                throw new NullReferenceException(k_PostErrorMessage);
            }

            return mostLikedPost;
        }

        public void OverwriteBoostSettings() // BoostEngine
        {
            m_BoostSettings.LastLoggedInEmail = LoggedInUser.Email;
            m_BoostSettings.FirstLogin = false;
            m_BoostSettings.LastAccessToken = LoginResult.AccessToken;
            m_BoostSettings.LastLogin = DateTime.Now;
            m_BoostSettings.FirstName = LoggedInUser.FirstName;
            m_BoostSettings.LastUsedVersion = BoostEngine.SR_CurrentVersion;
        }

        #endregion
    }
}