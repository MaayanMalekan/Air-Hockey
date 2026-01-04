using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App20
{
    class ListOptions
    {
        private List<Option> options;

        public ListOptions()
        {
            this.options = new List<Option>();
        }

        public List<Option> GetOptions()
        {
            return options;
        }

        public Option GetOption(int i)
        {
            return this.options[i];
        }


    }
}