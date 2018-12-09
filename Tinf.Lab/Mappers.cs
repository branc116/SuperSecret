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
    public static class Mappers
    {
        public static VjerojatnosnaMatematika ToVjerojatnosnaMatematika(uint height, uint width)
        {
            var vm = new VjerojatnosnaMatematika();
            vm.MatricaVjeojatnost = Enumerable
                .Range(0, (int)height)
                .Select(i => Enumerable
                    .Range(0, (int)width)
                    .Select(j => 0.0)
                    .Cast<double>()
                    .ToList())
                .ToList();

            return vm;
        }
        public static DataGridView ToDataGidView(this VjerojatnosnaMatematika vm)
        {
            var _matrixDataGridView = new DataGridView
            {
                RowHeadersWidth = 64,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.None,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            for (uint j = 1; j <= vm.MatrixWidth; ++j)
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = column.HeaderText = "y" + j.ToString();
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                _matrixDataGridView.Columns.Add(column);
            }
            
            for (uint i = 1; i <= vm.MatrixHeight; ++i)
            {
                var rowIndex = _matrixDataGridView.Rows.Add(vm.MatricaVjeojatnost[(int)i - 1].Select(j => j.ToString()).Cast<object>().ToArray());
                //var row = new DataGridViewRow();
                //row.Cells.Add()
                //row.HeaderCell.Value = $"   x{i}";
                //var j = 0;
                var row = _matrixDataGridView.Rows[rowIndex];
                row.HeaderCell.Value = $"   x{i}";
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.White;
                }

            }
            return _matrixDataGridView;
        }
        public static VjerojatnosnaMatematika ToVjerojatnosnaMatematika(this DataGridView dgv)
        {
            var vm = new VjerojatnosnaMatematika();
            var MatrixHeight = dgv.RowCount;
            var MatrixWidth = dgv.ColumnCount;
            var value = Enumerable
                .Range(0, MatrixHeight)
                .Select(i => Enumerable
                    .Range(0, MatrixWidth)
                    .Select(j => 0.0)
                    .Cast<double>()
                    .ToList())
                .ToList();

            // Attempt to parse all values
            for (int i = 0; i < MatrixHeight; ++i)
            {
                for (int j = 0; j < MatrixWidth; ++j)
                {
                    DataGridViewCell cell = dgv.Rows[i].Cells[j];
                    try
                    {
                        value[i][j] = double.Parse(cell.Value.ToString().Replace(',', '.'), CultureInfo.InvariantCulture);
                        if ((value[i][j] < 0.0) || (value[i][j] > 1.0))
                        {
                            throw new Exception();
                        }
                        cell.Style.BackColor = Color.White;
                    }
                    catch
                    {
                        var red = Color.Red;
                        cell.Style.BackColor = red;
                        vm.ValuesValid = false;
                    }
                }
            }
            vm.MatricaVjeojatnost = value;
            return vm;
        }
        public static IEnumerable<double> Add(this IEnumerable<double> v1, List<double> v2)
        {
            var i = 0;
            foreach(var d in v1)
            {
                yield return d + v2[i];
                i++;
            }
        }
        public static string Str(this IEnumerable<double> v1)
        {
            return "{" + v1.Select(i => i.ToString()).Aggregate((i, j) => i + ", " + j) + "}";
        }
        public static double Entropija(this IEnumerable<double> v1) => v1.Select(i => -1 * i * Log2(i)).Sum();
        public static double Entropija(this IEnumerable<IEnumerable<double>> v1) => v1.SelectMany(i => i).Select(i => -1 * i * Log2(i)).Sum();
        private static double Log2(double input)
        {
            if (input == 0.0)
            {
                return 0.0;
            }
            return Math.Log(input, 2.0);
        }
    }
}
