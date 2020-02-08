namespace TrackerUI
{
    partial class CreateTeamForm
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
            this.createTeamValueTextBox = new System.Windows.Forms.TextBox();
            this.teamNameLabel = new System.Windows.Forms.Label();
            this.CreateTeamLabel = new System.Windows.Forms.Label();
            this.selectTeamLabel = new System.Windows.Forms.Label();
            this.selectTeamMemberDropDown = new System.Windows.Forms.ComboBox();
            this.addTeamMemberButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.phoneTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.createMemberButton = new System.Windows.Forms.Button();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.lastNameTextBox = new System.Windows.Forms.TextBox();
            this.lastNameLabel = new System.Windows.Forms.Label();
            this.firstNameTextBox = new System.Windows.Forms.TextBox();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.teamMembersListBox = new System.Windows.Forms.ListBox();
            this.deleteSelectedPlayerButton = new System.Windows.Forms.Button();
            this.createTeamButton = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // createTeamValueTextBox
            // 
            this.createTeamValueTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.createTeamValueTextBox.Location = new System.Drawing.Point(41, 128);
            this.createTeamValueTextBox.Name = "createTeamValueTextBox";
            this.createTeamValueTextBox.Size = new System.Drawing.Size(336, 35);
            this.createTeamValueTextBox.TabIndex = 12;
            this.createTeamValueTextBox.TextChanged += new System.EventHandler(this.createTeamValueTextBox_TextChanged);
            // 
            // teamNameLabel
            // 
            this.teamNameLabel.AutoSize = true;
            this.teamNameLabel.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.teamNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))),
                ((int) (((byte) (153)))), ((int) (((byte) (255)))));
            this.teamNameLabel.Location = new System.Drawing.Point(36, 95);
            this.teamNameLabel.Name = "teamNameLabel";
            this.teamNameLabel.Size = new System.Drawing.Size(123, 30);
            this.teamNameLabel.TabIndex = 11;
            this.teamNameLabel.Text = "Team Name";
            // 
            // CreateTeamLabel
            // 
            this.CreateTeamLabel.AutoSize = true;
            this.CreateTeamLabel.Font = new System.Drawing.Font("Segoe UI Light", 27.75F,
                System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.CreateTeamLabel.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))),
                ((int) (((byte) (153)))), ((int) (((byte) (255)))));
            this.CreateTeamLabel.Location = new System.Drawing.Point(32, 25);
            this.CreateTeamLabel.Name = "CreateTeamLabel";
            this.CreateTeamLabel.Size = new System.Drawing.Size(213, 50);
            this.CreateTeamLabel.TabIndex = 10;
            this.CreateTeamLabel.Text = "Create Team";
            // 
            // selectTeamLabel
            // 
            this.selectTeamLabel.AutoSize = true;
            this.selectTeamLabel.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.selectTeamLabel.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))),
                ((int) (((byte) (153)))), ((int) (((byte) (255)))));
            this.selectTeamLabel.Location = new System.Drawing.Point(36, 183);
            this.selectTeamLabel.Name = "selectTeamLabel";
            this.selectTeamLabel.Size = new System.Drawing.Size(205, 30);
            this.selectTeamLabel.TabIndex = 11;
            this.selectTeamLabel.Text = "Select Team Member";
            // 
            // selectTeamMemberDropDown
            // 
            this.selectTeamMemberDropDown.FormattingEnabled = true;
            this.selectTeamMemberDropDown.Location = new System.Drawing.Point(41, 216);
            this.selectTeamMemberDropDown.Name = "selectTeamMemberDropDown";
            this.selectTeamMemberDropDown.Size = new System.Drawing.Size(336, 38);
            this.selectTeamMemberDropDown.TabIndex = 13;
            // 
            // addTeamMemberButton
            // 
            this.addTeamMemberButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addTeamMemberButton.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))),
                ((int) (((byte) (153)))), ((int) (((byte) (255)))));
            this.addTeamMemberButton.Location = new System.Drawing.Point(117, 260);
            this.addTeamMemberButton.Name = "addTeamMemberButton";
            this.addTeamMemberButton.Size = new System.Drawing.Size(188, 39);
            this.addTeamMemberButton.TabIndex = 14;
            this.addTeamMemberButton.Text = "Add Member";
            this.addTeamMemberButton.UseVisualStyleBackColor = true;
            this.addTeamMemberButton.Click += new System.EventHandler(this.addTeamMemberButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.phoneTextBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.createMemberButton);
            this.groupBox2.Controls.Add(this.emailTextBox);
            this.groupBox2.Controls.Add(this.emailLabel);
            this.groupBox2.Controls.Add(this.lastNameTextBox);
            this.groupBox2.Controls.Add(this.lastNameLabel);
            this.groupBox2.Controls.Add(this.firstNameTextBox);
            this.groupBox2.Controls.Add(this.firstNameLabel);
            this.groupBox2.Location = new System.Drawing.Point(41, 331);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(352, 254);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add New Member";
            // 
            // phoneTextBox
            // 
            this.phoneTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.phoneTextBox.Location = new System.Drawing.Point(143, 152);
            this.phoneTextBox.Name = "phoneTextBox";
            this.phoneTextBox.Size = new System.Drawing.Size(193, 35);
            this.phoneTextBox.TabIndex = 17;
            this.phoneTextBox.TextChanged += new System.EventHandler(this.phoneTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))), ((int) (((byte) (153)))),
                ((int) (((byte) (255)))));
            this.label2.Location = new System.Drawing.Point(6, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 30);
            this.label2.TabIndex = 17;
            this.label2.Text = "Phone";
            // 
            // createMemberButton
            // 
            this.createMemberButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createMemberButton.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))),
                ((int) (((byte) (153)))), ((int) (((byte) (255)))));
            this.createMemberButton.Location = new System.Drawing.Point(67, 209);
            this.createMemberButton.Name = "createMemberButton";
            this.createMemberButton.Size = new System.Drawing.Size(188, 39);
            this.createMemberButton.TabIndex = 14;
            this.createMemberButton.Text = "Create Member";
            this.createMemberButton.UseVisualStyleBackColor = true;
            this.createMemberButton.Click += new System.EventHandler(this.createMemberButton_Click);
            // 
            // emailTextBox
            // 
            this.emailTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.emailTextBox.Location = new System.Drawing.Point(143, 111);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(193, 35);
            this.emailTextBox.TabIndex = 17;
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.emailLabel.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))), ((int) (((byte) (153)))),
                ((int) (((byte) (255)))));
            this.emailLabel.Location = new System.Drawing.Point(6, 113);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(137, 30);
            this.emailLabel.TabIndex = 17;
            this.emailLabel.Text = "EmailAddress";
            // 
            // lastNameTextBox
            // 
            this.lastNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lastNameTextBox.Location = new System.Drawing.Point(143, 70);
            this.lastNameTextBox.Name = "lastNameTextBox";
            this.lastNameTextBox.Size = new System.Drawing.Size(193, 35);
            this.lastNameTextBox.TabIndex = 17;
            // 
            // lastNameLabel
            // 
            this.lastNameLabel.AutoSize = true;
            this.lastNameLabel.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lastNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))),
                ((int) (((byte) (153)))), ((int) (((byte) (255)))));
            this.lastNameLabel.Location = new System.Drawing.Point(6, 72);
            this.lastNameLabel.Name = "lastNameLabel";
            this.lastNameLabel.Size = new System.Drawing.Size(112, 30);
            this.lastNameLabel.TabIndex = 17;
            this.lastNameLabel.Text = "Last Name";
            // 
            // firstNameTextBox
            // 
            this.firstNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.firstNameTextBox.Location = new System.Drawing.Point(143, 29);
            this.firstNameTextBox.Name = "firstNameTextBox";
            this.firstNameTextBox.Size = new System.Drawing.Size(193, 35);
            this.firstNameTextBox.TabIndex = 17;
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.Font = new System.Drawing.Font("Segoe UI Light", 16F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.firstNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))),
                ((int) (((byte) (153)))), ((int) (((byte) (255)))));
            this.firstNameLabel.Location = new System.Drawing.Point(6, 31);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(113, 30);
            this.firstNameLabel.TabIndex = 17;
            this.firstNameLabel.Text = "First Name";
            // 
            // teamMembersListBox
            // 
            this.teamMembersListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.teamMembersListBox.FormattingEnabled = true;
            this.teamMembersListBox.ItemHeight = 30;
            this.teamMembersListBox.Location = new System.Drawing.Point(426, 128);
            this.teamMembersListBox.Name = "teamMembersListBox";
            this.teamMembersListBox.Size = new System.Drawing.Size(315, 452);
            this.teamMembersListBox.TabIndex = 17;
            this.teamMembersListBox.SelectedIndexChanged +=
                new System.EventHandler(this.teamMembersListBox_SelectedIndexChanged);
            // 
            // deleteSelectedPlayerButton
            // 
            this.deleteSelectedPlayerButton.Enabled = false;
            this.deleteSelectedPlayerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteSelectedPlayerButton.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))),
                ((int) (((byte) (153)))), ((int) (((byte) (255)))));
            this.deleteSelectedPlayerButton.Location = new System.Drawing.Point(426, 83);
            this.deleteSelectedPlayerButton.Name = "deleteSelectedPlayerButton";
            this.deleteSelectedPlayerButton.Size = new System.Drawing.Size(117, 39);
            this.deleteSelectedPlayerButton.TabIndex = 18;
            this.deleteSelectedPlayerButton.Text = "Delete";
            this.deleteSelectedPlayerButton.UseVisualStyleBackColor = true;
            this.deleteSelectedPlayerButton.Click += new System.EventHandler(this.deleteSelectedPlayerButton_Click);
            // 
            // createTeamButton
            // 
            this.createTeamButton.Enabled = false;
            this.createTeamButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createTeamButton.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (51)))),
                ((int) (((byte) (153)))), ((int) (((byte) (255)))));
            this.createTeamButton.Location = new System.Drawing.Point(251, 610);
            this.createTeamButton.Name = "createTeamButton";
            this.createTeamButton.Size = new System.Drawing.Size(278, 72);
            this.createTeamButton.TabIndex = 19;
            this.createTeamButton.Text = "Create Team";
            this.createTeamButton.UseVisualStyleBackColor = true;
            this.createTeamButton.Click += new System.EventHandler(this.createTeamButton_Click);
            // 
            // CreateTeamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(785, 738);
            this.Controls.Add(this.createTeamButton);
            this.Controls.Add(this.deleteSelectedPlayerButton);
            this.Controls.Add(this.teamMembersListBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.addTeamMemberButton);
            this.Controls.Add(this.selectTeamMemberDropDown);
            this.Controls.Add(this.selectTeamLabel);
            this.Controls.Add(this.createTeamValueTextBox);
            this.Controls.Add(this.teamNameLabel);
            this.Controls.Add(this.CreateTeamLabel);
            this.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "CreateTeamForm";
            this.Text = "CreateTeamForm";
            this.Load += new System.EventHandler(this.CreateTeamForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox createTeamValueTextBox;
        private System.Windows.Forms.Label teamNameLabel;
        private System.Windows.Forms.Label CreateTeamLabel;
        private System.Windows.Forms.Label selectTeamLabel;
        private System.Windows.Forms.ComboBox selectTeamMemberDropDown;
        private System.Windows.Forms.Button addTeamMemberButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox phoneTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button createMemberButton;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox lastNameTextBox;
        private System.Windows.Forms.Label lastNameLabel;
        private System.Windows.Forms.TextBox firstNameTextBox;
        private System.Windows.Forms.Label firstNameLabel;
        private System.Windows.Forms.ListBox teamMembersListBox;
        private System.Windows.Forms.Button deleteSelectedPlayerButton;
        private System.Windows.Forms.Button createTeamButton;
    }
}