using System.Windows.Forms;

namespace Tinf.Lab
{
    public static class Program
    {
        public static void Main(string[] arguments)
        {
            uint previousHeight = 1, previousWidth = 1;

            // Enable visual styles (commctrl6)
            Application.EnableVisualStyles();

            // Main loop
            while (true)
            {
                // Show matrix dimensions form
                var matrixDimensionsForm = new MatrixDimensionsForm(previousHeight, previousWidth);
                if (matrixDimensionsForm.ShowDialog() == DialogResult.Cancel)
                {
                    break;
                }

                // Rembember previous matrix dimensions
                previousHeight = matrixDimensionsForm.MatrixHeight;
                previousWidth = matrixDimensionsForm.MatrixWidth;

                // Show matrix calculations form
                var matrixCalculationsForm = new MatrixCalculationsForm(previousHeight, previousWidth);
                if (matrixCalculationsForm.ShowDialog() == DialogResult.Cancel)
                {
                    break;
                }
            }
        }
    }
}
