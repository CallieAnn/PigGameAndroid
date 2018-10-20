using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;




namespace PigGame.LittlePig
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        PigLogic game;
        EditText p1;
        EditText p2;
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


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            game = new PigLogic();
            player1 = FindViewById<EditText>(Resource.Id.player_one);
            player2 = FindViewById<EditText>(Resource.Id.player_two);
            pOneScore = FindViewById<TextView>(Resource.Id.one_score);
            pTwoScore = FindViewById<TextView>(Resource.Id.two_score);
            points = FindViewById<TextView>(Resource.Id.points);
            die = FindViewById<ImageView>(Resource.Id.image);
            var rollButton = FindViewById<Button>(Resource.Id.roll_button);
            var endButton = FindViewById<Button>(Resource.Id.end_button);
            whoseTurn = FindViewById<TextView>(Resource.Id.turn_label);

            p1Score = game.Player1Score;
            p2Score = game.Player2Score;
            turnPoints = game.TurnPoints;

            pOneScore.Text = p1Score.ToString();
            pTwoScore.Text = p2Score.ToString();
            points.Text = turnPoints.ToString();


            

            player1.TextChanged += (sender,  e) =>
            {
                p1Name = player1.Text;
                game.Player1Name = p1Name;
            };

            player2.TextChanged += (sender, e) =>
            {
                p2Name = player2.Text;
                game.Player2Name = p2Name;
            };

           
            rollButton.Click += delegate
            {
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
                

                //check winner
                winner = game.CheckForWinner();
                if(winner != "")
                {
                    rollButton.Enabled = false;
                    endButton.Enabled = false;
                    whoseTurn.Text = winner + "wins!"; 
                }
            };

            
            endButton.Click += delegate
            {
                turn = game.ChangeTurn();
                whoseTurn.Text = game.GetCurrentPlayer() + "'s turn";
                rollButton.Enabled = true;
            };

            //var newGameButton = FindViewById<Button>(Resource.Id.new_game_button);
            //newGameButton.Click += delegate
            //{
            //    game.ResetGame();

            //};
        }

        

        


    }
}