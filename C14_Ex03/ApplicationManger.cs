using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace C14_Ex03
{
    public class ApplicationManger
    {
        private const int k_NumOfFeeds = 25;
        private int m_FeedIndex;
        private User m_FacebookUser;
        private FacebookObjectCollection<Post> m_FacebookFeed;
        private IPostStatus m_PostStatus;
        private ApplicationConfiguration m_ApplicationConfiguration;

        public enum ePictureSizeMode
        {
            SmallPicture,
            MediumPicture,
            LargePicture,
        }

        public enum eLikeCategory
        {
            Movies,
            Books,
            TVShows,
            Musics
        }

        public ApplicationManger()
        {
            m_FeedIndex = 0;
            m_ApplicationConfiguration = ApplicationConfiguration.GetConfiguration;
        }

        public User FacebookUser
        {
            get
            {
                return this.m_FacebookUser;
            }
        }

        public string GetImageUrlOfUser(ePictureSizeMode i_SizeMode)
        {
            string urlOfImage;
            try
            {
                switch (i_SizeMode)
                {
                    case ePictureSizeMode.SmallPicture:
                        urlOfImage = m_FacebookUser.PictureSmallURL;
                        break;
                    case ePictureSizeMode.MediumPicture:
                        urlOfImage = m_FacebookUser.PictureNormalURL;
                        break;
                    case ePictureSizeMode.LargePicture:
                        urlOfImage = m_FacebookUser.PictureLargeURL;
                        break;
                    default:
                        urlOfImage = null;
                        break;
                }
            }
            catch(Facebook.FacebookOAuthException)
            {
                urlOfImage = null;
            }

            return urlOfImage;
        }

        public string GetUserCurrentStatus()
        {
            string currentUserStatus;
            FacebookObjectCollection<Status> userStatus = m_FacebookUser.Statuses;
           
            if (userStatus.Count == 0)
            {
                currentUserStatus = null;
            }
            else
            {
                currentUserStatus = userStatus[0].Message;
            }

            return currentUserStatus;
        }

        public void PostOnUserWall(string i_Messege)
        {
            m_PostStatus.PostStatus(i_Messege);
            m_FacebookUser.ReFetch();
        }

        public FacebookObjectCollection<Photo> GetUserRecentPhoto()
        {
            FacebookObjectCollection<Photo> userPhotos = m_FacebookUser.PhotosTaggedIn;
            return userPhotos;
        }

        public LoginResult ConnectToFacebook()
        {
            LoginResult loginSession;
            string applicationId = m_ApplicationConfiguration.GetApplicationId;
            List<string> applicationPermission = m_ApplicationConfiguration.GetCopyOfAccessPermission();
            loginSession = FacebookService.Login(applicationId, applicationPermission.ToArray());

            m_FacebookUser = loginSession.LoggedInUser;
            m_PostStatus = new DecoratedUserBackupPost(new CoredUser(m_FacebookUser));
            return loginSession;
        }

        public string FetchCurrentFeedMessege()
        {
            string currentFeedText;
            m_FacebookFeed = m_FacebookUser.NewsFeed;
            currentFeedText = m_FacebookFeed[m_FeedIndex].Message;
            return currentFeedText;
        }

        public string FetchFeedlerFullDetails()
        {
            string fullFeedlerDetails;
            string updateTime = m_FacebookFeed[m_FeedIndex].UpdateTime.ToString();
            string createdTime = m_FacebookFeed[m_FeedIndex].CreatedTime.ToString();

            fullFeedlerDetails = string.Format("Created At : {0}.{1}Last updated at : {2}.", createdTime, Environment.NewLine, updateTime);

            return fullFeedlerDetails;
        }

        public string FetchFeedlerPhoto()
        {
            string feedlerPhotoUrl;
            feedlerPhotoUrl = m_FacebookFeed[m_FeedIndex].From.PictureLargeURL;
            return feedlerPhotoUrl;
        }

        public string FetchFeedStatistics()
        {
            string state;
            int numOfComments;
            int numOfLike;

            numOfComments = m_FacebookFeed[m_FeedIndex].Comments.Count;
            numOfLike = m_FacebookFeed[m_FeedIndex].LikedBy.Count;

            state = string.Format("{0} Peoples Liked this,{1}{2} Peoples Commented on this.", numOfLike, Environment.NewLine, numOfComments);
            return state;
        }

        public string FetchFeedlerName()
        {
            string feedlerName;
            feedlerName = m_FacebookFeed[m_FeedIndex].From.Name;
            return feedlerName;
        }

        public void ReloadUser()
        {
            m_FacebookUser.ReFetch();
        }

        public void SetFeedIndexToZero()
        {
            this.m_FeedIndex = 0;
        }

        public void DecreaseFeedIndexByOneCircularWay()
        {
            if(this.m_FeedIndex == 0)
            {
                this.m_FeedIndex = k_NumOfFeeds - 1;
            }
            else
            {
                this.m_FeedIndex--;
            }    
        }

        public void IncreaseFeedIndexByOneCircularWay()
        {
            if (this.m_FeedIndex == k_NumOfFeeds - 1)
            {
                this.m_FeedIndex = 0;
            }
            else
            {
                this.m_FeedIndex++;
            }
        }

        public FacebookObjectCollection<Page> FetchLikes(string i_LikeCtegory, string i_UserID)
        {
            FacebookObjectCollection<Page> likes = FacebookService.GetCollection<Page>(i_LikeCtegory, i_UserID);
            return likes;
        }

        public FacebookObjectCollection<User> FetchMyfriends()
        {
            FacebookObjectCollection<User> friends = FacebookService.GetCollection<User>("friends");
            return friends;
        }

        public FacebookObjectCollection<Event> FetchUserEvents(User i_UserToFetchEvents)
        {
            FacebookObjectCollection<Event> Events = FacebookService.GetCollection<Event>("Events", i_UserToFetchEvents.Id);
            return Events;
        }

        public string GetUserProfileImage(User i_User)
        {
            return i_User.PictureLargeURL;
        }
    }
}
