using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.System;
namespace BookMyBook
{
    public sealed partial class MainPage : Page
    {
        public static string isbn = "", srchTxt = "";
        public MainPage()
        {
            this.InitializeComponent();
            Enter.QueryText = "";
            Window.Current.SizeChanged += Window_SizeChanged;
        }
        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            var rect = Window.Current.Bounds;
            PageTitle.Width = rect.Width - 340.0;
        }
        private bool checkisbn(long n,int l)
        {
            long m = n;
            m = m / 10;
            int sum = 0;
            if (l == 10)
            {
                for (int i = 1; i < 10; i++)
                {
                    sum = sum + (int)((10-i) * (m % 10));
                    m /= 10;
                }
                sum = sum % 11;
            }
            else if (l == 13)
            {
                for (int i = 1; i < 13; i++)
                {
                    if(i%2==0)
                    sum = sum + (int)(1 * (m % 10));
                    else sum = sum + (int)(3 * (m % 10));
                    m /= 10;
                }
                sum =10- sum % 10;
            }
            if (sum == (n % 10))
                return true;
            else return false;
        }
        private bool check()
        {
            if (!(srchTxt.Length == 10 || srchTxt.Length == 13)) {  return false; }
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
        private void ShowPopupAnimationClicked(String s)
        {
            if (!LightDismissAnimatedPopup.IsOpen)
            {
                errortext.Text = s;
                LightDismissAnimatedPopup.IsOpen = true;
            }
        }
        private void CloseAnimatedPopupClicked(object sender, RoutedEventArgs e)
        {
            if (LightDismissAnimatedPopup.IsOpen) { LightDismissAnimatedPopup.IsOpen = false; }
        }
        private void pop(string s)
        {
            ShowPopupAnimationClicked("OOPS :( :( :(\n" + s);
        }
        private void Grid_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter) NavigateToNextPage();
        }
        private void Search_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            NavigateToNextPage();
        }
        private void NavigateToNextPage()
        {
            srchTxt = Enter.QueryText;
            srchTxt = srchTxt.Replace("-", "");
            Enter.QueryText = srchTxt;
            if (srchTxt.Equals("")) { ShowPopupAnimationClicked("OOPS :( :( :(\nSearch Text cannot be empty!"); return; }
            //if (App.IsInternetAvailable)
          //  {
                if (check())
                {

                    if (this.Frame != null)
                    {
                        this.Frame.Navigate(typeof(SplitPage1));
                    }
                }
                else
                {
                    if (this.Frame != null)
                    {
                        this.Frame.Navigate(typeof(ItemsPage1));
                    }
                }
          //  }
          //  else { pop("No internet connection found."); }
        }
        
    }
}
