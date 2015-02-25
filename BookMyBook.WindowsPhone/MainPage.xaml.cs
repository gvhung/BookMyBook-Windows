using System;
using System.IO;
using Windows.ApplicationModel.Email;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace BookMyBook
{
    public sealed partial class MainPage : Page
    {
        public static string isbn = "",srchTxt="";
        public MainPage()
        {
            this.InitializeComponent();
            read();
            run();
            if (Tut.IsOpen)
            {
                enable.Label = "disable tutorial";
            }
            else
            {
                enable.Label = "enable tutorial";
            }
        }
        private async void delete()
        {
            try
            {
                StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;
                StorageFile storageFile = await roamingFolder.CreateFileAsync("Tutorial.txt", CreationCollisionOption.OpenIfExists);
                await storageFile.DeleteAsync();
            }
            catch (Exception) {}
        }
        private void run()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
                timer.Tick += (sender, e) =>
                {
                    int k = FlipView.SelectedIndex;
                    if (k !=7) k++;
                    else k = 0;
                    FlipView.SelectedIndex = k;
                };
            timer.Start();
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
        
        private bool check()
        {
            if (!(srchTxt.Length == 10 || srchTxt.Length == 13)) { return false; }
            try
            {
                long d = Convert.ToInt64(srchTxt);
                if (!checkisbn(d, srchTxt.Length))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            isbn = srchTxt;
            return true;
        }
        public static int save=10;
        private async void read()
        {
            try
            {
                StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;
                StorageFile storageFile = await roamingFolder.CreateFileAsync("Tutorial.txt", CreationCollisionOption.OpenIfExists);
                Stream readStream = await storageFile.OpenStreamForReadAsync();
                using (StreamReader reader = new StreamReader(readStream))
                {
                    if (reader.EndOfStream) { write(10 + ""); Tut.IsOpen = true; }
                    while (!reader.EndOfStream)
                   {
                        String txt = reader.ReadLine();
                        save = Convert.ToInt16(txt); write(save.ToString()); 
                    }
                }
                //await storageFile.DeleteAsync();
            }
            catch (Exception) {   }//Message.Show("Read", e.ToString()); }
        }
        private async void write(string s)
        {
            try
            {
                StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;
                StorageFile storageFile = await roamingFolder.CreateFileAsync("Tutorial.txt", CreationCollisionOption.OpenIfExists);
                Stream writeStream = await storageFile.OpenStreamForWriteAsync();
                using (StreamWriter writer = new StreamWriter(writeStream))
                {
                    writer.WriteLine(s);
                }
            }
            catch (Exception) {  }
        }
        /*private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender,AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            //SearchBox.IsSuggestionListOpen = false;
            String text = args.SelectedItem.ToString();
            s = text;
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(Stores));
                }
        }
        private void SearchBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
           // SearchBox.ItemsSource = SearchBox_list1; //SearchBox.IsSuggestionListOpen = true;
        }
         * private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            SearchBox_list1.Clear();
            for (int i = 0; i < SearchBox_list.Count; i++)
            {
                if (SearchBox_list.ElementAt(i).Contains(SearchBox.Text))
                { SearchBox_list1.Add(SearchBox_list.ElementAt(i)); }
            }
          //  SearchBox.ItemsSource = SearchBox_list1;
          //  SearchBox.IsSuggestionListOpen = true;
        }
        */
        private async void Feedback_event(object sender, RoutedEventArgs e)
        {
            EmailRecipient sendTo = new EmailRecipient()
            {
                Address = "srujanjha.jha@gmail.com"
            };
            EmailMessage mail = new EmailMessage();
            mail.Subject = "Feedback for BookMyBook";
            mail.Body = "Write your feedback here...";
            mail.To.Add(sendTo);
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }
        private void about_event(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(About));
            }
        }
        private void Search_Click(object sender, RoutedEventArgs e) { search(); }
        private void Search_Tapped(object sender, TappedRoutedEventArgs e) { search(); }
        private void search()
        {
            SearchBox.Text = "9781449394707";
            srchTxt = SearchBox.Text;
            srchTxt = srchTxt.Replace("-", "");
            SearchBox.Text = srchTxt;
            if (srchTxt.Equals("")) { Message.Show("Search Text cannot be empty!", "Error"); return; }
            //if (App.IsInternetAvailable)
            //  {
            if (check())
            {

                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(Stores));
                }
            }
            else
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(SearchPage));
                }
            }
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
        
    }
}