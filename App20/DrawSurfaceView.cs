using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace App20
{
    class DrawSurfaceView : SurfaceView
    {
        Ball ball;
        public static Enemy computer;
        public static User user;
        bool start = true;
        Paint p = new Paint();
        int scoreM, scoreU;
        float canvasH, canvasW;
        public Context context;
        Bitmap[] b = new Bitmap[6];
        public static MediaPlayer enemyPoint, lose, startSound, userPoint, win;
        public static AudioManager am;
       
        Dialog winDialog, loseDialog;
        bool showDialog = true;

        public bool threadRunning = true;
        public bool isRunning = true;


        public Thread t;
        ThreadStart ts;
        public int cycle = 0;
        Bitmap back;

        public DrawSurfaceView(Context context) : base(context)
        {
            enemyPoint = MediaPlayer.Create(context, Resource.Raw.enemyPoint);
            lose = MediaPlayer.Create(context, Resource.Raw.lose);
            startSound = MediaPlayer.Create(context, Resource.Raw.start);
            userPoint = MediaPlayer.Create(context, Resource.Raw.userPoint);
            win = MediaPlayer.Create(context, Resource.Raw.win);

            DrawSurfaceView.startSound.Start();
            this.context = context;
            ball = new Ball(context);
            if (user == null)
                user = new User(context, true);
            else
                user = new User(context);
            if (computer == null)
                computer = new Enemy(context, true);
            else
                computer = new Enemy(context);
            if (user.GetBitmap() == null)
                user.SetBitmap(BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.red_palyer));
            if (computer.GetBitmap() == null)
                computer.SetBitmap(BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.blue_player));

            p.SetARGB(255, 225, 255, 255);
            p.TextSize = 70;
            p.StrokeWidth = 50;
            
            back = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.game_background);

            winDialog = new Dialog(context);
            loseDialog = new Dialog(context);

            
            ts = new ThreadStart(Run);
            t = new Thread(ts);
        }
          
        
        public void destroy()
        {
            isRunning = false;
            ((GameActivity)context).Finish();
        }

        public void pause()
        {
            isRunning = false;
        }

        public void resume()
        {
            isRunning = true;
        }

        public void startGame()
        {
            isRunning = true;
        }
        
        public void Run()
        {
            bool wasCollision;
            bool bStart = true;
            while (threadRunning)
            {
                if (isRunning)
                {
                    if (!this.Holder.Surface.IsValid)
                        continue;
                    Canvas c = null;
                    try
                    {
                        c = this.Holder.LockCanvas();
                        if (bStart)
                        {
                            canvasH = c.Height;
                            canvasW = c.Width;
                            bStart = false;
                        }
                        cycle++;
                        wasCollision = false;
                        Rect s = new Rect(0, 0, back.Width, back.Height);
                        Rect r = new Rect(0, 0, c.Width, c.Height);
                        c.DrawBitmap(back, s, r, null);                      
                        c.DrawText(scoreM + ":" + scoreU, canvasW - 150, 100, p);
                        if (start)
                        {                        
                            ball.SetX(c.Width / 2 - (ball.GetBitmap().Width) / 2);
                            ball.SetY(c.Height / 2 - (ball.GetBitmap().Height) / 2);
                        }
                        
                        if (user.Collision(ball))
                        {
                            ball.SetVX(user.GetVx() * 2);
                            ball.SetVY(user.GetVy() * 2);//לשנות מהירות של כדור 
                            wasCollision = true;
                        }
                        else if (computer.Collision(ball))
                        {
                            ball.SetVX(computer.GetVx() * 2);
                            ball.SetVY(computer.GetVy() * 2);//לשנות מהירות של כדור 
                            wasCollision = true;
                        }
                        if (ball.MoveAccordingV(canvasH, canvasW))
                        {
                            scoreM = ball.GetScoreM();
                            scoreU = ball.GetScoreU();
                            if (scoreM == 7 && showDialog)
                            {
                                showDialog = false;
                                Action action = new Action (createWinDialog);
                                ((Activity)context).RunOnUiThread(new Java.Lang.Runnable(action){ });
                                win.Start();
                            }
                            else if (scoreU == 7 && showDialog)
                            {
                                showDialog = false;
                                Action action = new Action(createLoseDialog);
                                ((Activity)context).RunOnUiThread(new Java.Lang.Runnable(action) { });
                                lose.Start();
                            }

                            SetStart();
                        }
                        //if (!wasCollision && me.Collision(ball))
                        //{
                        //    ball.SetVX(me.GetVx());
                        //    ball.SetVY(me.GetVy());//לשנות מהירות של כדור 
                        //    wasCollision = true;
                        //}
                        computer.Move(canvasH);
                        
                        ball.Draw(c);
                        user.Draw(c);
                        computer.Draw(c);
                        start = false;
                        user.CourtCollision(canvasH, canvasW);
                        computer.CourtCollision(canvasH, canvasW);

                        if (computer.Collision(ball)) //אמורה להיות מהירות אחרת- זה רק לבדיקה
                        {
                            ball.SetVX(computer.GetVx() * 2);
                            ball.SetVY(computer.GetVy() * 2);
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    finally
                    {
                        if (c != null)
                        {
                            this.Holder.UnlockCanvasAndPost(c);
                        }
                    }
                }
            }
        }//end of run

        public void createWinDialog() //יצירה של מסך דיאלוג במקרה של ניצחון
        {
            winDialog.SetContentView(Resource.Layout.win_custom_layout);
            winDialog.SetTitle("");
            winDialog.SetCancelable(true);

            Button btnRestart = winDialog.FindViewById<Button>(Resource.Id.btnRestart);
            Button btnExit = winDialog.FindViewById<Button>(Resource.Id.btnExit);

            btnRestart.Click += btnRestart_click;
            btnExit.Click += btnExit_click;

            winDialog.Show();
        }

        public void createLoseDialog() //יצירה של מסך דיאלוג במקרה של הפסד
        {
            loseDialog.SetContentView(Resource.Layout.lose_custom_layout);
            loseDialog.SetTitle("");
            loseDialog.SetCancelable(true);

            Button btnRestart = winDialog.FindViewById<Button>(Resource.Id.btnRestart);
            Button btnExit = winDialog.FindViewById<Button>(Resource.Id.btnExit);

            btnRestart.Click += btnRestart_click;
            btnExit.Click += btnExit_click;

            winDialog.Show();
        }

        private void btnRestart_click(object sender, EventArgs e)
        {
            Intent intent = new Intent(context, typeof(GameActivity));
            context.StartActivity(intent);
            winDialog.Dismiss();
        }

        private void btnExit_click(object sender, EventArgs e)
        {
            Activity activity = (Activity)context;
            activity.FinishAffinity();
        }

        private void SetStart() //הצבת ערכים התחלתיים של האובייקטים במשחק
        {
            user.SetX(canvasW / 2 - (user.GetBitmap().Width / 2));
            user.SetY(canvasH - user.GetBitmap().Height);
            user.SetVX(0);
            user.SetVY(0);
            computer.SetX(canvasW / 2 - (computer.GetBitmap().Width / 2));
            computer.SetY(0);
            computer.SetVX(0);
            computer.SetVY(0);
        } 

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (MotionEventActions.Down == e.Action)
            {
                //Option option = enemy.BestOption(ball, canvasW, canvasH);
                //float xo = option.GetX();
                //float yo = option.GetY();
                //float t = option.GetT();
                //float dX = xo - enemy.GetX();
                //float dY = yo - enemy.GetY();
                //enemy.SetVX(dX / t);
                //enemy.SetVY(dY / t);
                //if (enemy.Collision(ball)) //אמורה להיות מהירות אחרת- זה רק לבדיקה
                //{
                //    ball.SetVX(enemy.GetVx() * 2);
                //    ball.SetVY(enemy.GetVy() * 2);
                //}
                user.Move(e.GetX(), e.GetY(), true, canvasH);
                if (user.Collision(ball)) //אמורה להיות מהירות אחרת- זה רק לבדיקה
                {
                    ball.SetVX(user.GetVx() * 2);
                    ball.SetVY(user.GetVy() * 2);
                    //bool bRes = enemy.InDownGoal(ball, canvasH, canvasW);
                    //if (bRes)                                               //debug
                    //    bRes = bRes;
                   // Toast.MakeText(context, bRes.ToString()+"        Down ", ToastLength.Long).Show();
                }
            }

            if (MotionEventActions.Move == e.Action)
            {
                user.Move(e.GetX(), e.GetY(), false, canvasH);
                if (user.Collision(ball)) //אמורה להיות מהירות אחרת- זה רק לבדיקה
                {
                    ball.SetVX(user.GetVx() * 2);
                    ball.SetVY(user.GetVy() * 2);

                    // -----Best Option ----------
                    computer.ComputerHit(ball, canvasW, canvasH);
                    
                }
            }

            //if (MotionEventActions.Up == e.Action)
            //{
            //    coordx = e.GetX();
            //    coordy = e.GetY();
            //    Toast.MakeText(context, "coord:(" + coordx + "," + coordy + ")", ToastLength.Long).Show();
            //}
            return true;
        }
    }
}
