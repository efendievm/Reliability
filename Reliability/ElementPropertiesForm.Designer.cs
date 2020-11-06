namespace Reliability
{
    partial class ElementPropertiesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.NameLabel = new System.Windows.Forms.Label();
            this.MajorityLabel = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.MajorityTextBox = new System.Windows.Forms.TextBox();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.NamePanel = new System.Windows.Forms.Panel();
            this.GroupOfElementsPanel = new System.Windows.Forms.Panel();
            this.ChildElementPanel = new System.Windows.Forms.Panel();
            this.CyclogrammPanel = new System.Windows.Forms.Panel();
            this.GridPanel = new System.Windows.Forms.Panel();
            this.CyclogramGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModifyRowCountButtonsPanel = new System.Windows.Forms.Panel();
            this.ModifyRowCountButtonsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.RemoveAllRowsButton = new System.Windows.Forms.Button();
            this.RemoveRowButton = new System.Windows.Forms.Button();
            this.AddRowButton = new System.Windows.Forms.Button();
            this.WorkTypePanel = new System.Windows.Forms.Panel();
            this.CycleDurationLabel = new System.Windows.Forms.Label();
            this.CycleDurationTextBox = new System.Windows.Forms.TextBox();
            this.DurabilityTextBox = new System.Windows.Forms.TextBox();
            this.DurabilityRadioButton = new System.Windows.Forms.RadioButton();
            this.CyclogrammLabel = new System.Windows.Forms.Label();
            this.SuccessPossibilityTextBox = new System.Windows.Forms.TextBox();
            this.WorkTypeLabel = new System.Windows.Forms.Label();
            this.SuccessProbabilityRadioButton = new System.Windows.Forms.RadioButton();
            this.NamePanel.SuspendLayout();
            this.GroupOfElementsPanel.SuspendLayout();
            this.ChildElementPanel.SuspendLayout();
            this.CyclogrammPanel.SuspendLayout();
            this.GridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CyclogramGrid)).BeginInit();
            this.ModifyRowCountButtonsPanel.SuspendLayout();
            this.ModifyRowCountButtonsTableLayoutPanel.SuspendLayout();
            this.WorkTypePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NameLabel.Location = new System.Drawing.Point(3, 11);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(199, 25);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Название элемента";
            // 
            // MajorityLabel
            // 
            this.MajorityLabel.AutoSize = true;
            this.MajorityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MajorityLabel.Location = new System.Drawing.Point(3, 9);
            this.MajorityLabel.Name = "MajorityLabel";
            this.MajorityLabel.Size = new System.Drawing.Size(270, 25);
            this.MajorityLabel.TabIndex = 1;
            this.MajorityLabel.Text = "Степень мажаритирования";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(219, 11);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(281, 26);
            this.NameTextBox.TabIndex = 2;
            // 
            // MajorityTextBox
            // 
            this.MajorityTextBox.Location = new System.Drawing.Point(402, 10);
            this.MajorityTextBox.Name = "MajorityTextBox";
            this.MajorityTextBox.Size = new System.Drawing.Size(100, 26);
            this.MajorityTextBox.TabIndex = 3;
            // 
            // ApplyButton
            // 
            this.ApplyButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ApplyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ApplyButton.Location = new System.Drawing.Point(0, 603);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(531, 40);
            this.ApplyButton.TabIndex = 4;
            this.ApplyButton.Text = "Принять";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // NamePanel
            // 
            this.NamePanel.Controls.Add(this.NameLabel);
            this.NamePanel.Controls.Add(this.NameTextBox);
            this.NamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NamePanel.Location = new System.Drawing.Point(0, 0);
            this.NamePanel.Name = "NamePanel";
            this.NamePanel.Size = new System.Drawing.Size(531, 45);
            this.NamePanel.TabIndex = 5;
            // 
            // GroupOfElementsPanel
            // 
            this.GroupOfElementsPanel.Controls.Add(this.MajorityLabel);
            this.GroupOfElementsPanel.Controls.Add(this.MajorityTextBox);
            this.GroupOfElementsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.GroupOfElementsPanel.Location = new System.Drawing.Point(0, 45);
            this.GroupOfElementsPanel.Name = "GroupOfElementsPanel";
            this.GroupOfElementsPanel.Size = new System.Drawing.Size(531, 45);
            this.GroupOfElementsPanel.TabIndex = 6;
            // 
            // ChildElementPanel
            // 
            this.ChildElementPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ChildElementPanel.Controls.Add(this.CyclogrammPanel);
            this.ChildElementPanel.Controls.Add(this.WorkTypePanel);
            this.ChildElementPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChildElementPanel.Location = new System.Drawing.Point(0, 90);
            this.ChildElementPanel.Name = "ChildElementPanel";
            this.ChildElementPanel.Size = new System.Drawing.Size(531, 513);
            this.ChildElementPanel.TabIndex = 7;
            // 
            // CyclogrammPanel
            // 
            this.CyclogrammPanel.Controls.Add(this.GridPanel);
            this.CyclogrammPanel.Controls.Add(this.ModifyRowCountButtonsPanel);
            this.CyclogrammPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CyclogrammPanel.Location = new System.Drawing.Point(0, 205);
            this.CyclogrammPanel.Name = "CyclogrammPanel";
            this.CyclogrammPanel.Size = new System.Drawing.Size(531, 308);
            this.CyclogrammPanel.TabIndex = 8;
            // 
            // GridPanel
            // 
            this.GridPanel.Controls.Add(this.CyclogramGrid);
            this.GridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridPanel.Location = new System.Drawing.Point(0, 0);
            this.GridPanel.Name = "GridPanel";
            this.GridPanel.Size = new System.Drawing.Size(531, 262);
            this.GridPanel.TabIndex = 12;
            // 
            // CyclogramGrid
            // 
            this.CyclogramGrid.AllowUserToAddRows = false;
            this.CyclogramGrid.AllowUserToDeleteRows = false;
            this.CyclogramGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CyclogramGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.CyclogramGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CyclogramGrid.Location = new System.Drawing.Point(0, 0);
            this.CyclogramGrid.Name = "CyclogramGrid";
            this.CyclogramGrid.RowHeadersVisible = false;
            this.CyclogramGrid.RowTemplate.Height = 28;
            this.CyclogramGrid.Size = new System.Drawing.Size(531, 262);
            this.CyclogramGrid.TabIndex = 9;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column1.HeaderText = "Время, ч";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column2.HeaderText = "Работает: Да(1)/Нет(0)";
            this.Column2.Name = "Column2";
            // 
            // ModifyRowCountButtonsPanel
            // 
            this.ModifyRowCountButtonsPanel.Controls.Add(this.ModifyRowCountButtonsTableLayoutPanel);
            this.ModifyRowCountButtonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ModifyRowCountButtonsPanel.Location = new System.Drawing.Point(0, 262);
            this.ModifyRowCountButtonsPanel.Name = "ModifyRowCountButtonsPanel";
            this.ModifyRowCountButtonsPanel.Size = new System.Drawing.Size(531, 46);
            this.ModifyRowCountButtonsPanel.TabIndex = 11;
            // 
            // ModifyRowCountButtonsTableLayoutPanel
            // 
            this.ModifyRowCountButtonsTableLayoutPanel.ColumnCount = 3;
            this.ModifyRowCountButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.ModifyRowCountButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ModifyRowCountButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.ModifyRowCountButtonsTableLayoutPanel.Controls.Add(this.RemoveAllRowsButton, 2, 0);
            this.ModifyRowCountButtonsTableLayoutPanel.Controls.Add(this.RemoveRowButton, 1, 0);
            this.ModifyRowCountButtonsTableLayoutPanel.Controls.Add(this.AddRowButton, 0, 0);
            this.ModifyRowCountButtonsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ModifyRowCountButtonsTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ModifyRowCountButtonsTableLayoutPanel.Name = "ModifyRowCountButtonsTableLayoutPanel";
            this.ModifyRowCountButtonsTableLayoutPanel.RowCount = 1;
            this.ModifyRowCountButtonsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ModifyRowCountButtonsTableLayoutPanel.Size = new System.Drawing.Size(531, 46);
            this.ModifyRowCountButtonsTableLayoutPanel.TabIndex = 10;
            // 
            // RemoveAllRowsButton
            // 
            this.RemoveAllRowsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveAllRowsButton.Location = new System.Drawing.Point(356, 3);
            this.RemoveAllRowsButton.Name = "RemoveAllRowsButton";
            this.RemoveAllRowsButton.Size = new System.Drawing.Size(172, 40);
            this.RemoveAllRowsButton.TabIndex = 2;
            this.RemoveAllRowsButton.Text = "Удалить все строки";
            this.RemoveAllRowsButton.UseVisualStyleBackColor = true;
            this.RemoveAllRowsButton.Click += new System.EventHandler(this.ModifyRowCount_Click);
            // 
            // RemoveRowButton
            // 
            this.RemoveRowButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemoveRowButton.Location = new System.Drawing.Point(180, 3);
            this.RemoveRowButton.Name = "RemoveRowButton";
            this.RemoveRowButton.Size = new System.Drawing.Size(170, 40);
            this.RemoveRowButton.TabIndex = 1;
            this.RemoveRowButton.Text = "Удалить строку";
            this.RemoveRowButton.UseVisualStyleBackColor = true;
            this.RemoveRowButton.Click += new System.EventHandler(this.ModifyRowCount_Click);
            // 
            // AddRowButton
            // 
            this.AddRowButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddRowButton.Location = new System.Drawing.Point(3, 3);
            this.AddRowButton.Name = "AddRowButton";
            this.AddRowButton.Size = new System.Drawing.Size(171, 40);
            this.AddRowButton.TabIndex = 0;
            this.AddRowButton.Text = "Добавить строку";
            this.AddRowButton.UseVisualStyleBackColor = true;
            this.AddRowButton.Click += new System.EventHandler(this.ModifyRowCount_Click);
            // 
            // WorkTypePanel
            // 
            this.WorkTypePanel.Controls.Add(this.CycleDurationLabel);
            this.WorkTypePanel.Controls.Add(this.CycleDurationTextBox);
            this.WorkTypePanel.Controls.Add(this.DurabilityTextBox);
            this.WorkTypePanel.Controls.Add(this.DurabilityRadioButton);
            this.WorkTypePanel.Controls.Add(this.CyclogrammLabel);
            this.WorkTypePanel.Controls.Add(this.SuccessPossibilityTextBox);
            this.WorkTypePanel.Controls.Add(this.WorkTypeLabel);
            this.WorkTypePanel.Controls.Add(this.SuccessProbabilityRadioButton);
            this.WorkTypePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.WorkTypePanel.Location = new System.Drawing.Point(0, 0);
            this.WorkTypePanel.Name = "WorkTypePanel";
            this.WorkTypePanel.Size = new System.Drawing.Size(531, 205);
            this.WorkTypePanel.TabIndex = 7;
            // 
            // CycleDurationLabel
            // 
            this.CycleDurationLabel.AutoSize = true;
            this.CycleDurationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CycleDurationLabel.Location = new System.Drawing.Point(5, 136);
            this.CycleDurationLabel.Name = "CycleDurationLabel";
            this.CycleDurationLabel.Size = new System.Drawing.Size(232, 25);
            this.CycleDurationLabel.TabIndex = 9;
            this.CycleDurationLabel.Text = "Длительность цикла, ч";
            // 
            // CycleDurationTextBox
            // 
            this.CycleDurationTextBox.Location = new System.Drawing.Point(404, 137);
            this.CycleDurationTextBox.Name = "CycleDurationTextBox";
            this.CycleDurationTextBox.Size = new System.Drawing.Size(100, 26);
            this.CycleDurationTextBox.TabIndex = 10;
            // 
            // DurabilityTextBox
            // 
            this.DurabilityTextBox.Enabled = false;
            this.DurabilityTextBox.Location = new System.Drawing.Point(404, 95);
            this.DurabilityTextBox.Name = "DurabilityTextBox";
            this.DurabilityTextBox.Size = new System.Drawing.Size(100, 26);
            this.DurabilityTextBox.TabIndex = 8;
            // 
            // DurabilityRadioButton
            // 
            this.DurabilityRadioButton.AutoSize = true;
            this.DurabilityRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DurabilityRadioButton.Location = new System.Drawing.Point(12, 92);
            this.DurabilityRadioButton.Name = "DurabilityRadioButton";
            this.DurabilityRadioButton.Size = new System.Drawing.Size(299, 29);
            this.DurabilityRadioButton.TabIndex = 7;
            this.DurabilityRadioButton.Text = "Интенсивность отказов, 1/ч";
            this.DurabilityRadioButton.UseVisualStyleBackColor = true;
            this.DurabilityRadioButton.CheckedChanged += new System.EventHandler(this.ElementTypeRadioButton_CheckedChanged);
            // 
            // CyclogrammLabel
            // 
            this.CyclogrammLabel.AutoSize = true;
            this.CyclogrammLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CyclogrammLabel.Location = new System.Drawing.Point(6, 169);
            this.CyclogrammLabel.Name = "CyclogrammLabel";
            this.CyclogrammLabel.Size = new System.Drawing.Size(217, 25);
            this.CyclogrammLabel.TabIndex = 6;
            this.CyclogrammLabel.Text = "Циклограмма работы";
            // 
            // SuccessPossibilityTextBox
            // 
            this.SuccessPossibilityTextBox.Location = new System.Drawing.Point(404, 54);
            this.SuccessPossibilityTextBox.Name = "SuccessPossibilityTextBox";
            this.SuccessPossibilityTextBox.Size = new System.Drawing.Size(100, 26);
            this.SuccessPossibilityTextBox.TabIndex = 5;
            // 
            // WorkTypeLabel
            // 
            this.WorkTypeLabel.AutoSize = true;
            this.WorkTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.WorkTypeLabel.Location = new System.Drawing.Point(5, 14);
            this.WorkTypeLabel.Name = "WorkTypeLabel";
            this.WorkTypeLabel.Size = new System.Drawing.Size(175, 25);
            this.WorkTypeLabel.TabIndex = 1;
            this.WorkTypeLabel.Text = "Характер работы";
            // 
            // SuccessProbabilityRadioButton
            // 
            this.SuccessProbabilityRadioButton.AutoSize = true;
            this.SuccessProbabilityRadioButton.Checked = true;
            this.SuccessProbabilityRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SuccessProbabilityRadioButton.Location = new System.Drawing.Point(12, 51);
            this.SuccessProbabilityRadioButton.Name = "SuccessProbabilityRadioButton";
            this.SuccessProbabilityRadioButton.Size = new System.Drawing.Size(358, 29);
            this.SuccessProbabilityRadioButton.TabIndex = 2;
            this.SuccessProbabilityRadioButton.TabStop = true;
            this.SuccessProbabilityRadioButton.Text = "Вероятность безотказной работы";
            this.SuccessProbabilityRadioButton.UseVisualStyleBackColor = true;
            this.SuccessProbabilityRadioButton.CheckedChanged += new System.EventHandler(this.ElementTypeRadioButton_CheckedChanged);
            // 
            // ElementPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(531, 643);
            this.Controls.Add(this.ChildElementPanel);
            this.Controls.Add(this.GroupOfElementsPanel);
            this.Controls.Add(this.NamePanel);
            this.Controls.Add(this.ApplyButton);
            this.Name = "ElementPropertiesForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Свойства элемента";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ElementPropertiesForm_FormClosing);
            this.NamePanel.ResumeLayout(false);
            this.NamePanel.PerformLayout();
            this.GroupOfElementsPanel.ResumeLayout(false);
            this.GroupOfElementsPanel.PerformLayout();
            this.ChildElementPanel.ResumeLayout(false);
            this.CyclogrammPanel.ResumeLayout(false);
            this.GridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CyclogramGrid)).EndInit();
            this.ModifyRowCountButtonsPanel.ResumeLayout(false);
            this.ModifyRowCountButtonsTableLayoutPanel.ResumeLayout(false);
            this.WorkTypePanel.ResumeLayout(false);
            this.WorkTypePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label MajorityLabel;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.TextBox MajorityTextBox;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Panel NamePanel;
        private System.Windows.Forms.Panel GroupOfElementsPanel;
        private System.Windows.Forms.Panel ChildElementPanel;
        private System.Windows.Forms.TextBox SuccessPossibilityTextBox;
        private System.Windows.Forms.RadioButton SuccessProbabilityRadioButton;
        private System.Windows.Forms.Label WorkTypeLabel;
        private System.Windows.Forms.Panel CyclogrammPanel;
        private System.Windows.Forms.Panel WorkTypePanel;
        private System.Windows.Forms.Label CyclogrammLabel;
        private System.Windows.Forms.TextBox DurabilityTextBox;
        private System.Windows.Forms.RadioButton DurabilityRadioButton;
        private System.Windows.Forms.Panel GridPanel;
        private System.Windows.Forms.DataGridView CyclogramGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Panel ModifyRowCountButtonsPanel;
        private System.Windows.Forms.TableLayoutPanel ModifyRowCountButtonsTableLayoutPanel;
        private System.Windows.Forms.Button RemoveAllRowsButton;
        private System.Windows.Forms.Button RemoveRowButton;
        private System.Windows.Forms.Button AddRowButton;
        private System.Windows.Forms.Label CycleDurationLabel;
        private System.Windows.Forms.TextBox CycleDurationTextBox;
    }
}