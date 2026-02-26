namespace WinFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            btnIniciar = new Button();
            loadingGif = new PictureBox();
            txtInput = new TextBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)loadingGif).BeginInit();
            SuspendLayout();
            // 
            // btnIniciar
            // 
            btnIniciar.Location = new Point(41, 107);
            btnIniciar.Name = "btnIniciar";
            btnIniciar.Size = new Size(229, 29);
            btnIniciar.TabIndex = 0;
            btnIniciar.Text = "Iniciar Proceso";
            btnIniciar.UseVisualStyleBackColor = true;
            btnIniciar.Click += btnIniciar_Click;
            // 
            // loadingGif
            // 
            loadingGif.Image = (Image)resources.GetObject("loadingGif.Image");
            loadingGif.Location = new Point(41, 160);
            loadingGif.Name = "loadingGif";
            loadingGif.Size = new Size(229, 226);
            loadingGif.SizeMode = PictureBoxSizeMode.Zoom;
            loadingGif.TabIndex = 1;
            loadingGif.TabStop = false;
            loadingGif.Visible = false;
            // 
            // txtInput
            // 
            txtInput.Location = new Point(111, 41);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(271, 27);
            txtInput.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(41, 41);
            label1.Name = "label1";
            label1.Size = new Size(64, 20);
            label1.TabIndex = 3;
            label1.Text = "Nombre";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(txtInput);
            Controls.Add(loadingGif);
            Controls.Add(btnIniciar);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)loadingGif).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnIniciar;
        private PictureBox loadingGif;
        private TextBox txtInput;
        private Label label1;
    }
}
