using System;
using System.Windows.Forms;

namespace serpent_experiment
{
    public partial class SettingsForm : Form
    {
        public string SelectedTheme { get; private set; }
        public int SelectedGridSize { get; private set; }
        public int SelectedTickSpeed { get; private set; }
                
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lightModeRadioButton = new System.Windows.Forms.RadioButton();
            this.darkModeRadioButton = new System.Windows.Forms.RadioButton();
            this.okButton = new System.Windows.Forms.Button();
            this.gridSizeComboBox = new System.Windows.Forms.ComboBox();
            this.tickSpeedTextBox = new System.Windows.Forms.TextBox();
            this.appearanceSettingsLabel = new System.Windows.Forms.Label();
            this.gameSettingsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lightModeRadioButton
            // 
            this.lightModeRadioButton.AutoSize = true;
            this.lightModeRadioButton.Checked = true;
            this.lightModeRadioButton.Location = new System.Drawing.Point(12, 32);
            this.lightModeRadioButton.Name = "lightModeRadioButton";
            this.lightModeRadioButton.Size = new System.Drawing.Size(94, 24);
            this.lightModeRadioButton.TabIndex = 0;
            this.lightModeRadioButton.TabStop = true;
            this.lightModeRadioButton.Text = "Light Mode";
            this.lightModeRadioButton.UseVisualStyleBackColor = true;
            // 
            // darkModeRadioButton
            // 
            this.darkModeRadioButton.AutoSize = true;
            this.darkModeRadioButton.Location = new System.Drawing.Point(12, 62);
            this.darkModeRadioButton.Name = "darkModeRadioButton";
            this.darkModeRadioButton.Size = new System.Drawing.Size(97, 24);
            this.darkModeRadioButton.TabIndex = 1;
            this.darkModeRadioButton.Text = "Dark Mode";
            this.darkModeRadioButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(12, 180);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(94, 29);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // gridSizeComboBox
            // 
            this.gridSizeComboBox.FormattingEnabled = true;
            this.gridSizeComboBox.Items.AddRange(new object[] {
        "8x8",
        "16x16",
        "32x32"});
            this.gridSizeComboBox.Location = new System.Drawing.Point(12, 122);
            this.gridSizeComboBox.Name = "gridSizeComboBox";
            this.gridSizeComboBox.Size = new System.Drawing.Size(94, 28);
            this.gridSizeComboBox.TabIndex = 3;
            this.gridSizeComboBox.SelectedIndex = 0;
            // 
            // tickSpeedTextBox
            // 
            this.tickSpeedTextBox.Location = new System.Drawing.Point(12, 156);
            this.tickSpeedTextBox.Name = "tickSpeedTextBox";
            this.tickSpeedTextBox.Size = new System.Drawing.Size(94, 27);
            this.tickSpeedTextBox.TabIndex = 4;
            this.tickSpeedTextBox.Text = "100";
            // 
            // appearanceSettingsLabel
            // 
            this.appearanceSettingsLabel.AutoSize = true;
            this.appearanceSettingsLabel.Location = new System.Drawing.Point(12, 9);
            this.appearanceSettingsLabel.Name = "appearanceSettingsLabel";
            this.appearanceSettingsLabel.Size = new System.Drawing.Size(147, 20);
            this.appearanceSettingsLabel.TabIndex = 5;
            this.appearanceSettingsLabel.Text = "Appearance Settings";
            // 
            // gameSettingsLabel
            // 
            this.gameSettingsLabel.AutoSize = true;
            this.gameSettingsLabel.Location = new System.Drawing.Point(12, 99);
            this.gameSettingsLabel.Name = "gameSettingsLabel";
            this.gameSettingsLabel.Size = new System.Drawing.Size(105, 20);
            this.gameSettingsLabel.TabIndex = 6;
            this.gameSettingsLabel.Text = "Game Settings";
            // 
            // SettingsForm
            // 
            this.ClientSize = new System.Drawing.Size(200, 221);
            this.Controls.Add(this.gameSettingsLabel);
            this.Controls.Add(this.appearanceSettingsLabel);
            this.Controls.Add(this.tickSpeedTextBox);
            this.Controls.Add(this.gridSizeComboBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.darkModeRadioButton);
            this.Controls.Add(this.lightModeRadioButton);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (lightModeRadioButton.Checked)
            {
                SelectedTheme = "Light";
            }
            else if (darkModeRadioButton.Checked)
            {
                SelectedTheme = "Dark";
            }

            // Parse grid size
            string gridSizeText = gridSizeComboBox.SelectedItem.ToString();
            SelectedGridSize = int.Parse(gridSizeText.Split('x')[0]);

            // Parse tick speed
            SelectedTickSpeed = int.Parse(tickSpeedTextBox.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private RadioButton lightModeRadioButton;
        private RadioButton darkModeRadioButton;
        private Button okButton;
        private ComboBox gridSizeComboBox;
        private TextBox tickSpeedTextBox;
        private Label appearanceSettingsLabel;
        private Label gameSettingsLabel;
    }
}