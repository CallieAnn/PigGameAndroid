using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace PigGame.LittlePig
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class SecondActivity : AppCompatActivity
    {
        PigLogic game;
        EditText player1;
        string p1Name;
        EditText player2;
        string p2Name;
        int p1Score;
        int p2Score;
        TextView pOneScore;
        TextView pTwoScore;
        int turn;
        int turnPoints;
        TextView points;
        TextView whoseTurn;
        int roll;
        string dieName;
        ImageView die;
        string winner;

        //Keys for ints saved in bundle
        const string ONE_SCORE = "Player_One_Score";
        const string TWO_SCORE = "Player_Two_Score";
        const string ONE = "Player_One_Name";
        const string TWO = "Player_Two_Name";
        const string TURN_POINTS = "Turn_Points";
        const string TURN = "Turn";
        const string IMAGE = "Die";


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.SecondActivity);

            if (savedInstanceState != null)
            {
                //get the scores, points, turn, and image name from bundle
                p1Score = savedInstanceState.GetInt(ONE_SCORE, 0);
                p2Score = savedInstanceState.GetInt(TWO_SCORE, 0);
                p1Name = savedInstanceState.GetString(ONE, "");
                p2Name = savedInstanceState.GetString(TWO, "");
                turnPoints = savedInstanceState.GetInt(TURN_POINTS, 0);
                turn = savedInstanceState.GetInt(TURN, 0);
                dieName = savedInstanceState.GetString(IMAGE, "die1");


                game = new PigLogic(p1Score, p2Score, turnPoints, turn);

                game.Player1Name = p1Name;
                game.Player2Name = p2Name;
                whoseTurn = FindViewById<TextView>(Resource.Id.turn_label);
                whoseTurn.Text = game.GetCurrentPlayer() + "'s turn";

                int resID = Resources.GetIdentifier(dieName, "drawable", PackageName);
                die = FindViewById<ImageView>(Resource.Id.image);
                die.SetImageResource(resID);
            }

            else
            {
                game = new PigLogic();
                p1Score = game.Player1Score;
                p2Score = game.Player2Score;
                turnPoints = game.TurnPoints;
                die = FindViewById<ImageView>(Resource.Id.image);
                whoseTurn = FindViewById<TextView>(Resource.Id.turn_label);
                p1Name = Intent.GetStringExtra("OneName");
                p2Name = Intent.GetStringExtra("TwoName");
                game.Player1Name = p1Name;
                game.Player2Name = p2Name;
            }

            player1 = FindViewById<EditText>(Resource.Id.player_one);
            player2 = FindViewById<EditText>(Resource.Id.player_two);
            pOneScore = FindViewById<TextView>(Resource.Id.one_score);
            pTwoScore = FindViewById<TextView>(Resource.Id.two_score);
            points = FindViewById<TextView>(Resource.Id.points);

            var rollButton = FindViewById<Button>(Resource.Id.roll_button);
            var endButton = FindViewById<Button>(Resource.Id.end_button);


            pOneScore.Text = p1Score.ToString();
            pTwoScore.Text = p2Score.ToString();
            points.Text = turnPoints.ToString();

            //set the players' names whenever the EditTexts are changed
            //player1.TextChanged += (sender, e) =>
            //{
            //    p1Name = player1.Text;
            //    game.Player1Name = p1Name;
            //};

            //player2.TextChanged += (sender, e) =>
            //{
            //    p2Name = player2.Text;
            //    game.Player2Name = p2Name;
            //};


            rollButton.Click += delegate
            {
                turn = game.Turn;
                whoseTurn.Text = game.GetCurrentPlayer() + "'s turn";

                roll = game.RollDie();

                //set correct die image
                dieName = "die" + roll;
                int resID = Resources.GetIdentifier(dieName, "drawable", PackageName);
                die.SetImageResource(resID);

                //update points
                turnPoints = game.TurnPoints;
                points.Text = turnPoints.ToString();

                //if roll a 1
                bool rollOne = game.CheckForOne();
                if (rollOne)
                {
                    rollButton.Enabled = false;
                }
            };


            endButton.Click += (sender, e) =>
            {
                //check winner
                turn = game.ChangeTurn();
                if (turn == 1)
                {
                    winner = game.CheckForWinner();
                    if (winner != "")
                    {
                        rollButton.Enabled = false;
                        endButton.Enabled = false;
                        UpdateScore();
                        whoseTurn.Text = winner + " wins!";
                    }
                    else
                    {
                        UpdateScore();
                        UpdateTurn();
                        rollButton.Enabled = true;
                        endButton.Enabled = true;
                    }
                }


                else
                {
                    UpdateScore();
                    UpdateTurn();
                    rollButton.Enabled = true;
                    endButton.Enabled = true;
                }

            };

            var newGameButton = FindViewById<Button>(Resource.Id.new_game_button);
            newGameButton.Click += delegate
            {
                var main = new Intent(this, typeof(MainActivity));
                StartActivity(main);
                //game.ResetGame();
                //UpdateScore();
                //UpdateTurn();
                //rollButton.Enabled = true;
                //endButton.Enabled = true;

            };
        }

        public void UpdateScore()
        {
            p1Score = game.Player1Score;
            p2Score = game.Player2Score;
            pOneScore.Text = p1Score.ToString();
            pTwoScore.Text = p2Score.ToString();

        }

        public void UpdateTurn()
        {
            whoseTurn.Text = game.GetCurrentPlayer() + "'s turn";
            turnPoints = game.TurnPoints;
            points.Text = turnPoints.ToString();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt(ONE_SCORE, p1Score);
            outState.PutInt(TWO_SCORE, p2Score);
            outState.PutInt(TURN_POINTS, turnPoints);
            outState.PutInt(TURN, turn);
            outState.PutString(IMAGE, dieName);
            outState.PutString(ONE, p1Name);
            outState.PutString(TWO, p2Name);


            base.OnSaveInstanceState(outState);
        }
    }
}