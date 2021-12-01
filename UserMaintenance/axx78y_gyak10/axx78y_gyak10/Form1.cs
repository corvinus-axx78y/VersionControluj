using axx78y_gyak10.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace axx78y_gyak10
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();

        List<SimulatedResults> szimulalteredmeny = new List<SimulatedResults>();

        Random rng = new Random(1234);
        public Form1()
        {
            InitializeComponent();

            
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");

            


        }

        private void Simulation(int ev)
        {
            for (int year = 2005; year <= ev; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();

                SimulatedResults sr = new SimulatedResults();
                sr.year = year.ToString();
                sr.men = nbrOfMales.ToString();
                sr.women = nbrOfFemales.ToString();

                szimulalteredmeny.Add(sr);

                Console.WriteLine(
                string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
            }

            DisplayResults();
        }

        private void SimStep(int year, Person person)
        {
            //Ha halott akkor kihagyjuk, ugrunk a ciklus következő lépésére
            if (!person.IsAlive) return;

            // Letároljuk az életkort, hogy ne kelljen mindenhol újraszámolni
            byte age = (byte)(year - person.BirthYear);

            // Halál kezelése
            // Halálozási valószínűség kikeresése
            double pDeath = (from x in DeathProbabilities
                             where x.nem == person.Gender && x.kor == age
                             select x.valoszinuseg).FirstOrDefault();
            // Meghal a személy?
            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;

            //Születés kezelése - csak az élő nők szülnek
            if (person.IsAlive && person.Gender == Gender.Female)
            {
                //Szülési valószínűség kikeresése
                double pBirth = (from x in BirthProbabilities
                                 where x.kor == age
                                 select x.valoszinuseg).FirstOrDefault();
                //Születik gyermek?
                if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }

        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }
            return population;
        }

        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> birthprobabilities = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birthprobabilities.Add(new BirthProbability()
                    {
                        kor = int.Parse(line[0]),
                        gyermekekszama = int.Parse(line[1]),
                        valoszinuseg = double.Parse(line[2])
                    });
                }
            }
            return birthprobabilities;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> deathprobabilities = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathprobabilities.Add(new DeathProbability()
                    {
                        nem = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        kor = int.Parse(line[1]),
                        valoszinuseg = double.Parse(line[2])
                    });
                }
            }

            return deathprobabilities;
        }

        private void browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ok = new OpenFileDialog();

            if (ok.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ok.FileName;
            }
        }

        private void DisplayResults()
        {
            for (int i = 0; i < szimulalteredmeny.Count; i++)
            {
                richTextBox2.Text += "Szimulációs év: " + szimulalteredmeny[i].year + "\n" + "\t Fiúk: " + szimulalteredmeny[i].men + "\n" + "\t Lányok: " + szimulalteredmeny[i].women + "\n";
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            szimulalteredmeny.Clear();
            richTextBox2.Text = "";
            Population = GetPopulation(textBox1.Text);
            Simulation((int)numericUpDown1.Value);
        }
    }
}


