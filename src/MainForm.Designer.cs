namespace TerrainNormals
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelControls = new System.Windows.Forms.Panel();
            this.buttonMesh = new System.Windows.Forms.Button();
            this.textBoxScale = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxRotation = new System.Windows.Forms.TextBox();
            this.buttonNormals = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonColor = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.panelView = new System.Windows.Forms.Panel();
            this.glControl = new OpenTK.GLControl();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelControls.SuspendLayout();
            this.panelView.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControls
            // 
            this.panelControls.Controls.Add(this.buttonMesh);
            this.panelControls.Controls.Add(this.textBoxScale);
            this.panelControls.Controls.Add(this.label2);
            this.panelControls.Controls.Add(this.textBoxRotation);
            this.panelControls.Controls.Add(this.buttonNormals);
            this.panelControls.Controls.Add(this.label1);
            this.panelControls.Controls.Add(this.buttonColor);
            this.panelControls.Controls.Add(this.buttonLoad);
            this.panelControls.Controls.Add(this.buttonSave);
            this.panelControls.Controls.Add(this.buttonQuit);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(241, 523);
            this.panelControls.TabIndex = 0;
            // 
            // buttonMesh
            // 
            this.buttonMesh.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonMesh.Location = new System.Drawing.Point(0, 355);
            this.buttonMesh.Name = "buttonMesh";
            this.buttonMesh.Size = new System.Drawing.Size(241, 27);
            this.buttonMesh.TabIndex = 13;
            this.buttonMesh.Text = "Export as OBJ Mesh";
            this.buttonMesh.UseVisualStyleBackColor = true;
            this.buttonMesh.Click += new System.EventHandler(this.buttonMesh_Click);
            // 
            // textBoxScale
            // 
            this.textBoxScale.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxScale.Location = new System.Drawing.Point(0, 68);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new System.Drawing.Size(241, 22);
            this.textBoxScale.TabIndex = 12;
            this.textBoxScale.TextChanged += new System.EventHandler(this.textBoxScale_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 45);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(6, 6, 0, 0);
            this.label2.Size = new System.Drawing.Size(53, 23);
            this.label2.TabIndex = 11;
            this.label2.Text = "Scale:";
            // 
            // textBoxRotation
            // 
            this.textBoxRotation.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxRotation.Location = new System.Drawing.Point(0, 23);
            this.textBoxRotation.Name = "textBoxRotation";
            this.textBoxRotation.Size = new System.Drawing.Size(241, 22);
            this.textBoxRotation.TabIndex = 10;
            this.textBoxRotation.TextChanged += new System.EventHandler(this.textBoxRotation_TextChanged);
            // 
            // buttonNormals
            // 
            this.buttonNormals.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonNormals.Location = new System.Drawing.Point(0, 382);
            this.buttonNormals.Name = "buttonNormals";
            this.buttonNormals.Size = new System.Drawing.Size(241, 28);
            this.buttonNormals.TabIndex = 8;
            this.buttonNormals.Text = "Generate Normals";
            this.buttonNormals.UseVisualStyleBackColor = true;
            this.buttonNormals.Click += new System.EventHandler(this.buttonNormals_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(6, 6, 0, 0);
            this.label1.Size = new System.Drawing.Size(71, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Rotation:";
            // 
            // buttonColor
            // 
            this.buttonColor.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonColor.Location = new System.Drawing.Point(0, 410);
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.Size = new System.Drawing.Size(241, 28);
            this.buttonColor.TabIndex = 3;
            this.buttonColor.Text = "Load Color Texture";
            this.buttonColor.UseVisualStyleBackColor = true;
            this.buttonColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonLoad.Location = new System.Drawing.Point(0, 438);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(241, 29);
            this.buttonLoad.TabIndex = 2;
            this.buttonLoad.Text = "Load Heightmap";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonSave.Location = new System.Drawing.Point(0, 467);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(241, 28);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save Heightmap";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonQuit
            // 
            this.buttonQuit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonQuit.Location = new System.Drawing.Point(0, 495);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(241, 28);
            this.buttonQuit.TabIndex = 0;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // panelView
            // 
            this.panelView.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panelView.Controls.Add(this.glControl);
            this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView.Location = new System.Drawing.Point(241, 0);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(551, 523);
            this.panelView.TabIndex = 1;
            // 
            // glControl
            // 
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.Location = new System.Drawing.Point(0, 0);
            this.glControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(551, 523);
            this.glControl.TabIndex = 0;
            this.glControl.VSync = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 523);
            this.Controls.Add(this.panelView);
            this.Controls.Add(this.panelControls);
            this.Name = "MainForm";
            this.Text = "Terrain Normal Mapper";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            this.panelView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.Panel panelView;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private OpenTK.GLControl glControl;
        private System.Windows.Forms.Button buttonColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonNormals;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.TextBox textBoxRotation;
        private System.Windows.Forms.TextBox textBoxScale;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonMesh;
    }
}

