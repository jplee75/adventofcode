﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    class Program
    {
        static List<int> caughtMeLayer = new List<int>();
        static int currentPicoSecond = 0;
        static bool iGotCaught = true;
        static int startingPicoSecond=0;

        static void Main(string[] args)
        {
            DotOne();
            DotTwo();
        }

        static void DotOne()
        {
            var puzzleInput = GetInput();

            for (currentPicoSecond = 0; currentPicoSecond <= puzzleInput.Max(p => p.Depth); currentPicoSecond++)
            {
                int caughtCheck = puzzleInput.Where(x => x.Depth == currentPicoSecond).Select(y => y.CurrentRange).FirstOrDefault();
                if (caughtCheck == 1)
                {
                    caughtMeLayer.Add(currentPicoSecond);
                }

                //move scanners
                puzzleInput.ForEach(p => p.CurrentRange += 1);
            }

            var answer = puzzleInput.Join(caughtMeLayer, p => p.Depth, c => c, (puz, caught) => puz.Depth * puz.MaxRange).Sum();

            Console.WriteLine(answer);
            Console.ReadKey();
        }

        static void DotTwo()
        {
            var puzzleInput = GetInput();
            List<Firewall> checkMe;
            while (iGotCaught)
            {
                checkMe = puzzleInput.Clone().ToList();
                for (currentPicoSecond = startingPicoSecond; currentPicoSecond - startingPicoSecond <= puzzleInput.Max(p => p.Depth); currentPicoSecond++)
                {
                    int caughtCheck = checkMe.Where(x => x.Depth == currentPicoSecond - startingPicoSecond).Select(y => y.CurrentRange).FirstOrDefault();
                    if (caughtCheck == 1)
                    {
                        caughtMeLayer.Add(currentPicoSecond - startingPicoSecond);
                        break;
                    }
                    checkMe.ForEach(p => p.CurrentRange += 1);
                }

                puzzleInput.ForEach(p => p.CurrentRange += 1);

                iGotCaught = caughtMeLayer.Count() > 0;
                caughtMeLayer.Clear();
                checkMe.Clear();
                startingPicoSecond += 1;
            }

            var answer = startingPicoSecond-1;

            Console.WriteLine(answer);
            Console.ReadKey();
        }

        static List<Firewall> GetInput()
        {
            List<Firewall> returnMe = new List<Firewall>();
            var lines =  File.ReadAllLines("~/../../../Input").ToList();
            foreach(string line in lines)
            {
                Firewall addMe = new Firewall() { Depth = int.Parse(line.Split(':')[0]), MaxRange = int.Parse(line.Split(':')[1]), CurrentRange = 1 };
                returnMe.Add(addMe);
            }

            return returnMe;
        }
    }

    class Firewall:ICloneable
    {
        private int _currentRange;

        public int Depth { get; set; }
        public int MaxRange { get; set; }

        public int CurrentRange
        {
            get
            {
                return _currentRange;
            }
            set
            {
                if (_moveDown)
                {
                    _currentRange -= 1;
                    if(_currentRange == 1)
                    {
                        _moveDown = false;
                    }
                }
                else
                {
                    _currentRange = value;
                    if(_currentRange==MaxRange)
                    {
                        _moveDown = true;
                    }
                }
            }
        }

        private bool _moveDown = false;

        public object Clone()
        {
            return (Firewall)this.MemberwiseClone();
        }
    }

    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
