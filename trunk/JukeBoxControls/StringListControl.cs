using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JukeBoxControls
{
	public delegate void StringSelectedEventHandler(string selectedstring);

	public class StringListControl : System.Windows.Forms.UserControl
	{
		private List<string> _list;
		private int _currentindex = 0;
		private Label _selected;
		private List<Label> _resultlabels = new List<Label>();

		public event StringSelectedEventHandler StringSelected;

		private System.Windows.Forms.Label lblNext;
		private System.Windows.Forms.Label lblPrevious;

		private System.Windows.Forms.Label lbl1;
		private System.Windows.Forms.Label lbl2;
		private System.Windows.Forms.Label lbl3;
		private System.Windows.Forms.Label lbl4;
		private System.Windows.Forms.Label lbl5;
		private System.Windows.Forms.Label lbl6;
		private System.Windows.Forms.Label lbl7;
		private System.Windows.Forms.Label lbl8;
		private System.Windows.Forms.Label lbl9;
		private System.Windows.Forms.Label lbl10;
		private System.Windows.Forms.Label lblSummary;

		private System.ComponentModel.Container components = null;

		public StringListControl()
		{
			InitializeComponent();

			_resultlabels.Add(lbl1);
			_resultlabels.Add(lbl2);
			_resultlabels.Add(lbl3);
			_resultlabels.Add(lbl4);
			_resultlabels.Add(lbl5);
			_resultlabels.Add(lbl6);
			_resultlabels.Add(lbl7);
			_resultlabels.Add(lbl8);
			_resultlabels.Add(lbl9);
			_resultlabels.Add(lbl10);

			InitialiseResultLabels();

			Constants.SetLabelPresentation(lblSummary);

			Constants.SetLabelPresentation(lblNext);
			lblNext.Click+=new EventHandler(lblNext_Click);
			lblNext.MouseEnter+=new EventHandler(label_MouseEnter);
			lblNext.MouseLeave+=new EventHandler(label_MouseLeave);

			Constants.SetLabelPresentation(lblPrevious);
			lblPrevious.Click+=new EventHandler(lblPrevious_Click);
			lblPrevious.MouseEnter+=new EventHandler(label_MouseEnter);
			lblPrevious.MouseLeave+=new EventHandler(label_MouseLeave);
		}

		private void SetSelected(Label label)
		{
			foreach(Label l in _resultlabels) { Constants.SetDetailLabelPresentation(l); }
			_selected = label;
			Constants.SetLabelPresentationSelected(label);
		}

		private void StringSelectedNotify()
		{
			if(StringSelected!=null)
			{
				try
				{
					StringSelected(_selected.Text);
				} 
				catch {}
			}
		}

		private void label_MouseEnter(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			if (label==_selected) return;
			Constants.SetLabelPresentationHover(label);
		}

		private void label_MouseLeave(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			if (label==_selected) return;
			Constants.SetDetailLabelPresentation(label);
		}

		private void label_Click(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			Constants.SetLabelPresentationSelected(label);
			if (_selected==label) StringSelectedNotify();
			else SetSelected(label);
		}

		private void label_DoubleClick(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			Constants.SetLabelPresentationSelected(label);
			SetSelected(label);
			StringSelectedNotify();
		}

		private void ClearResults()
		{
			foreach(Label label in _resultlabels) { label.Visible = false; }
		}

		private void DisplayResults()
		{
			ClearResults();
			lblNext.Visible = true;
			lblPrevious.Visible = true;
			int currentindex = _currentindex;
			foreach(Label label in _resultlabels)
			{
				if (currentindex>=_list.Count) break;
				label.Text = (string)_list[currentindex++];
				label.Visible = true;
			}

			lblPrevious.Visible = (currentindex > _resultlabels.Count);
			lblNext.Visible = (currentindex + 1 <= _list.Count);

			if (currentindex==_currentindex) lblSummary.Text = string.Empty;
			else lblSummary.Text = string.Format("{0} to {1} of {2}", _currentindex + 1, currentindex, _list.Count);

			if (_selected==null) SetSelected((Label)_resultlabels[0]);
			else if (!_selected.Visible) SelectLastVisible();
		}

		private void SelectLastVisible()
		{
			foreach(Label control in _resultlabels)
			{
				if (control.Visible) SetSelected(_selected = control);
			}
		}

		public void NextPage()
		{
			if ((_currentindex + _resultlabels.Count) >= _list.Count) return;
			_currentindex+=_resultlabels.Count;
			DisplayResults();
		}

		public void PreviousPage()
		{
			if ((_currentindex-_resultlabels.Count)<0) _currentindex=0;
			else _currentindex-=_resultlabels.Count;
			DisplayResults();
		}

		public void NextItem()
		{
			Label firstlabel = (Label)_resultlabels[0];
	
			if (_selected==null) SetSelected(firstlabel);

			int index = _resultlabels.IndexOf(_selected);

			index++;

			if (index==_resultlabels.Count)
			{
				NextPage();
				if (!firstlabel.Text.Equals(firstlabel)) SetSelected(firstlabel);
			}
			else
			{
				Label nextlabel = (Label)_resultlabels[index];
				if (nextlabel.Visible) SetSelected(nextlabel);
			}
		}

		public void PreviousItem()
		{
			Label lastlabel = (Label)_resultlabels[_resultlabels.Count-1];
			int index = _resultlabels.IndexOf(_selected);

			index--;

			if (index<0)
			{
				string last = lastlabel.Text;
				PreviousPage();
				if (!lastlabel.Text.Equals(last)) SetSelected(lastlabel);
			}
			else
			{
				Label previouslabel = (Label)_resultlabels[index];
				SetSelected(previouslabel);
			}
		}

		public string Selected
		{
			get
			{
				if (_selected==null) return null;
				return _selected.Text;
			}
		}

		private void InitialiseResultLabels()
		{
			foreach(Label label in _resultlabels) { InitialiseResultLabel(label); }
		}

		private void InitialiseResultLabel(Label label)
		{
			Constants.SetDetailLabelPresentation(label);
			label.MouseEnter+=new EventHandler(label_MouseEnter);
			label.MouseLeave+=new EventHandler(label_MouseLeave);
			label.Click+=new EventHandler(label_Click);
			label.DoubleClick+=new EventHandler(label_DoubleClick);
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public List<string> Strings
		{
			//get { return _list; }
			set
			{
				_list = value;
				if (_list==null)
				{
					lblNext.Visible = false;
					lblPrevious.Visible = false;
				}
				else
				{
					lblNext.Visible = true;
					lblPrevious.Visible = true;
					_currentindex=0;
					DisplayResults();
				}
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lbl1 = new System.Windows.Forms.Label();
			this.lbl2 = new System.Windows.Forms.Label();
			this.lbl3 = new System.Windows.Forms.Label();
			this.lbl4 = new System.Windows.Forms.Label();
			this.lbl5 = new System.Windows.Forms.Label();
			this.lbl6 = new System.Windows.Forms.Label();
			this.lbl7 = new System.Windows.Forms.Label();
			this.lbl8 = new System.Windows.Forms.Label();
			this.lbl9 = new System.Windows.Forms.Label();
			this.lbl10 = new System.Windows.Forms.Label();
			this.lblNext = new System.Windows.Forms.Label();
			this.lblPrevious = new System.Windows.Forms.Label();
			this.lblSummary = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lbl1
			// 
			this.lbl1.Location = new System.Drawing.Point(8, 8);
			this.lbl1.Name = "lbl1";
			this.lbl1.Size = new System.Drawing.Size(728, 27);
			this.lbl1.TabIndex = 0;
			this.lbl1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbl1.UseMnemonic = false;
			// 
			// lbl2
			// 
			this.lbl2.Location = new System.Drawing.Point(8, 48);
			this.lbl2.Name = "lbl2";
			this.lbl2.Size = new System.Drawing.Size(728, 27);
			this.lbl2.TabIndex = 1;
			this.lbl2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbl2.UseMnemonic = false;
			// 
			// lbl3
			// 
			this.lbl3.Location = new System.Drawing.Point(8, 88);
			this.lbl3.Name = "lbl3";
			this.lbl3.Size = new System.Drawing.Size(728, 27);
			this.lbl3.TabIndex = 2;
			this.lbl3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbl3.UseMnemonic = false;
			// 
			// lbl4
			// 
			this.lbl4.Location = new System.Drawing.Point(8, 128);
			this.lbl4.Name = "lbl4";
			this.lbl4.Size = new System.Drawing.Size(728, 27);
			this.lbl4.TabIndex = 3;
			this.lbl4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbl4.UseMnemonic = false;
			// 
			// lbl5
			// 
			this.lbl5.Location = new System.Drawing.Point(8, 168);
			this.lbl5.Name = "lbl5";
			this.lbl5.Size = new System.Drawing.Size(728, 27);
			this.lbl5.TabIndex = 4;
			this.lbl5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbl5.UseMnemonic = false;
			// 
			// lbl6
			// 
			this.lbl6.Location = new System.Drawing.Point(8, 208);
			this.lbl6.Name = "lbl6";
			this.lbl6.Size = new System.Drawing.Size(728, 27);
			this.lbl6.TabIndex = 5;
			this.lbl6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbl6.UseMnemonic = false;
			// 
			// lbl7
			// 
			this.lbl7.Location = new System.Drawing.Point(8, 248);
			this.lbl7.Name = "lbl7";
			this.lbl7.Size = new System.Drawing.Size(728, 27);
			this.lbl7.TabIndex = 6;
			this.lbl7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbl7.UseMnemonic = false;
			// 
			// lbl8
			// 
			this.lbl8.Location = new System.Drawing.Point(8, 288);
			this.lbl8.Name = "lbl8";
			this.lbl8.Size = new System.Drawing.Size(728, 27);
			this.lbl8.TabIndex = 7;
			this.lbl8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbl8.UseMnemonic = false;
			// 
			// lbl9
			// 
			this.lbl9.Location = new System.Drawing.Point(8, 328);
			this.lbl9.Name = "lbl9";
			this.lbl9.Size = new System.Drawing.Size(728, 27);
			this.lbl9.TabIndex = 8;
			this.lbl9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbl9.UseMnemonic = false;
			// 
			// lbl10
			// 
			this.lbl10.Location = new System.Drawing.Point(8, 368);
			this.lbl10.Name = "lbl10";
			this.lbl10.Size = new System.Drawing.Size(728, 27);
			this.lbl10.TabIndex = 9;
			this.lbl10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbl10.UseMnemonic = false;
			// 
			// lblNext
			// 
			this.lblNext.BackColor = System.Drawing.Color.CornflowerBlue;
			this.lblNext.Location = new System.Drawing.Point(495, 395);
			this.lblNext.Name = "lblNext";
			this.lblNext.Size = new System.Drawing.Size(62, 40);
			this.lblNext.TabIndex = 19;
			this.lblNext.Text = ">";
			this.lblNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblPrevious
			// 
			this.lblPrevious.BackColor = System.Drawing.Color.CornflowerBlue;
			this.lblPrevious.Location = new System.Drawing.Point(3, 395);
			this.lblPrevious.Name = "lblPrevious";
			this.lblPrevious.Size = new System.Drawing.Size(56, 40);
			this.lblPrevious.TabIndex = 18;
			this.lblPrevious.Text = "<";
			this.lblPrevious.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSummary
			// 
			this.lblSummary.BackColor = System.Drawing.Color.CornflowerBlue;
			this.lblSummary.Location = new System.Drawing.Point(77, 403);
			this.lblSummary.Name = "lblSummary";
			this.lblSummary.Size = new System.Drawing.Size(403, 24);
			this.lblSummary.TabIndex = 20;
			this.lblSummary.Text = "1 to 6";
			this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// StringListControl
			// 
			this.Controls.Add(this.lblSummary);
			this.Controls.Add(this.lblNext);
			this.Controls.Add(this.lblPrevious);
			this.Controls.Add(this.lbl10);
			this.Controls.Add(this.lbl9);
			this.Controls.Add(this.lbl8);
			this.Controls.Add(this.lbl7);
			this.Controls.Add(this.lbl6);
			this.Controls.Add(this.lbl5);
			this.Controls.Add(this.lbl4);
			this.Controls.Add(this.lbl3);
			this.Controls.Add(this.lbl2);
			this.Controls.Add(this.lbl1);
			this.Name = "StringListControl";
			this.Size = new System.Drawing.Size(570, 438);
			this.ResumeLayout(false);

		}
		#endregion

		private void lblNext_Click(object sender, EventArgs e)
		{
			NextPage();
		}

		private void lblPrevious_Click(object sender, EventArgs e)
		{
			PreviousPage();
		}
	}
}
