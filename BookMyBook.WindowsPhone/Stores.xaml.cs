using BookMyBook.Common;
using NotificationsExtensions.TileContent;
using System;
using System.Net.Http;
using Windows.ApplicationModel.Email;
using Windows.Data.Json;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace BookMyBook
{
    public sealed partial class Stores : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public Stores()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }
        
        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
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
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
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
            catch (Exception) { }
            done++; if (done >= 8) Progress.Visibility = Visibility.Collapsed;
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
            done++; if (done >= 8) Progress.Visibility = Visibility.Collapsed;
        }
        public async void Infibeam()
        {
            if (SearchPage.ob != null)
            {
                try
                {
                    pr[3] = Convert.ToInt16(SearchPage.ob.sPrice);
                    ls3.Text = "" + pr[3];
                    if (pr[3] != 0) ab3.Visibility = Visibility.Collapsed;
                }
                catch (Exception) { }
                if (!titleSet)
                {
                    try
                    {
                        title = SearchPage.ob.Title;
                        Title.Text = "Title: " + title;
                        titleSet = true;
                    }
                    catch (Exception) { titleSet = false; }
                }
                if (!imageSet)
                {
                    try
                    {
                        ImageUrl = SearchPage.ob.ImageUrl;
                        Image.Source = SearchPage.ob.Image;
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
            done++; if (done >= 8) Progress.Visibility = Visibility.Collapsed;
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
            done++; if (done >= 8) Progress.Visibility = Visibility.Collapsed;
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
            done++; if (done >= 8) Progress.Visibility = Visibility.Collapsed;
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
                h6 = ob1.GetNamedString("pageUrl"); duRead();
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
            done++; if (done >= 8) Progress.Visibility = Visibility.Collapsed;
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
            done++; if (done >= 8) Progress.Visibility = Visibility.Collapsed;
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
                    }
                    Summary.Text = summary;
                }
                catch (Exception e) { Title.Text = e.ToString(); }
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
            done++; if (done >= 8) Progress.Visibility = Visibility.Collapsed;
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
        private void r3_Click(object sender, RoutedEventArgs e) { done--; Progress.Visibility = Visibility.Visible; Infibeam(); }
        private void r4_Click(object sender, RoutedEventArgs e) { done--; Progress.Visibility = Visibility.Visible; Rediff(); }
        private void r5_Click(object sender, RoutedEventArgs e) { done--; Progress.Visibility = Visibility.Visible; Crossword(); }
        private void r6_Click(object sender, RoutedEventArgs e) { done--; Progress.Visibility = Visibility.Visible; uRead(); }
        private void r7_Click(object sender, RoutedEventArgs e) { done--; Progress.Visibility = Visibility.Visible; Landmark(); }
        private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ListView1.SelectedIndex)
            {
                case 0: await Launcher.LaunchUriAsync(new Uri(h1)); break;
                case 1: await Launcher.LaunchUriAsync(new Uri(h2)); break;
                case 2: await Launcher.LaunchUriAsync(new Uri(h3)); break;
                case 3: await Launcher.LaunchUriAsync(new Uri(h4)); break;
                case 4: await Launcher.LaunchUriAsync(new Uri(h5)); break;
                case 5: await Launcher.LaunchUriAsync(new Uri(h6)); break;
                case 6: await Launcher.LaunchUriAsync(new Uri(h7)); break;
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
        
        private void refresh_all(object sender, RoutedEventArgs e)
        {
            done = 0; 
            Flipkart();  Amazon(); Infibeam();Rediff(); uRead(); Landmark(); Crossword();
            Progress.Visibility = Visibility.Visible;
        }
        private void about_event(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(About));
            }
        }
        private async void Feedback_event(object sender, RoutedEventArgs e)
        {

            EmailRecipient sendTo = new EmailRecipient()
            {
                Address = "srujanjha.jha@gmail.com"
            };

            //generate mail object
            EmailMessage mail = new EmailMessage();
            mail.Subject = "Feedback for BookMyBook";
            mail.Body = "Write your feedback here...";

            //add recipients to the mail object
            mail.To.Add(sendTo);
            //mail.Bcc.Add(sendTo);
            //mail.CC.Add(sendTo);

            //open the share contract with Mail only:
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }
        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BigTitle.Text = Title.Text;
            BigImage.Source = Image.Source;
            ImageViewer.IsOpen = true;
        }
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ImageViewer.IsOpen = false;
        }
        private void Title_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Results.SelectedIndex = 1;
        }
        private void UpdateTile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            ITileSquare310x310Image tileContent = TileContentFactory.CreateTileSquare310x310Image();
            tileContent.AddImageQuery = true;
            tileContent.Image.Src = ImageUrl;
            tileContent.Image.Alt = "Web Image";
            ITileWide310x150ImageAndText01 wide310x150Content = TileContentFactory.CreateTileWide310x150ImageAndText01();
            wide310x150Content.TextCaptionWrap.Text = "Last Book:ISBN-"+MainPage.isbn;
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
            square310x310TileContent.TextHeadingWrap.Text =  Title.Text + Environment.NewLine + "ISBN-" + MainPage.isbn;
            ITileWide310x150Text03 wide310x150TileContent = TileContentFactory.CreateTileWide310x150Text03();
            wide310x150TileContent.TextHeadingWrap.Text =  Title.Text + Environment.NewLine + "ISBN-" + MainPage.isbn;
            ITileSquare150x150Text04 square150x150TileContent = TileContentFactory.CreateTileSquare150x150Text04();
            square150x150TileContent.TextBodyWrap.Text =  Title.Text + Environment.NewLine + "ISBN-" + MainPage.isbn;
            wide310x150TileContent.Square150x150Content = square150x150TileContent;
            square310x310TileContent.Wide310x150Content = wide310x150TileContent;
            tileNotification = square310x310TileContent.CreateNotification();
            tag = "Title";
            tileNotification.Tag = tag;
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        private void refresh_summary(object sender, RoutedEventArgs e)
        {
            done--; duRead();
        }

        private void refresh_details(object sender, RoutedEventArgs e)
        {
            done--;duRead();
        }

        private void refresh_image(object sender, RoutedEventArgs e)
        {
            imageSet = false; titleSet = false;
            try { Flipkart(); Amazon(); Infibeam(); Rediff(); uRead(); Landmark(); Crossword(); }
            catch (Exception ) { }
        }
        private void read()
        {
            if (MainPage.save <= 1) {  Tut.IsOpen = true; }
                        else Tut.IsOpen = false;
        }
        private void Tutorial_event(object sender, RoutedEventArgs e)
        {
            if (enable.Label.Equals("enable tutorial"))
            {
                enable.Label = "disable tutorial";
                Tut.IsOpen = true;
            }
            else
            {
                enable.Label = "enable tutorial";
                Tut.IsOpen = false;
            }
        }
        private void Tut_Closed(object sender, object e)
        {
            if (Tut.IsOpen)
            {
                enable.Label = "disable tutorial";
            }
            else
            {
                enable.Label = "enable tutorial";
            }
        }
        private void Tut_Close(object sender, RoutedEventArgs e)
        {
            enable.Label = "enable tutorial"; Tut.IsOpen = false;
        }
        private void Tutorial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Tutorial.SelectedIndex >= 1) BottomAppBar.IsOpen = true;
            }
            catch (Exception) { }
        }
        
    }
}
