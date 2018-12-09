using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tinf.Lab
{
    public class VjerojatnosnaMatematika
    {
        public bool ValuesValid { get; set; } = true;
        public List<List<double>> MatricaVjeojatnost { get; set; }
        public uint MatrixWidth => (uint)(MatricaVjeojatnost == null ? 0 : MatricaVjeojatnost[0].Count);
        public uint MatrixHeight => (uint)(MatricaVjeojatnost == null ? 0 : MatricaVjeojatnost.Count);

        public IEnumerable<double> VjerojatnostiX() => MatricaVjeojatnost.Select(i => i.Sum());
        public IEnumerable<double> VjerojatnostiY() => MatricaVjeojatnost
            .Aggregate((i, j) => i.Add(j).ToList());
        public VjerojatnosnaMatematika Normaliziraj()
        {
            var sum = VjerojatnostiX().Sum();
            var k = 1 / (double)sum;
            return new VjerojatnosnaMatematika
            {
                MatricaVjeojatnost = MatricaVjeojatnost.Select(i => i.Select(j => j * k).ToList()).ToList()
            };
        }
        public VjerojatnosnaMatematika Randomiziraj()
        {
            var rand = new Random();
            return new VjerojatnosnaMatematika
            {
                MatricaVjeojatnost = MatricaVjeojatnost.Select(i => i.Select(j => rand.NextDouble()).ToList()).ToList()
            }.Normaliziraj();
        }
        public bool SumValid(double epsilon = 10e-6) => Math.Abs(1 - VjerojatnostiX().Sum()) < epsilon;

    }
}
