using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace BookMyBook
{
    public class Items : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private string _Title = string.Empty;
        public string Title
        {
            get
            {
                return this._Title;
            }

            set
            {
                if (this._Title != value)
                {
                    this._Title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }
        private string _Price = string.Empty;
        public string Price
        {
            get
            {
                return this._Price;
            }

            set
            {
                if (this._Price != value)
                {
                    this._Price = value;
                    this.OnPropertyChanged("Price");
                }
            }
        }
        private string _sPrice = string.Empty;
        public string sPrice
        {
            get
            {
                return this._sPrice;
            }

            set
            {
                if (this._sPrice != value)
                {
                    this._sPrice = value;
                    this.OnPropertyChanged("sPrice");
                }
            }
        }

        private string _Author = string.Empty;
        public string Author
        {
            get
            {
                return this._Author;
            }

            set
            {
                if (this._Author != value)
                {
                    this._Author = value;
                    this.OnPropertyChanged("Author");
                }
            }
        }

        private ImageSource _Image = null;
        public string ImageUrl = null;
        public ImageSource Image
        {
            get
            {
                return this._Image;
            }

            set
            {
                if (this._Image != value)
                {
                    this._Image = value;
                    this.OnPropertyChanged("Image");
                }
            }
        }

        
        private string _Link = string.Empty;
        public string Link
        {
            get
            {
                return this._Link;
            }

            set
            {
                if (this._Link != value)
                {
                    this._Link = value;
                    this.OnPropertyChanged("Link");
                }
            }
        }

        
        private string _Description = string.Empty;
        public string Description
        {
            get
            {
                return this._Description;
            }

            set
            {
                if (this._Description != value)
                {
                    this._Description = value;
                    this.OnPropertyChanged("Description");
                }
            }
        }

        private string _Publisher = string.Empty;
        public string Publisher
        {
            get
            {
                return this._Publisher;
            }

            set
            {
                if (this._Publisher != value)
                {
                    this._Publisher = value;
                    this.OnPropertyChanged("Publisher");
                }
            }
        }

        private string _Isbn = string.Empty;
        public string Isbn
        {
            get
            {
                return this._Isbn;
            }

            set
            {
                if (this._Isbn != value)
                {
                    this._Isbn = value;
                    this.OnPropertyChanged("ISBN");
                }
            }
        }
    }

}
