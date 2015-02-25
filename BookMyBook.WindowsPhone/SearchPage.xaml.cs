using BookMyBook.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BookMyBook
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public SearchPage()
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
            read(); 
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        List<Items> list = new List<Items>();
        
        public async void read()
        {
            progress.IsActive = true;
            if (list.Count < 2)
            {
                string h1 = "http://www.infibeam.com/Books/search?q=" + MainPage.srchTxt;
                try
                {
                    HttpContent hc = new StringContent("{ \"input\": { \"webpage/url\": \"" + h1 + "\"}}");
                    var client = new HttpClient();
                    var response = await client.PostAsync(new Uri("https://api.import.io/store/data/152b6c81-0417-42fe-a150-55ff8ff6e567/_query?_user=9af12242-5eff-4b79-990f-4d42fd017332&_apikey=t8y608aARzHmiuYn5ex4UmDCasxkmkGVT1OgOBvGo5WVzglDPNjqy1K0yEXpvUM1mX%2Bbr2K7lXoEHJp5h%2FpVIQ%3D%3D"), hc);
                    var jstring = await response.Content.ReadAsStringAsync();
                    JsonValue ob = JsonValue.Parse(jstring);
                    JsonObject ob1 = ob.GetObject();
                    JsonArray ob2 = ob1.GetNamedArray("results");
                    uint i = 0;
                    while (ob2.GetObjectAt(i) != null)
                    {
                        Items sr = new Items();
                        try
                        {
                            JsonObject ob3 = ob2.GetObjectAt(i);
                            sr.Title = (ob3.GetNamedString("title/_text"));
                            sr.Author = (ob3.GetNamedString("author/_text"));
                            sr.Price = ("MRP: " + ob3.GetNamedNumber("mrp"));
                            sr.sPrice = ("" + ob3.GetNamedArray("iprice").GetNumberAt(0));
                            sr.Link = (ob3.GetNamedString("title"));
                            sr.Publisher = ("");
                            sr.ImageUrl = ob3.GetNamedString("image");
                            sr.Image = new BitmapImage(new Uri((sr.ImageUrl), UriKind.Absolute));
                            string ttl = sr.Link;
                            try
                            {
                                int k = ttl.IndexOf(".html");
                                ttl = ttl.Substring(k - 13, 13);
                                long kd = Convert.ToInt64(ttl);
                                if (checkisbn(kd, ttl.Length))
                                    sr.Isbn = (ttl);

                            }
                            catch (Exception) { }
                        }
                        catch (Exception) { }
                        if (!sr.Isbn.Equals(""))
                            list.Add(sr);
                    }
                    itemListView.ItemsSource = list;
                    itemListView.Visibility = Visibility.Visible;
                }
                catch (Exception)
                {
                    progress.IsActive = false;
                }
            }
            else { itemListView.ItemsSource = list; itemListView.Visibility = Visibility.Visible; }
            progress.IsActive = false;

        }
        private bool checkisbn(long n, int l)
        {
            long m = n;
            m = m / 10;
            int sum = 0;
            if (l == 10)
            {
                for (int i = 1; i < 10; i++)
                {
                    sum = sum + (int)((10 - i) * (m % 10));
                    m /= 10;
                }
                sum = sum % 11;
            }
            else if (l == 13)
            {
                for (int i = 1; i < 13; i++)
                {
                    if (i % 2 == 0)
                        sum = sum + (int)(1 * (m % 10));
                    else sum = sum + (int)(3 * (m % 10));
                    m /= 10;
                }
                sum = 10 - sum % 10;
            }
            if (sum == (n % 10))
                return true;
            else return false;
        }

        private void refresh_all(object sender, RoutedEventArgs e)
        {
            read();
        }
        public static Items ob = new Items();
        private void itemListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ob = (Items)e.ClickedItem; 
            if (!progress.IsActive)
            {
                MainPage.isbn = ob.Isbn;
                if (this.Frame != null)
                    {
                        this.Frame.Navigate(typeof(Stores));
                    }
            }
        }

        private void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ob = (Items)itemListView.SelectedItem; 
            if (!progress.IsActive)
            {
                MainPage.isbn = ob.Isbn;
                if (this.Frame != null)
                    {
                        this.Frame.Navigate(typeof(Stores));
                    }
            }
        }
    }
}