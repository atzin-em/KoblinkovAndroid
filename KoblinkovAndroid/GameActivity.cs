using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace KoblinkovAndroid
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity
    {
        public Timer TimeTimer = new Timer();

        public bool firsttouch = true;
        public int Cur_Turns = 0;
        public DateTime Cur_Time = new DateTime(1,1,1,0,0,0);
        public List<Tuple<int[], ImageView>> BarCoords = new List<Tuple<int[], ImageView>>() { };
        public Java.IO.ISerializable Obj_Game_Mode;
        public GridLayout Obj_Game_Grid;
        public TextView TimeScore;
        public TextView TurnScore;
        float ScreenHeight;
        float ScreenWidth;
        public int cur_int;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_game);
            Obj_Game_Mode = Intent.GetSerializableExtra("Mode");
            Typeface KremlinTF = Typeface.CreateFromAsset(this.Assets, "kremlin.ttf");
            TimeTimer.Interval = 1000;
            TimeTimer.Elapsed += TimeTimer_Elapsed;
                
            ScreenHeight = (Resources.DisplayMetrics.HeightPixels);
            ScreenWidth =  (Resources.DisplayMetrics.WidthPixels);

            //Console.WriteLine("SCREEN HEIGHT IS {0} /n SCREEN WIDTH IS {1}", ScreenHeight, ScreenWidth);

            // Declaring views from activity
            Obj_Game_Grid = (GridLayout)FindViewById(Resource.Id.Game_Grid);
            TimeScore = (TextView)FindViewById(Resource.Id.Time_TxtView);
            TurnScore = (TextView)FindViewById(Resource.Id.Turns_TxtView);

            // Set window fade in animation
            Window.SetWindowAnimations(Android.Resource.Animation.FadeIn);

            // Setup view variables 
            TimeScore.SetTypeface(KremlinTF, TypefaceStyle.Normal);
            TurnScore.SetTypeface(KremlinTF, TypefaceStyle.Normal);

            CreateGridFromMode(Obj_Game_Mode.ToString());
            Randomizer(Obj_Game_Mode.ToString());
            

        }

        private void TimeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimeScore.Text = "Time: " + Cur_Time.ToString("mm:ss");
            Cur_Time = Cur_Time.AddSeconds(1);
            //Console.WriteLine(Cur_Time.ToString("mm:ss"));


        }

        public void CreateGridFromMode(string mode)
        {

            //Bars.SetImageResource(Application.Assets.Open()
            int[] SelectedSize = new int[] { };
            Dictionary<string, int[]> GridSizeDictionary = new Dictionary<string, int[]>
            {
                { "Easy"  , new int[] { 4, 4 } },
                { "Normal", new int[] { 6, 4 } },
                { "Hard"  , new int[] { 8, 4 } }
            };
            SelectedSize = GridSizeDictionary[mode];

            ViewGroup.LayoutParams ImgParams = new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.FillParent,
                ViewGroup.LayoutParams.FillParent);



            cur_int = SelectedSize[0];
            Obj_Game_Grid.ColumnCount = SelectedSize[1];
            Obj_Game_Grid.RowCount = SelectedSize[0];

            if (mode == "Easy")
            {
                
                ImgParams.Width = 320;
                ImgParams.Height = 560;
            }
            else if (mode == "Normal")
            {
                ImgParams.Width = 320;
                ImgParams.Height = 360;
            }
            else if (mode == "Hard")
            {
                ImgParams.Width = 320;
                ImgParams.Height = 260;
            }
            
            for (int i = 0; i < GridSizeDictionary[mode][0]; i++)
            {
                for (int x = 0; x < GridSizeDictionary[mode][1]; x++)
                {
                    
                    ImageView BarImg = new ImageView(this);

                    if (mode == "Easy")
                    {
                        BarImg.SetPadding(5, 0, 5, 0);
                    }
                    else if (mode == "Normal")
                    {
                        BarImg.SetPadding(15, 10, 15, 10);
                    }
                    else if (mode == "Hard")
                    {
                        BarImg.SetPadding(35, 15, 35, 15);
                    }
                    BarImg.SetImageResource(Resource.Drawable.bar);
                    BarImg.Id = int.Parse(x + "99" + i);
                    BarImg.LayoutParameters = ImgParams;
                    Obj_Game_Grid.AddView(BarImg);
                    BarImg.Touch += Obj_BarImg_Touch;
                    //BarImg.
                    BarImg.Rotation = 0;
                    BarCoords.Add(new Tuple<int[], ImageView>(
                        new int[] { i, x },
                        BarImg));
                }
            }
        }

        private void Randomizer(string mode)
        {
            if (mode == "Easy")
            {
                RandomSelect(33);
            }
            else if (mode == "Normal")
            {
                RandomSelect(66);
            }
            else if (mode == "Hard")
            {
                RandomSelect(99);
            }
        }

        private void RandomSelect(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                Random rnd = new Random();
                int r = rnd.Next(BarCoords.Count);
                var RandBar = BarCoords[r].Item1;
                //Console.WriteLine("This was the coordinate x:{0} y:{1}", RandBar[0], RandBar[1]);
                RotateBars(RandBar[0], RandBar[1], false);
            }
        }

        private void RotateBars(int x, int y, bool AnimateBars)
        {
            if (AnimateBars == true)
            {
                //Cur_Turns += 1;
                //Context context = this;
                // Get the Resources object from our context
                //TurnScore.Text = "Turns: " + Cur_Turns.ToString();
            }
            Console.WriteLine("RotateBars was called with params x:{0} y:{1}", x, y);
            for (int i = 0; i < BarCoords.Count; i++)
            {
                if (BarCoords[i].Item1[0] == x)
                {
                    if (BarCoords[i].Item2.Rotation == 90)
                    {
                        if (AnimateBars == true)
                        {
                            BarCoords[i].Item2.Animate().Rotation(0);
                        }
                        else
                        {
                            BarCoords[i].Item2.Rotation = 0;
                        }
                    }
                    else if (BarCoords[i].Item2.Rotation == 0)
                    {
                        if (AnimateBars == true)
                        {
                            BarCoords[i].Item2.Animate().Rotation(90);
                        }
                        else
                        {
                            BarCoords[i].Item2.Rotation = 90;
                        }
                    }
                }
                if (BarCoords[i].Item1[1] == y)
                {
                    if (BarCoords[i].Item2.Rotation == 90)
                    {
                        if (AnimateBars == true)
                        {
                            BarCoords[i].Item2.Animate().Rotation(0);
                        }
                        else
                        {
                            BarCoords[i].Item2.Rotation = 0;
                        }
                    }
                    else if (BarCoords[i].Item2.Rotation == 0)
                    {
                        if (AnimateBars == true)
                        {
                            BarCoords[i].Item2.Animate().Rotation(90);
                        }
                        else
                        {
                            BarCoords[i].Item2.Rotation = 90;
                        }
                    }
                }
                if (BarCoords[i].Item1[0] == x && BarCoords[i].Item1[1] == y)
                {
                    if (BarCoords[i].Item2.Rotation == 90)
                    {
                        if (AnimateBars == true)
                        {
                            BarCoords[i].Item2.Animate().Rotation(0);
                        }
                        else
                        {
                            BarCoords[i].Item2.Rotation = 0;
                        }
                    }
                    else if (BarCoords[i].Item2.Rotation == 0)
                    {
                        if (AnimateBars == true)
                        {
                            BarCoords[i].Item2.Animate().Rotation(90);
                        }
                        else
                        {
                            BarCoords[i].Item2.Rotation = 90;
                        }
                    }
                }
            }
        }

        private bool GameWonCheck()
        {
            bool BarsCorrect = false;
            for (int i = 0; i < BarCoords.Count; i++)
            {
                if (BarCoords[i].Item2.Rotation == 0)
                {
                    BarsCorrect = true;
                }
                else if (BarCoords[i].Item2.Rotation == 90)
                {
                    BarsCorrect = false;
                }
            }
            return BarsCorrect;
        }

        private void Obj_BarImg_Touch(object sender, View.TouchEventArgs e)
        {
            if (firsttouch == true)
            {
                TimeTimer.Start();
                firsttouch = !firsttouch;
            }
            if (e.Event.Action == MotionEventActions.Up)
            {
                for (int i = 0; i < BarCoords.Count; i++)
                {
                    if (BarCoords[i].Item2 == sender)
                    {
                        Console.WriteLine("Coords of the touched bar x:{0} y:{1}", BarCoords[i].Item1[0], BarCoords[i].Item1[1]);
                        //Console.WriteLine(TouchedBar.Id);
                        //Console.WriteLine(BarCoords[i].Item2.Rotation);
                        RotateBars(BarCoords[i].Item1[0], BarCoords[i].Item1[1], false); //Turned off animations due to delay in user input
                        //TouchedBar.Animate().RotationBy(90);
                    }
                }
                //if (GameWonCheck() == true)
                //{
                //    Console.WriteLine("You won!");
                //}
            }
        }
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                StartActivity(typeof(MainActivity));
                OverridePendingTransition(Android.Resource.Animation.FadeIn, Android.Resource.Animation.FadeOut);
                return false;
            }
            return true;
        }
    }
}