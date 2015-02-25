using BookMyBook.Common;
using NotificationsExtensions.TileContent;
using System;
using System.Net.Http;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Json;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace BookMyBook
{
   
    public sealed partial class SplitPage1 : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

       public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public SplitPage1()
        {
            this.InitializeComponent();

            // Setup the navigation helper
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            DataTransferManager.GetForCurrentView().DataRequested += dataTransferManager_DataRequested;

            // Start listening for Window size changes 
            // to change from showing two panes to showing a single pane
            Window.Current.SizeChanged += Window_SizeChanged;
            this.InvalidateVisualState();
        }

        void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.UsingLogicalPageNavigation())
            {
                this.navigationHelper.GoBackCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Assign a bindable group to Me.DefaultViewModel("Group")
            // TODO: Assign a collection of bindable items to Me.DefaultViewModel("Items")

            if (e.PageState == null)
            {
                // When this is a new page, select the first item automatically unless logical page
                // navigation is being used (see the logical page navigation #region below.)
                if (!this.UsingLogicalPageNavigation() && this.itemsViewSource.View != null)
                {
                    this.itemsViewSource.View.MoveCurrentToFirst();
                }
            }
            else
            {
                // Restore the previously saved state associated with this page
                if (e.PageState.ContainsKey("SelectedItem") && this.itemsViewSource.View != null)
                {
                    // TODO: Invoke Me.itemsViewSource.View.MoveCurrentTo() with the selected
                    //       item as specified by the value of pageState("SelectedItem")

                }
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (this.itemsViewSource.View != null)
            {
                // TODO: Derive a serializable navigation parameter and assign it to
                //       pageState("SelectedItem")

            }
        }

        #region Logical page navigation

        // The split page isdesigned so that when the Window does have enough space to show
        // both the list and the dteails, only one pane will be shown at at time.
        //
        // This is all implemented with a single physical page that can represent two logical
        // pages.  The code below achieves this goal without making the user aware of the
        // distinction.

        private const int MinimumWidthForSupportingTwoPanes = 768;

        /// <summary>
        /// Invoked to determine whether the page should act as one logical page or two.
        /// </summary>
        /// <returns>True if the window should show act as one logical page, false
        /// otherwise.</returns>
        private bool UsingLogicalPageNavigation()
        {
            return Window.Current.Bounds.Width < MinimumWidthForSupportingTwoPanes;
        }

        /// <summary>
        /// Invoked with the Window changes size
        /// </summary>
        /// <param name="sender">The current Window</param>
        /// <param name="e">Event data that describes the new size of the Window</param>
        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState();
        }

        /// <summary>
        /// Invoked when an item within the list is selected.
        /// </summary>
        /// <param name="sender">The GridView displaying the selected item.</param>
        /// <param name="e">Event data that describes how the selection was changed.</param>
        
        private void InvalidateVisualState()
        {
            this.navigationHelper.GoBackCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Invoked to determine the name of the visual state that corresponds to an application
        /// view state.
        /// </summary>
        /// <returns>The name of the desired visual state.  This is the same as the name of the
        /// view state except when there is a selected item in portrait and snapped views where
        /// this additional logical page is represented by adding a suffix of _Detail.</returns>
        

        #endregion

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Flipkart(); Amazon(); Infibeam(); Rediff(); Crossword(); uRead(); Landmark(); //duRead();
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        public static string title = "", ImageUrl = "", summary = "", details = "",
           h1 = "http://www.flipkart.com/search?q=" + MainPage.isbn,
           h2 = "http://www.amazon.in/s/ref=nb_sb_noss?url=search-alias%3Dstripbooks&field-keywords=" + MainPage.isbn,
           h3 = "http://www.infibeam.com/search.jsp?storeName=Books&query=" + MainPage.isbn,
           h4 = "http://books.rediff.com/book/" + MainPage.isbn,
           h5 = "http://www.crossword.in/home/search?q=" + MainPage.isbn,
           h6 = "http://www.uread.com/search-books/" + MainPage.isbn,
           h7 = "http://www.landmarkonthenet.com/books/" + MainPage.isbn;
        private static string hs1 = "https://api.import.io/store/data/f696659f-503e-40c4-97ae-5d38f328fb18/_query?_user=9af12242-5eff-4b79-990f-4d42fd017332&_apikey=t8y608aARzHmiuYn5ex4UmDCasxkmkGVT1OgOBvGo5WVzglDPNjqy1K0yEXpvUM1mX%2Bbr2K7lXoEHJp5h%2FpVIQ%3D%3D",
            hs2 = "https://api.import.io/store/data/860b95af-002d-44b1-b9f2-41b550bd31c5/_query?_user=9af12242-5eff-4b79-990f-4d42fd017332&_apikey=t8y608aARzHmiuYn5ex4UmDCasxkmkGVT1OgOBvGo5WVzglDPNjqy1K0yEXpvUM1mX%2Bbr2K7lXoEHJp5h%2FpVIQ%3D%3D",
            hs3 = "https://api.import.io/store/data/a5a94c0d-f397-4352-af5e-725dc48c2e75/_query?_user=9af12242-5eff-4b79-990f-4d42fd017332&_apikey=t8y608aARzHmiuYn5ex4UmDCasxkmkGVT1OgOBvGo5WVzglDPNjqy1K0yEXpvUM1mX%2Bbr2K7lXoEHJp5h%2FpVIQ%3D%3D",
            hs4 = "https://api.import.io/store/data/7be69692-cb88-478d-bdd1-6469a7344302/_query?_user=9af12242-5eff-4b79-990f-4d42fd017332&_apikey=t8y608aARzHmiuYn5ex4UmDCasxkmkGVT1OgOBvGo5WVzglDPNjqy1K0yEXpvUM1mX%2Bbr2K7lXoEHJp5h%2FpVIQ%3D%3D",
            hs5 = "https://api.import.io/store/data/64f906f0-2cf4-4f2f-b97c-745a7fbe0567/_query?_user=9af12242-5eff-4b79-990f-4d42fd017332&_apikey=t8y608aARzHmiuYn5ex4UmDCasxkmkGVT1OgOBvGo5WVzglDPNjqy1K0yEXpvUM1mX%2Bbr2K7lXoEHJp5h%2FpVIQ%3D%3D",
            hs6 = "https://api.import.io/store/data/698c07bf-7190-4583-bbe6-1914dab15ac6/_query?_user=9af12242-5eff-4b79-990f-4d42fd017332&_apikey=t8y608aARzHmiuYn5ex4UmDCasxkmkGVT1OgOBvGo5WVzglDPNjqy1K0yEXpvUM1mX%2Bbr2K7lXoEHJp5h%2FpVIQ%3D%3D",
            hs7 = "https://api.import.io/store/data/d9eded32-4f19-4018-acfd-4823ec8b7642/_query?_user=9af12242-5eff-4b79-990f-4d42fd017332&_apikey=t8y608aARzHmiuYn5ex4UmDCasxkmkGVT1OgOBvGo5WVzglDPNjqy1K0yEXpvUM1mX%2Bbr2K7lXoEHJp5h%2FpVIQ%3D%3D",
            hs8 = "https://api.import.io/store/data/585cb4d0-b06f-480c-93de-abe091fc6e53/_query?_user=9af12242-5eff-4b79-990f-4d42fd017332&_apikey=t8y608aARzHmiuYn5ex4UmDCasxkmkGVT1OgOBvGo5WVzglDPNjqy1K0yEXpvUM1mX%2Bbr2K7lXoEHJp5h%2FpVIQ%3D%3D";
        public static bool imageSet = false, titleSet = false;
        public static int done = 0;
        public static int[] pr = new int[8];
        public async void Flipkart()
        {
            try
            {
                HttpContent hc = new StringContent("{ \"input\": { \"webpage/url\": \"" + h1 + "\"}}");
                var client = new HttpClient();
                var response = await client.PostAsync(new Uri(hs1), hc);
                var jstring = await response.Content.ReadAsStringAsync();
                JsonValue ob = JsonValue.Parse(jstring);
                JsonObject ob1 = ob.GetObject();
                JsonObject ob2 = ob1.GetNamedArray("results").GetObjectAt(0);
                h1 = ob1.GetNamedString("pageUrl");
                try
                {
                    pr[1] = (int)ob2.GetNamedNumber("price");
                    ls1.Text = "" + pr[1];
                    if (pr[1] != 0) ab1.Visibility = Visibility.Collapsed;
                }
                catch (Exception) { }
                if (!titleSet)
                {
                    try
                    {
                        title = ob2.GetNamedString("title"); 
                        Title.Text = "Title: " + title; titleSet = true;
                    }
                    catch (Exception) { titleSet = false; }
                }
            }
            catch (Exception) {  }
            done++;if (done >= 8) Progress.Visibility = Visibility.Collapsed;
        }
        public async void Amazon()
        {
            try
            {
                HttpContent hc = new StringContent("{ \"input\": { \"webpage/url\": \"" + h2 + "\"}}");
                var client = new HttpClient();
                var response = await client.PostAsync(new Uri(hs2), hc);
                var jstring = await response.Content.ReadAsStringAsync();
                JsonValue ob = JsonValue.Parse(jstring);
                JsonObject ob1 = ob.GetObject();
                JsonObject ob2 = ob1.GetNamedArray("results").GetObjectAt(0);
                h2 = ob1.GetNamedString("pageUrl");
                try
                {
                    pr[2] = (int)ob2.GetNamedNumber("price");
                    ls2.Text = "" + pr[2];
                    if (pr[2] != 0) ab2.Visibility = Visibility.Collapsed;
                }
                catch (Exception) { }
                if (!titleSet)
                {
                    try
                    {
                        title = ob2.GetNamedString("title");
                        Title.Text = "Title: " + title;
                        titleSet = true;
                    }
                    catch (Exception) { titleSet = false; }
                }
                if (!imageSet)
                {
                    try
                    {
                        ImageUrl = ob2.GetNamedString("image");
                        Image.Source = new BitmapImage(new Uri(ImageUrl, UriKind.Absolute));
                        imageSet = true;
                    }
                    catch (Exception) { imageSet = false; }
                }
            }
            catch (Exception) { }
            done++;if (done >= 8) Progress.Visibility = Visibility.Collapsed;
        }
        public async void Infibeam()
        {
            if (ItemsPage1.ob != null)
            {
                try
                {
                    pr[3] = Convert.ToInt16(ItemsPage1.ob.sPrice);
                    ls3.Text = "" + pr[3];
                    if (pr[3] != 0) ab3.Visibility = Visibility.Collapsed;
                }
                catch (Exception) { }
                if (!titleSet)
                {
                    try
                    {
                        title = ItemsPage1.ob.Title;
                        Title.Text = "Title: " + title;
                        titleSet = true;
                    }
                    catch (Exception) { titleSet = false; }
                }
                if (!imageSet)
                {
                    try
                    {
                        ImageUrl = ItemsPage1.ob.ImageUrl;
                        Image.Source = ItemsPage1.ob.Image;
                        imageSet = true;
                    }
                    catch (Exception) { imageSet = false; }
                }
            }
            else
            {
                try
                {
                    HttpContent hc = new StringContent("{ \"input\": { \"webpage/url\": \"" + h3 + "\"}}");
                    var client = new HttpClient();
                    var response = await client.PostAsync(new Uri(hs3), hc);
                    var jstring = await response.Content.ReadAsStringAsync();
                    JsonValue ob = JsonValue.Parse(jstring);
                    JsonObject ob1 = ob.GetObject();
                    JsonObject ob2 = ob1.GetNamedArray("results").GetObjectAt(0);
                    h3 = ob1.GetNamedString("pageUrl");
                    try
                    {
                        pr[3] = Convert.ToInt16(ob2.GetNamedArray("price").GetStringAt(0).Substring(4));
                        ls3.Text = "" + pr[3];
                        if (pr[3] != 0) ab3.Visibility = Visibility.Collapsed;
                    }
                    catch (Exception) { }
                    if (!titleSet)
                    {
                        try
                        {
                            title = ob2.GetNamedString("title");
                            Title.Text = "Title: " + title;
                            titleSet = true;
                        }
                        catch (Exception) { titleSet = false; }
                    }
                    if (!imageSet)
                    {
                        try
                        {
                            ImageUrl = ob2.GetNamedString("image");
                            Image.Source = new BitmapImage(new Uri(ImageUrl, UriKind.Absolute));
                            imageSet = true;
                        }
                        catch (Exception) { imageSet = false; }
                    }
                }
                catch (Exception) { }
            }
            done++;if (done >= 8) Progress.Visibility = Visibility.Collapsed;
        }
        public async void Rediff()
        {
            try
            {
                HttpContent hc = new StringContent("{ \"input\": { \"webpage/url\": \"" + h4 + "\"}}");
                var client = new HttpClient();
                var response = await client.PostAsync(new Uri(hs4), hc);
                var jstring = await response.Content.ReadAsStringAsync();
                JsonValue ob = JsonValue.Parse(jstring);
                JsonObject ob1 = ob.GetObject();
                JsonObject ob2 = ob1.GetNamedArray("results").GetObjectAt(0);
                h4 = ob1.GetNamedString("pageUrl");
                try
                {
                    pr[4] = (int)ob2.GetNamedNumber("price");
                    ls4.Text = "" + pr[4];
                    if (pr[4] != 0) ab4.Visibility = Visibility.Collapsed;
                }
                catch (Exception) { }
                if (!titleSet)
                {
                    try
                    {
                        title = ob2.GetNamedString("title");
                        Title.Text = "Title: " + title;
                        titleSet = true;
                    }
                    catch (Exception) { titleSet = false; }
                }
                if (!imageSet)
                {
                    try
                    {
                        ImageUrl = ob2.GetNamedString("image");
                        Image.Source = new BitmapImage(new Uri(ImageUrl, UriKind.Absolute));
                        imageSet = true;
                    }
                    catch (Exception) { imageSet = false; }
                }
            }
            catch (Exception) { }
            done++;if (done >= 8) Progress.Visibility = Visibility.Collapsed;
        }
        public async void Crossword()
        {
            try
            {
                HttpContent hc = new StringContent("{ \"input\": { \"webpage/url\": \"" + h5 + "\"}}");
                var client = new HttpClient();
                var response = await client.PostAsync(new Uri(hs5), hc);
                var jstring = await response.Content.ReadAsStringAsync();
                JsonValue ob = JsonValue.Parse(jstring);
                JsonObject ob1 = ob.GetObject();
                JsonObject ob2 = ob1.GetNamedArray("results").GetObjectAt(0);
                h5 = ob1.GetNamedString("pageUrl");
                try
                {
                    pr[5] = (int)ob2.GetNamedNumber("price");
                    ls5.Text = "" + pr[5];
                    if (pr[5] != 0) ab5.Visibility = Visibility.Collapsed;
                }
                catch (Exception) { }
                if (!titleSet)
                {
                    try
                    {
                        title = ob2.GetNamedString("title");
                        Title.Text = "Title: " + title;
                        titleSet = true;
                    }
                    catch (Exception) { titleSet = false; }
                }
                if (!imageSet)
                {
                    try
                    {
                        ImageUrl = ob2.GetNamedString("image");
                        Image.Source = new BitmapImage(new Uri(ImageUrl, UriKind.Absolute));
                        imageSet = true;
                    }
                    catch (Exception) { imageSet = false; }
                }
            }
            catch (Exception) { }
            done++;if (done >= 8) Progress.Visibility = Visibility.Collapsed;
        }
        public async void uRead()
        {
            try
            {
                HttpContent hc = new StringContent("{ \"input\": { \"webpage/url\": \"" + h6 + "\"}}");
                var client = new HttpClient();
                var response = await client.PostAsync(new Uri(hs6), hc);
                var jstring = await response.Content.ReadAsStringAsync();
                JsonValue ob = JsonValue.Parse(jstring);
                JsonObject ob1 = ob.GetObject();
                JsonObject ob2 = ob1.GetNamedArray("results").GetObjectAt(0);
                h6 = ob1.GetNamedString("pageUrl");duRead();
                try
                {
                    pr[6] = Convert.ToInt16(ob2.GetNamedString("price").Substring(1));
                    ls6.Text = "" + pr[6];
                    if (pr[6] != 0) ab6.Visibility = Visibility.Collapsed;
                }
                catch (Exception) { }
                if (!titleSet)
                {
                    try
                    {
                        title = ob2.GetNamedString("title");
                        Title.Text = "Title: " + title;
                        titleSet = true;
                    }
                    catch (Exception) { titleSet = false; }
                }
                if (!imageSet)
                {
                    try
                    {
                        ImageUrl = ob2.GetNamedString("image").Replace("images200", "mainimages");
                        Image.Source = new BitmapImage(new Uri(ImageUrl, UriKind.Absolute));
                        imageSet = true;
                    }
                    catch (Exception) { imageSet = false; }
                }
            }
            catch (Exception) { }
            done++;if (done >= 8) Progress.Visibility = Visibility.Collapsed;
        }
        public async void Landmark()
        {

            try
            {
                HttpContent hc = new StringContent("{ \"input\": { \"webpage/url\": \"" + h7 + "\"}}");
                var client = new HttpClient();
                var response = await client.PostAsync(new Uri(hs7), hc);
                var jstring = await response.Content.ReadAsStringAsync();
                JsonValue ob = JsonValue.Parse(jstring);
                JsonObject ob1 = ob.GetObject();
                JsonObject ob2 = ob1.GetNamedArray("results").GetObjectAt(0);
                h7 = ob1.GetNamedString("pageUrl");
                try
                {
                    pr[7] = Convert.ToInt16(ob2.GetNamedString("price").Substring(3));
                    ls7.Text = "" + pr[7];
                    if (pr[7] != 0) ab7.Visibility = Visibility.Collapsed;
                }
                catch (Exception) { }
                if (!titleSet)
                {
                    try
                    {
                        title = ob2.GetNamedString("title");
                        Title.Text = "Title: " + title;
                        titleSet = true;
                    }
                    catch (Exception e) { titleSet = false; }
                }
                if (!imageSet)
                {
                    try
                    {
                        ImageUrl = ob2.GetNamedString("image");
                        Image.Source = new BitmapImage(new Uri(ImageUrl, UriKind.Absolute));
                        imageSet = true;
                    }
                    catch (Exception e) { imageSet = false; }
                }
            }
            catch (Exception e) { }
            done++;if (done >= 8) Progress.Visibility = Visibility.Collapsed;
        }
        public async void duRead()
        {
            try
            {
                HttpContent hc = new StringContent("{ \"input\": { \"webpage/url\": \"" + h6 + "\"}}");
                var client = new HttpClient();
                var response = await client.PostAsync(new Uri(hs8), hc);
                var jstring = await response.Content.ReadAsStringAsync();
                JsonValue ob = JsonValue.Parse(jstring);
                JsonObject ob1 = ob.GetObject();
                JsonObject ob2 = ob1.GetNamedArray("results").GetObjectAt(0);
                try
                {
                    JsonArray ob3 = ob2.GetNamedArray("summary");
                    uint k = 0;
                    while (ob3.GetStringAt(k).Equals(""))
                    {
                        summary += ob3.GetStringAt(k) + "\n"; k++;
                        summary1.Visibility = Visibility.Collapsed;
                    }
                    Summary.Text = summary;
                }
                catch (Exception e) { Title.Text = e.ToString(); }
                if(summary.Equals("")) summary1.Visibility = Visibility.Visible;
                try
                {
                    JsonArray ob3 = ob2.GetNamedArray("details");
                    uint k = 0;
                    while (ob3.GetStringAt(k).Equals(""))
                    {
                        details += ob3.GetStringAt(k) + " " + ob3.GetStringAt(k + 1) + "\n"; k += 2;
                    }
                    Details.Text = details;
                }
                catch (Exception e) { Title.Text = e.ToString(); }
            }
            catch (Exception e) { Title.Text = e.ToString(); }
            done++;if (done >= 8) Progress.Visibility = Visibility.Collapsed;
        }
        public void highlight()
        {
            TextBlock[] p = { ls1, ls2, ls3, ls4, ls5, ls6, ls7 };
            pr[0] = pr[1];
            for (int i = 1; i < 7; i++)
            {
                if (pr[i] != 0 && pr[0] > pr[i]) pr[0] = pr[i];
            }
            for (int i = 0; i < 7; i++)
            {
                if (pr[i] == 0) p[i].Text = "N/A";
                if (pr[0] != 0 && pr[i + 1] == pr[0]) { p[i].Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.GreenYellow); p[i].FontSize = 27; }
            }
        }
        private void r1_Click(object sender, RoutedEventArgs e) { done--; Progress.Visibility = Visibility.Visible; Flipkart(); }
        private void r2_Click(object sender, RoutedEventArgs e) { done--; Progress.Visibility = Visibility.Visible; Amazon(); }
        private void r3_Click(object sender, RoutedEventArgs e) { done--; Progress.Visibility = Visibility.Visible; Infibeam();  }
        private void r4_Click(object sender, RoutedEventArgs e) { done--; Progress.Visibility = Visibility.Visible; Rediff(); }
        private void r5_Click(object sender, RoutedEventArgs e) { done--; Progress.Visibility = Visibility.Visible; Crossword(); }
        private void r6_Click(object sender, RoutedEventArgs e) { done--; Progress.Visibility = Visibility.Visible; uRead(); }
        private void r7_Click(object sender, RoutedEventArgs e) { done--; Progress.Visibility = Visibility.Visible; Landmark(); }
        private void UpdateTile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            ITileSquare310x310Image tileContent = TileContentFactory.CreateTileSquare310x310Image();
            tileContent.AddImageQuery = true;
            tileContent.Image.Src = ImageUrl;
            tileContent.Image.Alt = "Web Image";
            ITileWide310x150ImageAndText01 wide310x150Content = TileContentFactory.CreateTileWide310x150ImageAndText01();
            wide310x150Content.TextCaptionWrap.Text = "Last Book:ISBN-" + MainPage.isbn;
            wide310x150Content.Image.Src = ImageUrl;
            wide310x150Content.Image.Alt = "Web image";
            ITileSquare150x150Image square150x150Content = TileContentFactory.CreateTileSquare150x150Image();
            square150x150Content.Image.Src = ImageUrl;
            square150x150Content.Image.Alt = "Web image";
            wide310x150Content.Square150x150Content = square150x150Content;
            tileContent.Wide310x150Content = wide310x150Content;
            TileNotification tileNotification = tileContent.CreateNotification();
            string tag = "Image";
            tileNotification.Tag = tag;
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            ITileSquare310x310Text09 square310x310TileContent = TileContentFactory.CreateTileSquare310x310Text09();
            square310x310TileContent.TextHeadingWrap.Text = Title.Text + Environment.NewLine + "ISBN-" + MainPage.isbn;
            ITileWide310x150Text03 wide310x150TileContent = TileContentFactory.CreateTileWide310x150Text03();
            wide310x150TileContent.TextHeadingWrap.Text = Title.Text + Environment.NewLine + "ISBN-" + MainPage.isbn;
            ITileSquare150x150Text04 square150x150TileContent = TileContentFactory.CreateTileSquare150x150Text04();
            square150x150TileContent.TextBodyWrap.Text = Title.Text + Environment.NewLine + "ISBN-" + MainPage.isbn;
            wide310x150TileContent.Square150x150Content = square150x150TileContent;
            square310x310TileContent.Wide310x150Content = wide310x150TileContent;
            tileNotification = square310x310TileContent.CreateNotification();
            tag = "Title";
            tileNotification.Tag = tag;
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }
        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e) { if (this.Frame != null) { itemWebGrid.Visibility = Visibility.Visible; Web.Source = new Uri(SplitPage1.h1); } }
        private void StackPanel_Tapped_1(object sender, TappedRoutedEventArgs e) { if (this.Frame != null) { itemWebGrid.Visibility = Visibility.Visible; Web.Source = new Uri(SplitPage1.h2); } }
        private void StackPanel_Tapped_2(object sender, TappedRoutedEventArgs e) { if (this.Frame != null) { itemWebGrid.Visibility = Visibility.Visible; Web.Source = new Uri(SplitPage1.h3); } }
        private void StackPanel_Tapped_3(object sender, TappedRoutedEventArgs e) { if (this.Frame != null) { itemWebGrid.Visibility = Visibility.Visible; Web.Source = new Uri(SplitPage1.h4); } }
        private void StackPanel_Tapped_4(object sender, TappedRoutedEventArgs e) { if (this.Frame != null) { itemWebGrid.Visibility = Visibility.Visible; Web.Source = new Uri(SplitPage1.h5); } }
        private void StackPanel_Tapped_5(object sender, TappedRoutedEventArgs e) { if (this.Frame != null) { itemWebGrid.Visibility = Visibility.Visible; Web.Source = new Uri(SplitPage1.h6); } }
        private void StackPanel_Tapped_6(object sender, TappedRoutedEventArgs e) { if (this.Frame != null) { itemWebGrid.Visibility = Visibility.Visible; Web.Source = new Uri(SplitPage1.h7); } }
        private void refresh_summary(object sender, RoutedEventArgs e)
        {
            done--; duRead();
        }
        private void refresh_details(object sender, RoutedEventArgs e)
        {
            done--; duRead();
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            itemWebGrid.Visibility = Visibility.Visible;
            switch (ListView1.SelectedIndex)
            {
                case 0: Web.Source = new Uri(SplitPage1.h1); break;
                case 1: Web.Source = new Uri(SplitPage1.h2); break;
                case 2: Web.Source = new Uri(SplitPage1.h3); break;
                case 3: Web.Source = new Uri(SplitPage1.h4); break;
                case 4: Web.Source = new Uri(SplitPage1.h5); break;
                case 5: Web.Source = new Uri(SplitPage1.h6); break;
                case 6: Web.Source = new Uri(SplitPage1.h7); break;
            }
        }

        private void ListView1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (((StackPanel)e.ClickedItem).Equals(sp1)) { ListView1.SelectedIndex = 0; }
            if (((StackPanel)e.ClickedItem).Equals(sp2)) { ListView1.SelectedIndex = 1; }
            if (((StackPanel)e.ClickedItem).Equals(sp3)) { ListView1.SelectedIndex = 2; }
            if (((StackPanel)e.ClickedItem).Equals(sp4)) { ListView1.SelectedIndex = 3; }
            if (((StackPanel)e.ClickedItem).Equals(sp5)) { ListView1.SelectedIndex = 4; }
            if (((StackPanel)e.ClickedItem).Equals(sp6)) { ListView1.SelectedIndex = 5; }
            if (((StackPanel)e.ClickedItem).Equals(sp7)) { ListView1.SelectedIndex = 6; }
            
        }

        private async void Browser_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(Web.Source);
        }
        private void Share_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
            Appbar.IsOpen = true;
        }
        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if (Web.CanGoForward) Web.GoForward();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Web.CanGoBack) Web.GoBack();
        }

        async void dataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            //We are going to use an async API to talk to the webview, so get a deferral for the results
            DataRequestDeferral deferral = args.Request.GetDeferral();
            DataPackage dp = await Web.CaptureSelectedContentToDataPackageAsync();

            if (dp != null && dp.GetView().AvailableFormats.Count > 0)
            {
                // Webview has a selection, so we'll share its data package
                dp.Properties.Title = "This is the selection from the webview control";
                request.Data = dp;
            }
            else
            {
                // No selection, so we'll share the url of the webview
                DataPackage myData = new DataPackage();
                myData.SetWebLink(Web.Source);
                myData.Properties.Title = "This is the URI from the webview control";
                myData.Properties.Description = Web.Source.ToString();
                request.Data = myData;
            }
            deferral.Complete();
        }
        string text1 = "We are piling up the books for you...", text2 = "We are arranging the books for you...", text3 = "Books are finally arranged for you to see...";
        private void Web_NavigationStarting_1(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            Mean.Opacity = 0.9;
            Progress.Visibility = Visibility.Visible;
            ProgressText.Visibility = Visibility.Visible;
            Mean.Visibility = Visibility.Visible;
            ProgressText.Text = text1;
        }

        private void Web_NavigationCompleted_1(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            ProgressText.Text = text3;
            Progress.Visibility = Visibility.Collapsed;
            ProgressText.Visibility = Visibility.Collapsed;
            Mean.Visibility = Visibility.Collapsed;
        }

        private void Web_FrameContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            ProgressText.Text = text2; Mean.Opacity = 0.5;
        }

        private void Web_FrameDOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            ProgressText.Text = text3; Mean.Opacity = 0.3;
            // Progress.Visibility = Visibility.Collapsed;
            //ProgressText.Visibility = Visibility.Collapsed;
            //Mean.Visibility = Visibility.Collapsed;
        }

    }
}
