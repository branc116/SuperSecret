using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Tinf.Lab;

namespace p02_02_z1
{
	public class MatrixCalculationsForm : Form
    {
		public const string ValueFormat = "0.000";
		public const double ApproxEqual = 1e-7;

		public uint MatrixHeight { get; set; }
		public uint MatrixWidth { get; set; }

		private Panel _bottomPanel, _buttonsPanel;
		private Button _calculateButton, _newMatrixButton;
		private Label _resultsLabel;
		private DataGridView _matrixDataGridView;
        
        public MatrixCalculationsForm(uint matrixHeight, uint matrixWidth)
        {
			// Initialize position and style
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
			ShowIcon = false;

            // Initialize title
            Text = "Unesite vrijednosti matrice";

			// Initialize values
			MatrixHeight = matrixHeight;
			MatrixWidth = matrixWidth;
			if ((MatrixHeight <= 0) || (MatrixWidth <= 0))
			{
				throw new Exception();
			}

            // Initialize bottom panel
            _bottomPanel = new Panel
            {
                Height = 200,
                Dock = DockStyle.Bottom
            };
            Controls.Add(_bottomPanel);

            // Initialize buttons panel
            _buttonsPanel = new Panel
            {
                Padding = new Padding(8, 8, 8, 8),
                Height = 48,
                Dock = DockStyle.Bottom
            };
            _bottomPanel.Controls.Add(_buttonsPanel);

            // Initialize calculate button
            _calculateButton = new Button
            {
                Size = new Size(100, 32),
                Dock = DockStyle.Left,
                Text = "Izračunaj"
            };
            _calculateButton.Click += CalculateButtonClick;
			_buttonsPanel.Controls.Add(_calculateButton);

            var noramlizeButton = new Button
            {
                Size = new Size(100, 32),
                Dock = DockStyle.Left,
                Text = "Normaliziraj"
            };
            noramlizeButton.Click += (send, e) =>
            {
                SuspendLayout();
                Controls.Remove(_matrixDataGridView);
                _matrixDataGridView = _matrixDataGridView.ToVjerojatnosnaMatematika()
                    .Normaliziraj()
                    .ToDataGidView();
                Controls.Add(_matrixDataGridView);
                this.OnSizeChanged(EventArgs.Empty);
                ResumeLayout();
            };
            _buttonsPanel.Controls.Add(noramlizeButton);
            var randomiziraj = new Button
            {
                Size = new Size(100, 32),
                Dock = DockStyle.Left,
                Text = "Randomiziraj"
            };
            randomiziraj.Click += (s, e) =>
            {
                SuspendLayout();
                Controls.Remove(_matrixDataGridView);
                _matrixDataGridView = _matrixDataGridView.ToVjerojatnosnaMatematika()
                    .Randomiziraj()
                    .ToDataGidView();
                Controls.Add(_matrixDataGridView);
                CalculateButtonClick(this, EventArgs.Empty);
                this.OnSizeChanged(EventArgs.Empty);
                ResumeLayout();
            };
            _buttonsPanel.Controls.Add(randomiziraj);
            // Initialize new matrix button
            _newMatrixButton = new Button
            {
                Size = new Size(100, 32),
                Dock = DockStyle.Right,
                Text = "Nova matrica"
            };
            _newMatrixButton.Click += NewMatrixButtonClick;
			_buttonsPanel.Controls.Add(_newMatrixButton);

            // Initialize results label
            _resultsLabel = new Label
            {
                Dock = DockStyle.Fill,
                Text = string.Empty,
                TextAlign = ContentAlignment.MiddleCenter
            };
            _bottomPanel.Controls.Add(_resultsLabel);
            
            _matrixDataGridView = Mappers
                .ToVjerojatnosnaMatematika(MatrixHeight, MatrixWidth)
                .ToDataGidView();
            Controls.Add(_matrixDataGridView);

            // Initialize dimensions
            Height = _matrixDataGridView.ColumnHeadersHeight + (int)MatrixHeight * _matrixDataGridView.Rows[0].Height + _bottomPanel.Height + (Height - ClientSize.Height);
			Width = Math.Max(400, _matrixDataGridView.RowHeadersWidth + (int)MatrixWidth * 80 + (Width - ClientSize.Width));
			MinimumSize = new Size(Width, Height);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

            // Update height for all rows
			foreach (DataGridViewRow row in _matrixDataGridView.Rows)
			{
				row.Height = (ClientSize.Height - _bottomPanel.Height - _matrixDataGridView.ColumnHeadersHeight) / (int)MatrixHeight;
            }
			_matrixDataGridView.Rows[0].Height += (ClientSize.Height - _bottomPanel.Height - _matrixDataGridView.ColumnHeadersHeight) % (int)MatrixHeight;
		}
        private void CalculateButtonClick(object sender, EventArgs e)
        {

            var vm = _matrixDataGridView.ToVjerojatnosnaMatematika();

            // Clear selection in matrix data grid view
			_matrixDataGridView.ClearSelection();

            // Invalidate matrix data grid view
			_matrixDataGridView.Invalidate();

            // Proceed with calculations if values are valid
			if (vm.ValuesValid)
			{
				// Clear results label
				_resultsLabel.ForeColor = Color.Black;
				_resultsLabel.Text = string.Empty;

                
				string pX_str = "[p(X)] = " + vm.VjerojatnostiX().Str(), pY_str = "[p(Y)] = " + vm.VjerojatnostiY().Str();

				// Display error if sums are not (approximately) equal to 1.0
				if (!vm.SumValid())
				{
                    _resultsLabel.ForeColor = Color.Red;
					_resultsLabel.Text = "Sume vrijednosti p(X) tj. p(Y) nisu jednake 1.0.";
					return;
				}

				_resultsLabel.Text += pX_str + Environment.NewLine + pY_str + Environment.NewLine;

				double HX = vm.VjerojatnostiX().Entropija(), HY = vm.VjerojatnostiY().Entropija(), HXY = vm.MatricaVjeojatnost.Entropija();
                

				// Display H(X), H(Y) and H(X, Y)
				_resultsLabel.Text += "H(X) = " + HX.ToString(ValueFormat) + Environment.NewLine + "H(Y) = " + HY.ToString(ValueFormat) + Environment.NewLine + "H(X, Y) = " + HXY.ToString(ValueFormat) + Environment.NewLine;

				// Calculate and display H(Y|X)
				_resultsLabel.Text += "H(Y|X) = " + (HXY - HX).ToString(ValueFormat) + Environment.NewLine;

				// Calculate and display H(X|Y)
                _resultsLabel.Text += "H(X|Y) = " + (HXY - HY).ToString(ValueFormat) + Environment.NewLine;

				// Calculate and display I(X;Y)
				_resultsLabel.Text += "I(X;Y) = " + (HX + HY - HXY).ToString(ValueFormat);
			}
			else
			{
				// Display error message in results label
				_resultsLabel.ForeColor = Color.Red;
				_resultsLabel.Text = "Neke od vrijednosti matrice nisu valjane.";
			}
        }

        private void NewMatrixButtonClick(object sender, EventArgs e)
        {
			// Close the form
			DialogResult = DialogResult.OK;
        }
	}
}
