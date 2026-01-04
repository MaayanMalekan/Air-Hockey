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
    class Option
    {
        private float x;
        private float y;
        private int t;

        public Option(float x, float y, int t)
        {
            this.x = x;
            this.y = y;
            this.t = t;
        }

        public float GetX()
        {
            return x;
        }
        public float GetY()
        {
            return this.y;
        }

        public int GetT()
        {
            return this.t;
        }
    }
}