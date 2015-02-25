﻿using BookMyBook.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234

namespace BookMyBook
{
    /// <summary>
    /// A page that displays a group title, a list of items within the group, and details for
    /// the currently selected item.
    /// </summary>
    public sealed partial class SplitPage2 : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public SplitPage2()
        {
            this.InitializeComponent();

            // Setup the navigation helper
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            Progress.Visibility = Visibility.Visible;
            DataTransferManager.GetForCurrentView().DataRequested += dataTransferManager_DataRequested;
             // Setup the logical page navigation components that allow
            // the page to only show one pane at a time.
            this.navigationHelper.GoBackCommand = new BookMyBook.Common.RelayCommand(() => this.GoBack(), () => this.CanGoBack());
            
            // Start listening for Window size changes 
            // to change from showing two panes to showing a single pane
            Window.Current.SizeChanged += Window_SizeChanged;
            this.InvalidateVisualState();
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
        String text1 = "We are piling up the books for you...", text2 = "We are arranging the books for you...", text3 = "Books are finally arranged for you to see...";
        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            var rect = Window.Current.Bounds;
            Web.Width = rect.Width - 20.0;
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
        
        /// <summary>
        /// Invoked when an item within the list is selected.
        /// </summary>
        /// <param name="sender">The GridView displaying the selected item.</param>
        /// <param name="e">Event data that describes how the selection was changed.</param>
        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Invalidate the view state when logical page navigation is in effect, as a change
            // in selection may cause a corresponding change in the current logical page.  When
            // an item is selected this has the effect of changing from displaying the item list
            // to showing the selected item's details.  When the selection is cleared this has the
            // opposite effect.
            if (this.UsingLogicalPageNavigation()) this.InvalidateVisualState();
        }

        private bool CanGoBack()
        {
            if (this.UsingLogicalPageNavigation())
            {
                return true;
            }
            else
            {
                return this.navigationHelper.CanGoBack();
            }
        }
        private void GoBack()
        {
            if (this.UsingLogicalPageNavigation())
            {
                
            }
            else
            {
                this.navigationHelper.GoBack();
            }
        }

