using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace C14_Ex03
{
    public partial class MainForm : Form
    {
        private const int k_LoginFormHeight = 230;
        private const int k_LoginFormWidth = 290;
        private const int k_ApplicationHeight = 625;
        private const int k_ApplicationWidth = 1068;
        private const int k_StartingPositionXOfApplication = 150;
        private const int k_StartingPositionYOfApplication = 150;
        private const int k_PictureBoxOfUserPhotoWidth = 120;
        private const int k_PictureBoxOfUserPhotoHeight = 120;
        private const int k_SpaceBeteewnElementInFirstRowInMyZone = 40;
        private const int k_NumberOfTaggedInPicturesPerRow = 6;
        private ApplicationManger m_Manger;
        private ApplicationManger.eLikeCategory m_Category;
        private ReminderManufacturer m_ReminderManufacturer;
        private IReminderBuilder m_ReminderBuilder;
        private UserEvent m_UserEvents;
        private Matcher m_MatchFriendsByParameter;

        private enum eCriterion 
        {
            Age,
            City,
            Country,
            Gender
        }

        public MainForm()
        {
            this.m_Manger = new ApplicationManger();
            InitializeComponent();
            m_ReminderBuilder = null;
            m_ReminderManufacturer = new ReminderManufacturer();
            this.Size = new Size(k_LoginFormWidth, k_LoginFormHeight);
            FacebookService.s_UseForamttedToStrings = true;
            m_MatchFriendsByParameter = new Matcher();
        }

        private void buttonConnectWithFacebook_Click(object sender, EventArgs e)
        {
            LoginResult resultFromFacebookLogin = m_Manger.ConnectToFacebook();

            if (!string.IsNullOrEmpty(resultFromFacebookLogin.AccessToken))
            {
                doActionsWhenSuccessLoggin(resultFromFacebookLogin.LoggedInUser);
            }
            else
            {
                MessageBox.Show("Oh no, something went wrong.... try again later");
            }
        }

        private void doActionsWhenSuccessLoggin(User i_LoggedInUser)
        {
            changeFromLoginPanelToLandingPanel();
        }

        private void changeFromLoginPanelToLandingPanel()
        {
            loadLandingPage();
        }

        private void loadLandingPage()
        {
            bool allowMaximaize = false;
            bool allowMinimize = true;
            switchPanels(this.panelLogin, this.panelLandingPage);
            changeFormPropertiesWhenSwitchingPanels(k_ApplicationWidth, k_ApplicationHeight, k_StartingPositionXOfApplication, k_StartingPositionYOfApplication, allowMaximaize, allowMinimize);
        }

        private void setPictureBoxWithSizeProperties(PictureBox i_PictureBoxToBeSetUp, int i_PBWidth, int i_PBHeight)
        {
            i_PictureBoxToBeSetUp.Size = new Size(i_PBWidth, i_PBHeight);
            i_PictureBoxToBeSetUp.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void setPictureBoxWithImage(PictureBox i_PictureBoxToBeSetUp, ApplicationManger.ePictureSizeMode i_SizeMode)
        {
            string urlOfImage = this.m_Manger.GetImageUrlOfUser(i_SizeMode);

            if (string.IsNullOrEmpty(urlOfImage) == true)
            {
                i_PictureBoxToBeSetUp.Image = Properties.Resources.IconAvatar;
            }
            else
            {
                i_PictureBoxToBeSetUp.ImageLocation = urlOfImage;
            }

            i_PictureBoxToBeSetUp.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void changeFormPropertiesWhenSwitchingPanels(int i_NewFormWidth, int i_NewFormHight, int i_NewFormStartPositionXCoord, int i_NewFormStartPositionYCoord, bool i_AllowMaximaize, bool i_AllowMinimize)
        {
            this.MaximizeBox = i_AllowMaximaize;
            this.MinimizeBox = i_AllowMinimize;

            this.Size = new Size(i_NewFormWidth, i_NewFormHight);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(i_NewFormStartPositionXCoord, i_NewFormStartPositionYCoord);
        }

        private void switchPanels(Panel i_From, Panel i_To)
        {
            i_From.Visible = false;
            i_To.Visible = true;
        }

        private void pictureBoxTileMeProfile_Click(object sender, EventArgs e)
        {
            switchPanels(this.panelLandingPage, this.panelMeProfile);
            setMeProfilePanel();
        }

        private void setMeProfilePanel()
        {
            Thread thread = new Thread(placeTaggedInPhotosPicturesOnPictureBox);
            thread.Start();
            setPictureBoxWithImage(this.pictureBoxMeProfileInMeZonePage, ApplicationManger.ePictureSizeMode.LargePicture);
            this.textBoxMyStatusInMyZonePage.Text = this.m_Manger.GetUserCurrentStatus();
        }

        private void richTextBoxUploadPostInMeZonePage_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.richTextBoxUploadPostInMeZonePage.Text) == true)
            {
                this.buttonUploadPostInMeZonePage.Enabled = false;
            }
            else
            {
                this.buttonUploadPostInMeZonePage.Enabled = true;
            }
        }

        private void buttonUploadPostInMeZonePage_Click(object sender, EventArgs e)
        {
            m_Manger.PostOnUserWall(this.richTextBoxUploadPostInMeZonePage.Text);
            this.richTextBoxUploadPostInMeZonePage.Clear();
            this.textBoxMyStatusInMyZonePage.Text = this.m_Manger.GetUserCurrentStatus();
            MessageBox.Show("Successfully post to your wall");
        }

        private void placeTaggedInPhotosPicturesOnPictureBox()
        {
            FacebookObjectCollection<Photo> userPhotos = m_Manger.GetUserRecentPhoto();
            int placedPicture = 0;
            PictureBox currentPB;
            int yCoords = this.labelTaggedPhotos.Height + 30;
            int xCoords;

            while (placedPicture < k_NumberOfTaggedInPicturesPerRow)
            {
                pictureBoxTaggedPhotoTile.Invoke(new Action(
                    () =>
                    {
                        currentPB = new PictureBox();
                        currentPB.Parent = this.pictureBoxTaggedPhotoTile;
                        currentPB.Size = new Size(k_PictureBoxOfUserPhotoWidth, k_PictureBoxOfUserPhotoHeight);
                        xCoords = this.labelTaggedPhotos.Location.X + (placedPicture * (k_PictureBoxOfUserPhotoWidth + k_SpaceBeteewnElementInFirstRowInMyZone));
                        currentPB.Location = new Point(xCoords, yCoords);
                        currentPB.ImageLocation = userPhotos[placedPicture].URL;
                        currentPB.SizeMode = PictureBoxSizeMode.StretchImage;
                        placedPicture++;
                    }));
            }

            yCoords = this.labelTaggedPhotos.Height + k_PictureBoxOfUserPhotoHeight + 60;
            while (placedPicture < 2 * k_NumberOfTaggedInPicturesPerRow)
            {
                pictureBoxTaggedPhotoTile.Invoke(new Action(
                   () =>
                   {
                       currentPB = new PictureBox();
                       currentPB.Parent = this.pictureBoxTaggedPhotoTile;
                       currentPB.Size = new Size(k_PictureBoxOfUserPhotoWidth, k_PictureBoxOfUserPhotoHeight);
                       xCoords = this.labelTaggedPhotos.Location.X + ((placedPicture - k_NumberOfTaggedInPicturesPerRow) * (k_PictureBoxOfUserPhotoWidth + k_SpaceBeteewnElementInFirstRowInMyZone));
                       currentPB.Location = new Point(xCoords, yCoords);
                       currentPB.ImageLocation = userPhotos[placedPicture].URL;
                       currentPB.SizeMode = PictureBoxSizeMode.StretchImage;
                       placedPicture++;
                   }));
            }
        }

        private void labelMeZone_Click(object sender, EventArgs e)
        {
            switchPanels(this.panelLandingPage, this.panelMeProfile);
            setMeProfilePanel();
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switchPanels(this.panelMeProfile, this.panelLandingPage);
        }

        private void labelMyFeed_Click(object sender, EventArgs e)
        {
            setUpAndLoadMyFeedPage();
        }

        private void pictureBoxMyFeed_Click(object sender, EventArgs e)
        {
            setUpAndLoadMyFeedPage();
        }

        private void setUpAndLoadMyFeedPage()
        {
            Thread thread = new Thread(loadComponentsOfFeedPageAccordingToCurrentFeed);
            thread.Start();
            switchPanels(this.panelLandingPage, this.panelMyFeed);
        }

        private void loadComponentsOfFeedPageAccordingToCurrentFeed()
        {
            Thread thread = new Thread(loadFeedMessege);
            thread.Start();
            thread = new Thread(new ThreadStart(loadFeedStates));
            thread.Start();
            thread = new Thread(new ThreadStart(loadFeedlerName));
            thread.Start();
            thread = new Thread(new ThreadStart(loadFeedlerImage));
            thread.Start();
            thread = new Thread(new ThreadStart(loadFeedlerFullDetails));
            thread.Start();
        }

        private void loadFeedlerFullDetails()
        {
            richTextBoxFeedlerDetails.Invoke(new Action(
                   () =>
                   {
                       this.richTextBoxFeedlerDetails.Text = m_Manger.FetchFeedlerFullDetails();
                   }));
        }

        private void loadFeedlerImage()
        {
            pictureBoxFeedlerPhoto.Invoke(new Action(
                   () =>
                   {
                       this.pictureBoxFeedlerPhoto.ImageLocation = m_Manger.FetchFeedlerPhoto();
                       this.pictureBoxFeedlerPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
                   }));
        }

        private void loadFeedlerName()
        {
            richTextBoxTheFeedler.Invoke(new Action(
                   () =>
                   {
                       this.richTextBoxTheFeedler.Text = m_Manger.FetchFeedlerName();
                   }));
        }

        private void loadFeedMessege()
        {
            richTextBoxCurrentFeed.Invoke(new Action(
                   () =>
                   {
                       this.richTextBoxCurrentFeed.Text = m_Manger.FetchCurrentFeedMessege();
                   }));
        }

        private void loadFeedStates()
        {
            richTextBoxStats.Invoke(new Action(
                   () =>
                   {
                       this.richTextBoxStats.Text = m_Manger.FetchFeedStatistics();
                   }));
        }

        private void toolStripMenuItemHomeInMyFeedPage_Click(object sender, EventArgs e)
        {
            switchPanels(this.panelMyFeed, this.panelLandingPage);
            m_Manger.ReloadUser();
        }

        private void buttonPrevFeed_Click(object sender, EventArgs e)
        {
            m_Manger.DecreaseFeedIndexByOneCircularWay();
            loadComponentsOfFeedPageAccordingToCurrentFeed();
        }

        private void buttonNextFeed_Click(object sender, EventArgs e)
        {
            m_Manger.IncreaseFeedIndexByOneCircularWay();
            loadComponentsOfFeedPageAccordingToCurrentFeed();
        }

        private void pictureBoxLikes_Click(object sender, EventArgs e)
        {
            switchPanels(this.panelLandingPage, this.panelLikes);
        }

        private void labelLikes_Click(object sender, EventArgs e)
        {
            switchPanels(this.panelLandingPage, this.panelLikes);
        }

        private void buttonMovies_Click(object sender, EventArgs e)
        {
            m_Category = ApplicationManger.eLikeCategory.Movies;
            doWhenButtonClicked();
            FacebookObjectCollection<Page> movies = m_Manger.FetchLikes("movies", "me");
            foreach (Page movie in movies)
            {
                listBoxLikes.Items.Add(movie.Name);
            }
        }

        private void buttonBooks_Click(object sender, EventArgs e)
        {
            m_Category = ApplicationManger.eLikeCategory.Books;
            doWhenButtonClicked();
            FacebookObjectCollection<Page> books = m_Manger.FetchLikes("books", "me");
            foreach (Page book in books)
            {
                listBoxLikes.Items.Add(book.Name);
            }
        }

        private void buttonTVShows_Click(object sender, EventArgs e)
        {
            m_Category = ApplicationManger.eLikeCategory.TVShows;
            doWhenButtonClicked();
            FacebookObjectCollection<Page> shows = m_Manger.FetchLikes("television", "me");
            foreach (Page show in shows)
            {
                listBoxLikes.Items.Add(show.Name);
            }
        }

        private void buttonMusic_Click(object sender, EventArgs e)
        {
            m_Category = ApplicationManger.eLikeCategory.Musics;
            doWhenButtonClicked();
            FacebookObjectCollection<Page> music = m_Manger.FetchLikes("music", "me");
            foreach (Page song in music)
            {
                listBoxLikes.Items.Add(song.Name);
            }
        }

        private void doWhenButtonClicked()
        {
            listBoxLikes.Items.Clear();
            listBoxFriends.Items.Clear();
            labelLikesList.Text = string.Format("You Like this {0} : ", m_Category.ToString());
            labelFriendsList.Text = string.Format("Friends who like this {0} :", m_Category.ToString().Remove(m_Category.ToString().Length - 1, 1));
        }

        private void toolStripMenuItemHomeLikes_Click_1(object sender, EventArgs e)
        {
            switchPanels(this.panelLikes, this.panelLandingPage);
        }

        private void listBoxLikes_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxFriends.Items.Clear();
            switch (m_Category)
            {
                case ApplicationManger.eLikeCategory.Movies:
                    getFriendsWithSameLike("movies");
                    break;
                case ApplicationManger.eLikeCategory.Books:
                    getFriendsWithSameLike("books");
                    break;
                case ApplicationManger.eLikeCategory.TVShows:
                    getFriendsWithSameLike("television");
                    break;
                case ApplicationManger.eLikeCategory.Musics:
                    getFriendsWithSameLike("music");
                    break;
                default:
                    break;
            }
        }

        private void getFriendsWithSameLike(string category)
        {
            FacebookObjectCollection<Page> likes;
            string selectedLike = listBoxLikes.SelectedItem.ToString();
            FacebookObjectCollection<User> friends = m_Manger.FetchMyfriends();
            foreach (User friend in friends)
            {
                likes = m_Manger.FetchLikes(category, friend.Id);
                foreach (Page like in likes)
                {
                    if (like.Name == selectedLike)
                    {
                        listBoxFriends.Items.Add(friend.Name);
                        break;
                    }
                }
            }
        }

        private void homeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            switchPanels(this.panelSingleFriends, this.panelLandingPage);
            listBoxMaleSingleFriends.Items.Clear();
            listBoxFemaleSingleFriends.Items.Clear();
            pictureBoxSingleFemales.Image = null;
            pictureBoxSingleMale.Image = null;
            listBoxMaleEvents.Items.Clear();
            listBoxFemaleEvents.Items.Clear();
        }

        private void pictureBoxFriends_Click(object sender, EventArgs e)
        {
            changeToFriendsPage();
        }

        private void labelFriends_Click(object sender, EventArgs e)
        {
            changeToFriendsPage();
        }

        private void changeToFriendsPage()
        {
            fetchSingleFriendsLists();
            switchPanels(this.panelLandingPage, this.panelSingleFriends);
        }

        private void fetchSingleFriendsLists()
        {
            User fullFriend;
            bool isSingleFriend;
            bool isMaleFriend;
            FacebookObjectCollection<User> friends = m_Manger.FetchMyfriends();
            foreach (User friend in friends)
            {
                fullFriend = FacebookService.GetObject<User>(friend.Id, User.s_FieldsToLoadFull[DynamicWrapper.eLoadOptions.Full]);
                isSingleFriend = isFriendSingle(fullFriend);

                if (isSingleFriend == true)
                {
                    isMaleFriend = isFriendMale(fullFriend);
                    if (isMaleFriend == true)
                    {
                        listBoxMaleSingleFriends.Items.Add(fullFriend);
                    }
                    else
                    {
                        listBoxFemaleSingleFriends.Items.Add(fullFriend);
                    }
                }
            }
        }

        private void fetchEventsList(User i_Friend, ListBox i_ListToFill)
        {
            m_UserEvents = new UserEvent(i_Friend);
            i_ListToFill.DisplayMember = "Name";

            foreach(Event eventOfUser in m_UserEvents)
            {
                i_ListToFill.Items.Add(eventOfUser);
            }
        }

        private bool isFriendSingle(User i_Friend)
        {
            bool isSingle = false;

            if (i_Friend.RelationshipStatus != null)
            {
                if (i_Friend.RelationshipStatus == User.eRelationshipStatus.Single)
                {
                    isSingle = true;
                }
                else
                {
                    isSingle = false;
                }
            }
            else
            {
                isSingle = false;
            }

            return isSingle;
        }

        private bool isFriendMale(User i_Friend)
        {
            bool isMale;
            if (i_Friend.Gender != null)
            {
                if (i_Friend.Gender == User.eGender.male)
                {
                    isMale = true;
                }
                else
                {
                    isMale = false;
                }
            }
            else
            {
                isMale = false;
            }

            return isMale;
        }

        private void listBoxFemaleSingleFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxFemaleEvents.Items.Clear();
            displayImageOfFemaleFriend();
            displayFriendsEvent(listBoxFemaleSingleFriends, listBoxFemaleEvents);
        }

        private void displayImageOfMaleFriend()
        {
            if (listBoxMaleSingleFriends.SelectedItems.Count == 1)
            {
                User selectedFriend = listBoxMaleSingleFriends.SelectedItem as User;
                pictureBoxSingleMale.ImageLocation = m_Manger.GetUserProfileImage(selectedFriend);
            }
        }

        private void displayFriendsEvent(ListBox i_FriendsList, ListBox i_EventsList)
        {
            i_FriendsList.DisplayMember = "Name";
            if (i_FriendsList.SelectedItems.Count == 1)
            {
                FacebookObjectCollection<Event> userEvents = m_Manger.FetchUserEvents(i_FriendsList.SelectedItem as User);
                foreach (Event friendEvent in userEvents)
                {
                    i_EventsList.Items.Add(friendEvent);
                }
            }
        }

        private void listBoxMaleSingleFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxMaleEvents.Items.Clear();
            displayImageOfMaleFriend();
            displayFriendsEvent(listBoxMaleSingleFriends, listBoxMaleEvents);
        }

        private void displayImageOfFemaleFriend()
        {
            if (listBoxFemaleSingleFriends.SelectedItems.Count == 1)
            {
                User selectedFriend = listBoxFemaleSingleFriends.SelectedItem as User;
                pictureBoxSingleFemales.ImageLocation = m_Manger.GetUserProfileImage(selectedFriend);
            }
        }

        private void homeToolStripMenuItemHome_Click(object sender, EventArgs e)
        {
            checkedListBoxEvents.Items.Clear();
            richTextBoxMailSubject.Clear();
            richTextBoxMailContent.Clear();
            switchPanels(this.panelSendMailToFriends, this.panelLandingPage);
        }

        private void pictureBoxInviteFriends_Click(object sender, EventArgs e)
        {
            changeToInviteFriendsPage();
        }

        private void labelInviteFriends_Click(object sender, EventArgs e)
        {
            changeToInviteFriendsPage();
        }

        private void changeToInviteFriendsPage()
        {
            fetchEventsList(m_Manger.FacebookUser, checkedListBoxEvents);
            buttonSend.Enabled = false;
            clearAllCheckedInCheckboxList();
            switchPanels(this.panelLandingPage, this.panelSendMailToFriends);
        }

        private void clearAllCheckedInCheckboxList()
        {
            checkedListBoxEvents.ClearSelected();
        }

        private void checkedListBoxFriends_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            toggleSendBtnStatus();
        }

        private void richTextBoxMailSubject_TextChanged(object sender, EventArgs e)
        {
            toggleSendBtnStatus();
        }

        private void toggleSendBtnStatus()
        {
            if (checkedListBoxEvents.SelectedItems.Count == 0)
            {
                buttonSend.Enabled = false;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(richTextBoxMailSubject.Text) || string.IsNullOrWhiteSpace(richTextBoxMailContent.Text))
                {
                    buttonSend.Enabled = false;
                }
                else
                {
                    buttonSend.Enabled = true;
                }
            }
        }

        private void richTextBoxMailContent_TextChanged(object sender, EventArgs e)
        {
            toggleSendBtnStatus();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            m_ReminderBuilder = new FacebookReminderBuilder();
            List<object> selectedEvent = new List<object>();
            foreach (Event item in checkedListBoxEvents.CheckedItems)
            {
                selectedEvent.Add(item);
            }

            m_ReminderManufacturer.Construct(m_ReminderBuilder, richTextBoxMailSubject.Text, richTextBoxMailContent.Text, selectedEvent, m_Manger.FacebookUser.Email);
            m_ReminderBuilder.RemiderNote.sendUserEventsReminder();
        }

        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            switchPanels(this.panelMeProfile, this.panelAboutMe);
            setAboutMePanel();
        }

        private void toolStripMenuItemHomeAbout_Click(object sender, EventArgs e)
        {
            switchPanels(this.panelAboutMe, this.panelLandingPage);
        }

        private void setAboutMePanel()
        {
            userBindingSource.DataSource = m_Manger.FacebookUser;
        }

        private void labelSendMessege_Click(object sender, EventArgs e)
        {
            doBeforeChangingToComparePanel();
        }

        private void pictureBoxSendMessege_Click(object sender, EventArgs e)
        {
            doBeforeChangingToComparePanel();
        }

        private void doBeforeChangingToComparePanel()
        {
            listBoxFriendToBeCompared.Items.Clear();
            listBoxFriendToBeCompared.Enabled = false;
            listBoxCompareFriendsResults.Enabled = false;
            switchPanels(this.panelLandingPage, this.panelCompareFriends);
        }

        private void radioButtonCompareByAge_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCompareByAge.Checked == true)
            {
                reFetchFriendsWithDataForSelectedCriterion(eCriterion.Age);
                listBoxFriendToBeCompared.Enabled = true;
                m_MatchFriendsByParameter.MatcherTool = new MatchByAge();
            }
        }

        private void radioButtonCompareByCity_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCompareByCity.Checked == true)
            {
                reFetchFriendsWithDataForSelectedCriterion(eCriterion.City);
                listBoxFriendToBeCompared.Enabled = true;
                m_MatchFriendsByParameter.MatcherTool = new MatchByCity();
            }
        }

        private void radioButtonCompareByCountry_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCompareByCountry.Checked == true)
            {
                reFetchFriendsWithDataForSelectedCriterion(eCriterion.Country);
                listBoxFriendToBeCompared.Enabled = true;
                m_MatchFriendsByParameter.MatcherTool = new MatchByCountry();
            }
        }

        private void radioButtonCompareByGender_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCompareByGender.Checked == true)
            {
                reFetchFriendsWithDataForSelectedCriterion(eCriterion.Gender);
                listBoxFriendToBeCompared.Enabled = true;
                m_MatchFriendsByParameter.MatcherTool = new MatchByGender();
            }
        }

        private void reFetchFriendsWithDataForSelectedCriterion(eCriterion i_Criterion)
        {
            User fullFriend;
            FacebookObjectCollection<User> friends = m_Manger.FetchMyfriends();
            listBoxFriendToBeCompared.Items.Clear();
            listBoxCompareFriendsResults.Items.Clear();
            foreach (User friend in friends)
            {
                fullFriend = FacebookService.GetObject<User>(friend.Id, User.s_FieldsToLoadFull[DynamicWrapper.eLoadOptions.Full]);
                if (isCriterionDataAvilable(i_Criterion, fullFriend) == true)
                {
                    listBoxFriendToBeCompared.Items.Add(fullFriend);
                }
            }
        }

        private bool isCriterionDataAvilable(eCriterion i_Criterion, User i_User)
        {
            bool returnResult;
            switch(i_Criterion)
            {
                case eCriterion.Age:
                    if(string.IsNullOrEmpty(i_User.Birthday) || (i_User.Birthday.Split('/').Length != 3))
                    {
                        returnResult = false;
                    }
                    else
                    {
                        returnResult = true;
                    }

                    break;
              
                case eCriterion.City:
                    if ((i_User.Location == null) || (i_User.Location.Name.Split(',').Length != 2))
                    {
                        returnResult = false;
                    }
                    else
                    {
                        returnResult = true;
                    }

                    break;

                case eCriterion.Country:
                    if ((i_User.Location == null) || (i_User.Location.Name.Split(',').Length == 0))
                    {
                        returnResult = false;
                    }
                    else
                    {
                        returnResult = true;
                    }

                    break;

                case eCriterion.Gender:
                    if(i_User.Gender == null)
                    { 
                        returnResult = false;
                    }
                    else
                    {
                        returnResult = true;
                    }

                    break;

                default:
                    returnResult = false;
                    break;
            }

            return returnResult;
        }

        private void listBoxFriendToBeCompared_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxCompareFriendsResults.Items.Clear();
            searchMatchFriends();
        }

        private void searchMatchFriends()
        {
          for(int i = 0; i < listBoxFriendToBeCompared.Items.Count; i++)
            {
                User user = listBoxFriendToBeCompared.Items[i] as User;
                if (listBoxFriendToBeCompared.SelectedItem as User == user)
                {
                    continue;
                }

               if (m_MatchFriendsByParameter.MatcherTool.IsMatch(listBoxFriendToBeCompared.SelectedItem as User, user) == true)
               {
                   listBoxCompareFriendsResults.Items.Add(user);
               }
            }
        }

        private void homeToolStripMenuCompareFriends_Click(object sender, EventArgs e)
        {
            listBoxCompareFriendsResults.Items.Clear();
            listBoxFriendToBeCompared.Items.Clear();
            radioButtonCompareByGender.Checked = false;
            radioButtonCompareByCountry.Checked = false;
            radioButtonCompareByCity.Checked = false;
            radioButtonCompareByAge.Checked = false;
            switchPanels(this.panelCompareFriends, this.panelLandingPage);
        }
    }
}
