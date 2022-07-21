using System.Media;

namespace Space_Game
{
    public partial class Form1 : Form
    {
        public bool isHidden(Label label)
        {
            //if the text is in ALL CAPS then it is hidden
            string text = label.Text;
            if (text == text.ToUpper())
                return true;
            return false;
        }
        string path = "C:/Users/ALIENWARE/source/repos/Space Game/Space Game/images/";
        string extension = ".png";
        Random random = new Random();

        List<string> celestials = new List<string>{"mercury", "venus", "earth", "mars", "jupiter", "saturn", "uranus", "neptune", "pluto", "moon", "callisto", "enceladus",
                                                        "europa", "ganymede", "iapetus", "io", "mimas", "titan" };

        Dictionary<string, string> planetFacts = new Dictionary<string, string>() { { "mercury","" }, { "venus","" }, { "earth", "" }, { "mars", "" }, { "jupiter", "" }, { "saturn", "" }
                                                                                     , { "uranus", "" }, { "neptune", "" }, { "pluto", "" }, { "moon", "" }, { "callisto", "" }
                                                                                     , { "enceladus", "" } , { "europa", "" } , { "ganymede", "" } , { "iapetus", "" } , { "io", "" } , { "mimas", "" }, { "titan", "" }};

        Label firstClicked = null;
        Label secondClicked = null;
        string[] iconPair = new string[] { "", "" };
        int remainingPairs = 18;
        bool won = false;
        bool over = false;

        SoundPlayer planetClick = new SoundPlayer("C:/Users/ALIENWARE/source/repos/Space Game/Space Game/sfx/planet_click.wav");
        SoundPlayer match = new SoundPlayer("C:/Users/ALIENWARE/source/repos/Space Game/Space Game/sfx/match.wav");
        SoundPlayer win = new SoundPlayer("C:/Users/ALIENWARE/source/repos/Space Game/Space Game/sfx/win.wav");
        SoundPlayer loss = new SoundPlayer("C:/Users/ALIENWARE/source/repos/Space Game/Space Game/sfx/loss.wav");
        SoundPlayer background = new SoundPlayer("C:/Users/ALIENWARE/source/repos/Space Game/Space Game/sfx/background.wav");
        public Form1()
        {
            LanguageSelect();
            InitializeComponent();
            AssignImagesToSquares();
        }
        private void LanguageSelect()
        {
            int i = 0;
            foreach (string line in File.ReadLines("C:/Users/ALIENWARE/source/repos/Space Game/Space Game/funfacts_ro.txt"))
            {

                if (line == "")
                {
                    i++;
                }
                else
                {
                    planetFacts[celestials[i]] += line + "\n";
                }
                
            }

        }
        private void AssignImagesToSquares()
        {
            List<string> newGameInstance = new List<string> {"mercury", "venus", "earth", "mars", "jupiter", "saturn", "uranus", "neptune", "pluto", "moon", "callisto", "enceladus",
                                                        "europa", "ganymede", "iapetus", "io", "mimas", "titan", "mercury", "venus", "earth", "mars", "jupiter", "saturn", "uranus", "neptune", "pluto", "moon", "callisto", "enceladus",
                                                        "europa", "ganymede", "iapetus", "io", "mimas", "titan"};
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(newGameInstance.Count);

                    string imageName = newGameInstance[randomNumber];
                    iconLabel.Text = imageName.ToUpper();

                    //Debugging here - clean after
                    iconLabel.ForeColor = Color.Black;

                    newGameInstance.RemoveAt(randomNumber);
                }
            }
        }
        private void labelClick(object sender, EventArgs e)
        {
            if (won == false)
            {
               
                Label clickedLabel = sender as Label;
                if (clickedLabel != null)
                {
                    if (!isHidden(clickedLabel) || over)
                        return;
                    if (firstClicked != null && secondClicked != null)
                        return;

                    //clickedLabel.Image = Image.FromFile(path + clickedLabel.Text + extension);
                    //clickedLabel.Text = "";

                    if (firstClicked == null)
                    {
                        firstClicked = clickedLabel;

                        iconPair[0] = clickedLabel.Text;
                        clickedLabel.Image = Image.FromFile(path + clickedLabel.Text.ToLower() + extension);
                        clickedLabel.Text = clickedLabel.Text.ToLower();
                        planetClick.Load();
                        planetClick.Play();


                        timer1.Start();

                        return;
                    }

                    secondClicked = clickedLabel;

                    iconPair[1] = clickedLabel.Text;
                    clickedLabel.Image = Image.FromFile(path + clickedLabel.Text.ToLower() + extension);
                    clickedLabel.Text = clickedLabel.Text.ToLower();
                    planetClick.Load();
                    planetClick.Play();

                    timer1.Start();


                }
            }
            else if(over == false)
            {
                Label clickedLabel = sender as Label;
                match.Load();
                match.Play();
                string planet = clickedLabel.Text.ToLower();
                MessageBox.Show(planetFacts[planet]);
            }
        }
        private void labelDoubleClick(object sender, EventArgs e)
        {
            if (remainingPairs == 0)
            {
                won = false;
                remainingPairs = 18;
                InitializeComponent();
                AssignImagesToSquares();
            }
        }

        private void timer(object sender, EventArgs e)
        {
            timer1.Stop();
            if (iconPair[0] != iconPair[1])
            {
                firstClicked.Text = firstClicked.Text.ToUpper();
                firstClicked.Image = Image.FromFile(path + "invalid" + extension);
                if (iconPair[1] != "")
                {
                    secondClicked.Text = secondClicked.Text.ToUpper();
                    secondClicked.Image = Image.FromFile(path + "invalid" + extension);
                }
            }
            else
            {
                if (remainingPairs != 1)
                {
                    match.Load();
                    match.Play();
                }
                remainingPairs--;
            }
            firstClicked = null;
            secondClicked = null;
            iconPair[0] = "";
            iconPair[1] = "";

            CheckForWinner();
        }
        private void CheckForWinner()
        {
            //Debugging cheat
            //remainingPairs = 0;
            //End cheat code
            if (remainingPairs == 0)
            {
                won = true;
                timer2.Stop();
                win.Load();
                win.Play();
                MessageBox.Show("Felicitari. Ai castigat!" + Environment.NewLine + "Da click pe oricare icoana pentru a afla curioziatai despre corpul ceresc respectiv.");
            }
        }

        private void label9_DoubleClick(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void gameOver(object sender, EventArgs e)
        {
            timer2.Stop();
            if (won != true)
            {
                loss.Load();
                loss.Play();
                MessageBox.Show("Ne pare rau. Ai pierdut jocul. Incearca din nou!");
                over = true;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            MessageBox.Show("Bine ai venit la aventura in spatiu! Ai timp la dispozitie 4:30 minute ca sa potrivesti toate icoanele");
            timer2.Start();
        }
    }
}