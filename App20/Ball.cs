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
    class Ball
    {
        private float x;
        private float y;
        private float vx;
        private float vy;
        private Bitmap bitmap;
        public int scoreM;
        public int scoreU;

        public Ball()
        {
        }
        public Ball(Context context)
        {
            this.bitmap = BitmapFactory.DecodeResource(context.Resources,Resource.Drawable.ball);
            this.vx = 0;
            this.vy = 0;
            this.scoreM = 0;
            this.scoreU = 0;
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
        public int GetScoreM()
        {
            return this.scoreM;
        }
        public int GetScoreU()
        {
            return this.scoreU;
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
             c.DrawBitmap(this.bitmap, this.x, this.y, null);
            Console.WriteLine("y =  " + y);
         }

        public bool MoveAccordingV(float h, float w) //אחראית על הזזת הכדור בהתאם למהירות שנקבעה לו
        {

            this.x += this.vx;
            this.y += this.vy;

            if (this.y < 0 && this.x > 363 && this.x < 739) //בדיקה האם הכדור נכנס לשער העליון
            {
                this.vx = 0;
                this.vy = 0;
                this.x = w / 2 - (this.bitmap.Width) / 2;
                this.y = h / 2 - (this.bitmap.Height) / 2;
                this.scoreM++;
                DrawSurfaceView.userPoint.Start();
                return true;
            }
            else if (this.y >= h- this.bitmap.Height && this.x > 363 && this.x < 739) // בדיקה האם הכדור נכנס לשער התחתון
            {
                this.vx = 0;
                this.vy = 0;
                this.x = w / 2 - (this.bitmap.Width) / 2;
                this.y = h / 2 - (this.bitmap.Height) / 2;
                this.scoreU++;
                DrawSurfaceView.enemyPoint.Start();              
                return true;
            }
            else //טיפול במקרה של פגיעת הכדור הקצוות המסך
            {
                if (this.x < 0 || this.x > w - this.bitmap.Width)
                    this.vx = -1 * this.vx;
                if (this.y < 0 || this.y > h - this.bitmap.Height)
                    this.vy = -1 * this.vy;
            }
            return false;
        }

        
    }
}