using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App20
{
    class DadPlayer
    {
        protected float x;
        protected float y;
        protected float vx;
        protected float vy;
        protected Bitmap bitmap;
        protected static float vMaxPow2 = 200; 

        public DadPlayer(float x, float y, Bitmap bitmap)
        {
            this.x = x;
            this.y = y;
            this.bitmap = bitmap;
        }

        public float GetX()
        {
            return x;
        }
        public float GetY()
        {
            return this.y;
        }
        public float GetVx()
        {
            return this.vx;
        }
        public float GetVy()
        {
            return this.vy;
        }
        public Bitmap GetBitmap()
        {
            return this.bitmap;
        }

        public void SetX(float x)
        {
            this.x = x;
        }
        public void SetY(float y)
        {
            this.y = y;
        }
        public void SetVX(float vx)
        {
            this.vx = vx;
        }
        public void SetVY(float vy)
        {
            this.vy = vy;
        }
        public void SetBitmap(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public void Draw(Canvas c)
        {
            Paint p = new Paint();
            p.Color = Color.Green;

            c.DrawBitmap(this.bitmap, this.x, this.y, null);
            c.DrawCircle(x, y, (float)Math.Sqrt(vMaxPow2), new Paint());
        }

        public bool Collision(Ball b) //בדיקה האם הייתה התנגשות בין השחקן לכדור
        {
            float xMiddlePlayer = this.x + this.bitmap.Width / 2;
            float yMiddlePlayer = this.y + this.bitmap.Height / 2;
            float xMiddleBall = b.GetX() + b.GetBitmap().Width / 2;
            float yMiddleBall = b.GetY() + b.GetBitmap().Height / 2;
            float player_ballR = (this.bitmap.Width / 2) + (b.GetBitmap().Width / 2);
            if (Math.Pow(xMiddlePlayer - xMiddleBall, 2) +
                Math.Pow(yMiddlePlayer - yMiddleBall, 2) <=
                player_ballR * player_ballR)
                return true;
            return false;
        }

        //public void CourtCollision(float h, float w) //שמירה על השחקנים בגבולות המסך, שלא יצאו ממנו
        //{
        //    if (this.x < 0)
        //        this.x = 0;
        //    if (this.x > w - this.bitmap.Width)
        //        this.x = w - this.bitmap.Width;
        //    if (this.y < 0)
        //        this.y = 0;
        //    if (this.y > h - this.bitmap.Height)
        //        this.y = h - this.bitmap.Width;
        //}
    }
}