        private void InvalidateVisualState()
        {
            var visualState = DetermineVisualState();
            VisualStateManager.GoToState(this, visualState, false);
            this.navigationHelper.GoBackCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Invoked to determine the name of the visual state that corresponds to an application
        /// view state.
        /// </summary>
        /// <returns>The name of the desired visual state.  This is the same as the name of the
        /// view state except when there is a selected item in portrait and snapped views where
        /// this additional logical page is represented by adding a suffix of _Detail.</returns>
        private string DetermineVisualState()
        {
            if (!UsingLogicalPageNavigation())
                return "PrimaryView";

            // Update the back button's enabled state when the view state changes
            var logicalPageBack = this.UsingLogicalPageNavigation();

            return logicalPageBack ? "SinglePane_Detail" : "SinglePane";
        }

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
        int done = SplitPage1.done;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (SplitPage1.pr[1] == 0) Flipkart(); else ls1.Text = SplitPage1.pr[1]+"";
            if (SplitPage1.pr[2] == 0) Amazon(); else ls2.Text = SplitPage1.pr[2] + "";
            if (SplitPage1.pr[3] == 0) Infibeam(); else ls3.Text = SplitPage1.pr[3] + "";
            if (SplitPage1.pr[4] == 0) Rediff(); else ls4.Text = SplitPage1.pr[4] + "";
            if (SplitPage1.pr[5] == 0) Crossword(); else ls5.Text = SplitPage1.pr[5] + "";
            if (SplitPage1.pr[6] == 0) uRead(); else ls6.Text = SplitPage1.pr[6] + "";
            if (SplitPage1.pr[7] == 0) Landmark(); else ls7.Text=SplitPage1.pr[7] + "";
            Title.Text = SplitPage1.title;
            highlight();
            Web.Source = new Uri(e.Parameter.ToString());
            if (Web.Source.Equals(SplitPage1.h1)) { ListView1.SelectedIndex = 0; }
            if (Web.Source.Equals(SplitPage1.h2)) { ListView1.SelectedIndex = 1; }
            if (Web.Source.Equals(SplitPage1.h3)) { ListView1.SelectedIndex = 2; }
            if (Web.Source.Equals(SplitPage1.h4)) { ListView1.SelectedIndex = 3; }
            if (Web.Source.Equals(SplitPage1.h5)) { ListView1.SelectedIndex = 4; }
            if (Web.Source.Equals(SplitPage1.h6)) { ListView1.SelectedIndex = 5; }
            if (Web.Source.Equals(SplitPage1.h7)) { ListView1.SelectedIndex = 6; }

            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e) { Web.Source = new Uri(SplitPage1.h1); }
        private void StackPanel_Tapped_1(object sender, TappedRoutedEventArgs e) { Web.Source = new Uri(SplitPage1.h2); }
        private void StackPanel_Tapped_2(object sender, TappedRoutedEventArgs e) { Web.Source = new Uri(SplitPage1.h3); }
        private void StackPanel_Tapped_3(object sender, TappedRoutedEventArgs e) { Web.Source = new Uri(SplitPage1.h4); }
        private void StackPanel_Tapped_4(object sender, TappedRoutedEventArgs e) { Web.Source = new Uri(SplitPage1.h5); }
        private void StackPanel_Tapped_5(object sender, TappedRoutedEventArgs e) { Web.Source = new Uri(SplitPage1.h6); }
        private void StackPanel_Tapped_6(object sender, TappedRoutedEventArgs e) { Web.Source = new Uri(SplitPage1.h7); }

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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(ListView1.SelectedIndex)
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
        private void Flipkart()
        {
            try
            {
                //SplitPage1.Flipkart();ls1.Text = SplitPage1.pr[1] + "";
            }
            catch (Exception) { }
            done++; if (done >= 8) Progress.Visibility = Visibility.Collapsed;
        }
        private void Amazon()
        {
            try
            {
                //SplitPage1.Amazon();ls2.Text = SplitPage1.pr[2] + "";
            }
            catch (Exception) { }
            done++; if (done >= 7) Progress.Visibility = Visibility.Collapsed;
        }
        private void Infibeam()
        {

            try
            {
               // SplitPage1.Infibeam(); ls3.Text = SplitPage1.pr[3] + "";
            }
            catch (Exception) { }
            done++; if (done >= 7) Progress.Visibility = Visibility.Collapsed;
        }
        private void Rediff()
        {
            try
            {
               // SplitPage1.Rediff(); ls4.Text = SplitPage1.pr[4] + "";
            }
            catch (Exception) { }
            done++; if (done >= 7) Progress.Visibility = Visibility.Collapsed;
        }
        private void Crossword()
        {
            try
            {
                //SplitPage1.Crossword(); ls5.Text = SplitPage1.pr[5] + "";
            }
            catch (Exception) { }
            done++; if (done >= 7) Progress.Visibility = Visibility.Collapsed;
        }
        private void uRead()
        {
            try
            {
                //SplitPage1.uRead(); ls6.Text = SplitPage1.pr[6] + "";
            }
            catch (Exception) { }
            done++; if (done >= 7) Progress.Visibility = Visibility.Collapsed;
        }
        private void Landmark()
        {

            try
            {
                //SplitPage1.Landmark(); ls7.Text = SplitPage1.pr[7] + "";
            }
            catch (Exception) { }
            done++; if (done >= 7) Progress.Visibility = Visibility.Collapsed;
        }
        private void highlight()
        {
            TextBlock[] p = { ls1, ls2, ls3, ls4, ls5, ls6, ls7 };
            //SplitPage1.highlight();
            for (int i = 0; i < 7; i++)
            {
                if (SplitPage1.pr[i] == 0) p[i].Text = "N/A";
                if (SplitPage1.pr[0] != 0 && SplitPage1.pr[i + 1] == SplitPage1.pr[0]) { p[i].Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.GreenYellow); p[i].FontSize = 27; }
            }
        }
        private void r1_Click(object sender, RoutedEventArgs e) { Flipkart(); }
        private void r2_Click(object sender, RoutedEventArgs e) { Amazon(); }
        private void r3_Click(object sender, RoutedEventArgs e) { Infibeam(); }
        private void r4_Click(object sender, RoutedEventArgs e) { Rediff(); }
        private void r5_Click(object sender, RoutedEventArgs e) { Crossword(); }
        private void r6_Click(object sender, RoutedEventArgs e) { uRead(); }
        private void r7_Click(object sender, RoutedEventArgs e) { Landmark(); }
        

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
        
        
    }
}
