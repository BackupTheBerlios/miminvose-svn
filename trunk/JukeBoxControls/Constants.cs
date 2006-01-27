using System.Drawing;
using System.Windows.Forms;

namespace JukeBoxControls
{
	public class Constants
	{
		private static Color _colorformbackground = Color.Black;
		private static Color _labelfontcolor = Color.Aqua;
		private static Color _labelfontcolorhover = Color.Orange;
		private static Color _labelfontcolorselected = Color.Red;

		private Constants() {}

		public static void SetMainFormPresentation(Form form)
		{
			form.BackColor = _colorformbackground;
		}

		public static void SetPanelPresentation(Panel panel)
		{
			panel.BackColor = _colorformbackground;
		}

		public static void SetPicturePresentation(PictureBox pic)
		{
			pic.BackColor = _colorformbackground;
		}

		public static void SetLabelPresentation(Label label)
		{
			label.BackColor = _colorformbackground;
			label.ForeColor = _labelfontcolor;
			label.Font = new Font(FONTFAMILYNAME,20F);
		}

		public static void SetLabelPresentationHover(Label label)
		{
			label.BackColor = _colorformbackground;
			label.ForeColor = _labelfontcolorhover;
		}

		public static void SetLabelPresentationSelected(Label label)
		{
			label.BackColor = _colorformbackground;
			label.ForeColor = _labelfontcolorselected;
		}

		public static void SetDetailLabelPresentation(Label label)
		{
			label.BackColor = _colorformbackground;
			label.ForeColor = _labelfontcolor;
			label.Font = new Font(FONTFAMILYNAME,(float)label.Height/1.75F);
		}

		private static string FONTFAMILYNAME = "Verdana";
	}
}