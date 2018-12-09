using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Tinf.Lab
{
	public class MatrixDimensionsForm : Form
    {
		public uint MatrixHeight { get => _matrixHeight; set => _heightTextBox.Text = (_matrixHeight = value).ToString(); }
		public uint MatrixWidth { get => _matrixWidth; set => _widthTextBox.Text = (_matrixWidth = value).ToString(); }

		private uint _matrixHeight, _matrixWidth;
		private Label _heightLabel, _widthLabel;
		private TextBox _heightTextBox, _widthTextBox;
		private Button _okButton, _cancelButton;

		public MatrixDimensionsForm(uint defaultHeight = 1, uint defaultWidth = 1)
		{
			// Initialize position and style
			StartPosition = FormStartPosition.CenterScreen;
			FormBorderStyle = FormBorderStyle.FixedSingle;
   			MaximizeBox = false;
			ShowIcon = false;

			// Initialize dimensions
			Width = 250 + (Width - ClientSize.Width);
			Height = 136 + (Height - ClientSize.Height);
			MinimumSize = MaximumSize = new Size(Width, Height);

			// Initialize title
			Text = "Unesite dimenzije matrice";

			// Initialize values
			_matrixHeight = defaultHeight;
			_matrixWidth = defaultWidth;
			if ((_matrixHeight <= 0) || (_matrixWidth <= 0))
            {
                throw new Exception();
            }
            
            // Initialize height label
			_heightLabel = new Label();
			_heightLabel.Text = "m = ";
			_heightLabel.Location = new Point(8, 8);
			_heightLabel.Size = new Size(32, 24);
			_heightLabel.TextAlign = ContentAlignment.MiddleCenter;
			Controls.Add(_heightLabel);
            
            // Initialize width label
			_widthLabel = new Label();
			_widthLabel.Text = "n = ";
			_widthLabel.Location = new Point(8, 40);
			_widthLabel.Size = new Size(32, 24);
			_widthLabel.TextAlign = ContentAlignment.MiddleCenter;
			Controls.Add(_widthLabel);

			// Initialize height text box
			_heightTextBox = new TextBox();
			_heightTextBox.Text = _matrixHeight.ToString();
			_heightTextBox.Location = new Point(48, 8);
			_heightTextBox.Size = new Size(194, 24);
			_heightTextBox.TextChanged += DimensionTextChanged;
			Controls.Add(_heightTextBox);

			// Initialize width text box
            _widthTextBox = new TextBox();
			_widthTextBox.Text = _matrixWidth.ToString();
			_widthTextBox.Location = new Point(48, 40);
			_widthTextBox.Size = new Size(194, 24);
			_widthTextBox.TextChanged += DimensionTextChanged;
			Controls.Add(_widthTextBox);

			// Initialize OK button
			_okButton = new Button();
			_okButton.Text = "OK";
			_okButton.Location = new Point(8, 72);
			_okButton.Size = new Size(234, 24);
			_okButton.Click += OKButtonClick;
			Controls.Add(_okButton);

			// Initialize cancel button
            _cancelButton = new Button();
			_cancelButton.Text = "Odustani";
			_cancelButton.Location = new Point(8, 104);
			_cancelButton.Size = new Size(234, 24);
			_cancelButton.Click += CancelButtonClick;
			Controls.Add(_cancelButton);
		}

		private void DimensionTextChanged(object sender, EventArgs e)
		{
			bool _heightValid, _widthValid;

            // Check if matrix height is valid
			try
			{
				_matrixHeight = uint.Parse(_heightTextBox.Text, CultureInfo.InvariantCulture);
                if (_matrixHeight <= 0)
				{
					throw new Exception();
				}
				_heightValid = true;
			}
            catch
			{
				_heightValid = false;
			}

            // Check if matrix width is valid
			try
            {
				_matrixWidth = uint.Parse(_widthTextBox.Text, CultureInfo.InvariantCulture);
				if (_matrixWidth <= 0)
                {
                    throw new Exception();
                }
				_widthValid = true;
            }
            catch
            {
				_widthValid = false;
            }

            // Set OK button enabled if both dimensions are valid
			_okButton.Enabled = (_heightValid) && (_widthValid);
		}

		private void OKButtonClick(object sender, EventArgs e)
        {
			// Confirms the form
			DialogResult = DialogResult.OK;
        }

		private void CancelButtonClick(object sender, EventArgs e)
        {
			// Cancels the form
			DialogResult = DialogResult.Cancel;
        }
	}
}
