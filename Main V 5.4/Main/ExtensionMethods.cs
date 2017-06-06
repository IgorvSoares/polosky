using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Main
{
    public static class ExtensionMethods
    {

        #region Converters

        #region FlatStleConverter
        public static FlatStyle ToFlatStyle(this string aValue)
        {
            switch (aValue)
            {
                case "Standard":
                    return FlatStyle.Standard;
                case "Flat":
                    return FlatStyle.Flat;
                case "Popup":
                    return FlatStyle.Popup;
                case "System":
                    return FlatStyle.System;
                default:
                    return FlatStyle.Standard;
            
            }
           
        }

        #endregion

        #region ColorConverter 
        public static Color ToColor(this string aValue)
        {
            Color Color = Color.FromArgb(int.Parse(aValue.Split(',')[0]), int.Parse(aValue.Split(',')[1]), int.Parse(aValue.Split(',')[2]), int.Parse(aValue.Split(',')[3]));//Converter a string para uma cor 
            return Color;
        }
        #endregion ColorConverter

        #region ImageBackGroundConverter
        public static ImageLayout ToBackGroundImageLayout(this string aValue)
        {
            switch (aValue)
            {
                case "Center":
                    return ImageLayout.Center;
                case "None":
                    return ImageLayout.None;
                case "Stretch":
                    return ImageLayout.Stretch;
                case "Tile":
                    return ImageLayout.Tile;
                case "Zoom":
                    return ImageLayout.Zoom;
                default:
                    return ImageLayout.None;

            }
        }

        #endregion

        #region DockStleConverter
        public static DockStyle ToDockStyle(this string aValue)
           {
               switch (aValue)
               {
                   case "Bottom":
                       return DockStyle.Bottom;
                   case "Fill":
                       return DockStyle.Fill;
                   case "Left":
                       return DockStyle.Left;
                   case "None":
                       return DockStyle.None;
                   case "Right":
                       return DockStyle.Right;
                   case "Top":
                       return DockStyle.Top;
                   default:
                       return DockStyle.None;

               }
           }

        #endregion DockStleConverter

        #region AlignConverter
        public static ContentAlignment ToAlign(this string aValue)
           {
               switch (aValue)
               {
                   case "BottomCenter":
                       return ContentAlignment.BottomCenter;
                   case "BottomLeft":
                       return ContentAlignment.BottomLeft;
                   case "BottomRight":
                       return ContentAlignment.BottomRight;
                   case "MiddleCenter":
                       return ContentAlignment.MiddleCenter;
                   case "MiddleLeft":
                       return ContentAlignment.MiddleLeft;
                   case "MiddleRight":
                       return ContentAlignment.MiddleRight;
                   case "TopCenter":
                       return ContentAlignment.TopCenter;
                   case "TopLeft":
                       return ContentAlignment.TopLeft;
                   case "TopRight":
                       return ContentAlignment.TopRight;
                   default:
                       return ContentAlignment.MiddleCenter;

               }
           }
        #endregion TextAlignConverter

        #region LocationConverter

        public static Point ToLocation(this string aValue)
        {
            Point ax = Point.Empty;

            string[] pontoSeparados = aValue.Split(';');

            ax.X = Convert.ToInt32(pontoSeparados[0]);
            ax.Y = Convert.ToInt32(pontoSeparados[1]);
            return ax;

        }
        #endregion

        #region Size
        public static Size ToSize(this string aValue)
        {
            Size ax = new Size();

            string[] pontoSeparados = aValue.Split(';');

            ax.Width = Convert.ToInt32(pontoSeparados[0]);
            ax.Height = Convert.ToInt32(pontoSeparados[1]);
           

            return ax;
        }


        #endregion

        #region SizeMode
        public static PictureBoxSizeMode ToSizeMode(this string aValue)
        {
            switch (aValue)
            {
                case "Center":
                    return PictureBoxSizeMode.Normal;
                case "None":
                    return PictureBoxSizeMode.StretchImage;
                case "Stretch":
                    return PictureBoxSizeMode.AutoSize;
                case "Tile":
                    return PictureBoxSizeMode.CenterImage;
                case "Zoom":
                    return PictureBoxSizeMode.Zoom;
                default:
                    return PictureBoxSizeMode.Normal;

            }

        }

        #endregion

        #region FontConverter
        public static FontFamily ToFontFamily(this string Avalue)
        {
            FontFamily e = new FontFamily(Avalue);
            return e;
        }

        public static Font ToFont(this string aValue)
        {
            //var cvt = new FontConverter();
            //Font f = cvt.ConvertFromString(aValue) as Font;
            Font e = new Font(aValue.Split(';')[0], float.Parse(aValue.Split(';')[1]), aValue.Split(';')[2].ToFontStyle());


            return e;
        }


        public static FontStyle ToFontStyle(this string aValue)
        {
            switch (aValue)
            {
                case "Bold":
                    return FontStyle.Bold;
                case "Italic":
                    return FontStyle.Italic;
                case "Regular":
                    return FontStyle.Regular;
                case "Strikeout":
                    return FontStyle.Strikeout;
                case "Underline":
                    return FontStyle.Underline;
                default:
                    return FontStyle.Regular;

            }

        }

     
        
        #endregion FontConverter

        #region FlatAppearance

        public static void AssignFlatAppearance(this Button ax,  string aValue)
        {

            ax.FlatAppearance.BorderColor = aValue.Split(';')[0].ToColor();
            ax.FlatAppearance.BorderSize = int.Parse(aValue.Split(';')[1]);
            ax.FlatAppearance.MouseDownBackColor = aValue.Split(';')[2].ToColor();
            ax.FlatAppearance.MouseOverBackColor = aValue.Split(';')[3].ToColor();
            

        }


        #endregion

        #region Visible Label

        public static void AssignVisibleControl(this Control ax, string aValue)
        {
            if (aValue == "True" || aValue == "On")
                ax.Visible = true;
            else if (aValue == "False" || aValue=="Off")
                ax.Visible = false;
            else
                ax.Visible = true;
           


        }

        #endregion 


        #endregion Converters




    }
}
