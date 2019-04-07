namespace Projekt_zaliczeniowy
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.spawn_train = new System.Windows.Forms.Button();
            this.spawn_car = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.random_wave = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // spawn_train
            // 
            this.spawn_train.Location = new System.Drawing.Point(173, 457);
            this.spawn_train.Margin = new System.Windows.Forms.Padding(4);
            this.spawn_train.Name = "spawn_train";
            this.spawn_train.Size = new System.Drawing.Size(145, 64);
            this.spawn_train.TabIndex = 0;
            this.spawn_train.Text = "DODAJ POCIĄG";
            this.spawn_train.UseVisualStyleBackColor = true;
            this.spawn_train.Click += new System.EventHandler(this.spawn_train_Click);
            // 
            // spawn_car
            // 
            this.spawn_car.Location = new System.Drawing.Point(12, 457);
            this.spawn_car.Margin = new System.Windows.Forms.Padding(4);
            this.spawn_car.Name = "spawn_car";
            this.spawn_car.Size = new System.Drawing.Size(153, 64);
            this.spawn_car.TabIndex = 1;
            this.spawn_car.Text = "DODAJ AUTO";
            this.spawn_car.UseVisualStyleBackColor = true;
            this.spawn_car.Click += new System.EventHandler(this.spawn_car_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 529);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(153, 22);
            this.textBox1.TabIndex = 2;
            // 
            // random_wave
            // 
            this.random_wave.Location = new System.Drawing.Point(12, 457);
            this.random_wave.Margin = new System.Windows.Forms.Padding(4);
            this.random_wave.Name = "random_wave";
            this.random_wave.Size = new System.Drawing.Size(153, 64);
            this.random_wave.TabIndex = 3;
            this.random_wave.Text = "KILKA SAMOCHODOW";
            this.random_wave.UseVisualStyleBackColor = true;
            this.random_wave.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(13, 428);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(124, 21);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "POJEDYNCZO";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(326, 457);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 64);
            this.button1.TabIndex = 5;
            this.button1.Text = "PIESZY NA PRZEJSCIU";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1888, 1037);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.random_wave);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.spawn_car);
            this.Controls.Add(this.spawn_train);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button spawn_train;
        private System.Windows.Forms.Button spawn_car;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button random_wave;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
    }
}

