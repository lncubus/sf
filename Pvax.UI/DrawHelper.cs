using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Pvax.UI
{
	// Change with accordance with DrawHelper.BitShift constant
	using Key=System.Int32;

	/// <summary>
	/// Represents a special collection of disposable drawing resources for
	/// light-weight views.
	/// </summary>
	/// <remarks>
	/// <para>This class provides a set of factory methods for some types from
	/// <c>System.Drawing</c> and <c>System.Drawing.Drawing2D</c> namespaces
	/// that implement <see cref="IDisposable"/> interface. If you need a
	/// resource for drawing, call a proper method. The method creates a
	/// drawing object and keeps it in the internal hash tables. If another
	/// <c>IView</c>-based object draws itself and requests an already created
	/// object, the instance of the <c>DrawHelper</c> will reuse it thus
	/// reducing resources consumption and, in some cases, speeding the
	/// application.</para>
	/// <para>Also if you dispose the <c>DrawingHelper</c> instance you also
	/// disposing of all alloted graphical resources at once. So while using
	/// this facility you don't have to keep track of those graphical resource.
	/// </para>
	/// </remarks>
	public sealed class DrawHelper
	{
		#region Singleton pattern support
		/// <summary>
		/// Gets the single instance of the <see cref="DrawHelper"/> class.
		/// </summary>
		public static DrawHelper Instance
		{
			get
			{
				return Nested.instance;
			}
		}

		class Nested
		{
			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit
			static Nested()
			{
			}

			internal static readonly DrawHelper instance = new DrawHelper();
		}
		#endregion

		#region Resource cleanup
		void Cleanup()
		{
			if(0 == disposer.Count)
				return;
			foreach(IDisposable disposable in disposer)
				disposable.Dispose();
			disposer.Clear();

			colorPens.Clear();
			brushPens.Clear();
			solidBrushes.Clear();
			textureBrushes.Clear();
			fonts.Clear();
			linearGradientBrushes.Clear();
			hatchBrushes.Clear();
			pathGradientBrushes1.Clear();
			pathGradientBrushes2.Clear();
			pathGradientBrushes3.Clear();
			stringFormat1.Clear();
			stringFormat2.Clear();
			Trace.WriteLine("Resources cleaned up");
		}

		void OnIdle(object sender, EventArgs e)
		{
			Cleanup();
		}

		void OnApplicationExit(object sender, EventArgs e)
		{
			Cleanup();
			Application.Idle -= new EventHandler(OnIdle);
			Application.ApplicationExit -= new EventHandler(OnApplicationExit);
		}
		#endregion

		const int BitShift = 1;

		ArrayList disposer;

		#region Hashtable declarations
		// NB: Keep in sync with Dispose method and factory properties
		Hashtable colorPens;
		Hashtable brushPens;
		Hashtable solidBrushes;
		Hashtable textureBrushes;
		Hashtable fonts;
		Hashtable linearGradientBrushes;
		Hashtable hatchBrushes;
		Hashtable pathGradientBrushes1;
		Hashtable pathGradientBrushes2;
		Hashtable pathGradientBrushes3;
		Hashtable stringFormat1;
		Hashtable stringFormat2;
		#endregion

		DrawHelper()
		{
			Application.ApplicationExit += new EventHandler(OnApplicationExit);
			Application.Idle += new EventHandler(OnIdle);
			this.disposer = new ArrayList();
			this.colorPens = new Hashtable();
			this.brushPens = new Hashtable();
			this.solidBrushes = new Hashtable();
			this.textureBrushes = new Hashtable();
			this.fonts = new Hashtable();
			this.linearGradientBrushes = new Hashtable();
			this.hatchBrushes = new Hashtable();
			this.pathGradientBrushes1 = new Hashtable();
			this.pathGradientBrushes2 = new Hashtable();
			this.pathGradientBrushes3 = new Hashtable();
			this.stringFormat1 = new Hashtable();
			this.stringFormat2 = new Hashtable();
		}

		/// <summary>
		/// Creates a <see cref="Pen"/> object with <paramref name="color"/> and
		/// <paramref name="width"/>.
		/// </summary>
		/// <param name="color">
		/// A <see cref="Color"/> structure that represents the color of this
		/// pen.
		/// </param>
		/// <param name="width">
		/// A value indicating the width of the new <see cref="Pen"/> object.
		/// </param>
		/// <returns>
		/// A new instance of the <see cref="Pen"/> with specified
		/// <paramref name="color"/> and <paramref name="width"/>.
		/// </returns>
		public Pen CreateColorPen(Color color, float width)
		{
			Key key = color.GetHashCode();
			key <<= BitShift;
			key ^= width.GetHashCode();
			Pen result = (Pen)colorPens[key];
			if(null == result)
			{
				result = new Pen(color, width);
				colorPens.Add(key, result);
				disposer.Add(result);
			}
			return result;
		}

		/// <summary>
		/// Creates a <see cref="Pen"/> object with specified
		/// <paramref name="color"/>.
		/// </summary>
		/// <param name="color">
		/// A <see cref="Color"/> structure that represents the color of this
		/// pen.
		/// </param>
		/// <returns>
		/// A new instance of the <see cref="Pen"/> with specified
		/// <paramref name="color"/>.
		/// </returns>
		public Pen CreateColorPen(Color color)
		{
			return CreateColorPen(color, 1.0f);
		}

		/// <summary>
		/// Create a <see cref="Pen"/> object based on <paramref name="brush"/>
		/// and with specified <paramref name="width"/>.
		/// </summary>
		/// <param name="brush">
		/// A <see cref="Brush"/> object that determines the fill properties of
		/// this <see cref="Pen"/> object.
		/// </param>
		/// <param name="width">
		/// The width of the new <see cref="Pen"/> object.
		/// </param>
		/// <returns>
		/// A new instance of the <see cref="Pen"/> with specified
		/// <paramref name="brush"/> and <paramref name="width"/>.
		/// </returns>
		public Pen CreateBrushPen(Brush brush, float width)
		{
			if(null == brush)
				throw new ArgumentNullException("brush");

			Key key = brush.GetHashCode();
			key <<= BitShift;
			key ^= width.GetHashCode();
			Pen result = (Pen)brushPens[key];
			if(null == result)
			{
				result = new Pen(brush, width);
				brushPens.Add(key, result);
				disposer.Add(result);
			}
			return result;
		}

		/// <summary>
		/// Create a <see cref="Pen"/> object based on <paramref name="brush"/>.
		/// </summary>
		/// <param name="brush">
		/// A <see cref="Brush"/> object that determines the fill properties of
		/// this <see cref="Pen"/> object.
		/// </param>
		/// <returns>
		/// A new instance of the <see cref="Pen"/> with specified
		/// <paramref name="brush"/>.
		/// </returns>
		public Pen CreateBrushPen(Brush brush)
		{
			return CreateBrushPen(brush, 1.0f);
		}

		/// <summary>
		/// Creates a solid brush with specified <paramref name="color"/>.
		/// </summary>
		/// <param name="color">
		/// A <see cref="Color"/> structure that represents the color of this
		/// brush.
		/// </param>
		/// <returns>
		/// An instance of a <see cref="SolidBrush"/> object with specified
		/// <paramref name="color"/>.
		/// </returns>
		public SolidBrush CreateSolidBrush(Color color)
		{
			Key key = color.GetHashCode();
			SolidBrush result = (SolidBrush)solidBrushes[key];
			if(null == result)
			{
				result = new SolidBrush(color);
				disposer.Add(result);
				solidBrushes.Add(key, result);
			}
			return result;
		}

		/// <summary>
		/// Creates a <see cref="TextureBrush"/> object with
		/// <paramref name="image"/>, <paramref name="wrapMode"/> and
		/// <paramref name="dstRect"/>.
		/// </summary>
		/// <param name="image">
		/// The <see cref="Image"/> object with which the new
		/// <see cref="TextureBrush"/> object fills interiors.
		/// </param>
		/// <param name="wrapMode">
		/// A <see cref="WrapMode"/> enumeration value that specifies how
		/// the new <see cref="TextureBrush"/> object is tiled.
		/// </param>
		/// <param name="dstRect">
		/// A <see cref="RectangleF"/> structure that represents the bounding
		/// rectangle for the new <see cref="TextureBrush"/> object.
		/// </param>
		/// <returns>
		/// An instance of the <see cref="TextureBrush"/> with specified
		/// <paramref name="image"/>, <paramref name="wrapMode"/> and
		/// <paramref name="dstRect"/>.
		/// </returns>
		public TextureBrush CreateTextureBrush(Image image, WrapMode wrapMode, RectangleF dstRect)
		{
			if(null == image)
				throw new ArgumentNullException("image");
			Key key = image.GetHashCode();
			key <<= BitShift;
			key ^= wrapMode.GetHashCode();
			key <<= BitShift;
			key ^= dstRect.GetHashCode();
			TextureBrush result = (TextureBrush)textureBrushes[key];
			if(null == result)
			{
				result = new TextureBrush(image, wrapMode, dstRect);
				textureBrushes.Add(key, result);
				disposer.Add(result);
			}
			return result;
		}

		/// <summary>
		/// Creates a <see cref="TextureBrush"/> object with
		/// <paramref name="image"/>, <paramref name="wrapMode"/> and
		/// <paramref name="dstRect"/>.
		/// </summary>
		/// <param name="image">
		/// The <see cref="Image"/> object with which the new
		/// <see cref="TextureBrush"/> object fills interiors.
		/// </param>
		/// <param name="wrapMode">
		/// A <see cref="WrapMode"/> enumeration value that specifies how
		/// the new <see cref="TextureBrush"/> object is tiled.
		/// </param>
		/// <param name="dstRect">
		/// A <see cref="Rectangle"/> structure that represents the bounding
		/// rectangle for the new <see cref="TextureBrush"/> object.
		/// </param>
		/// <returns>
		/// An instance of the <see cref="TextureBrush"/> with specified
		/// <paramref name="image"/>, <paramref name="wrapMode"/> and
		/// <paramref name="dstRect"/>.
		/// </returns>
		public TextureBrush CreateTextureBrush(Image image, WrapMode wrapMode, Rectangle dstRect)
		{
			return CreateTextureBrush(image, wrapMode, new RectangleF(dstRect.Left, dstRect.Top, dstRect.Width, dstRect.Height));
		}

		/// <summary>
		/// Creates a <see cref="Font"/> object with
		/// <paramref name="familyName"/>, <paramref name="emSize"/>,
		/// <paramref name="style"/>, <paramref name="unit"/>,
		/// <paramref name="characterSet"/> and <paramref name="isVerticalFont"/>.
		/// </summary>
		/// <param name="familyName">
		/// A string representation of the <see cref="FontFamily"/> object for
		/// the new <see cref="Font"/> object.
		/// </param>
		/// <param name="emSize">
		/// The em-size of the new font in the units specified by the
		/// <paramref name="unit"/> parameter.
		/// </param>
		/// <param name="style">
		/// The <see cref="FontStyle"/> of the new font.
		/// </param>
		/// <param name="unit">
		/// The <see cref="GraphicsUnit"/> of the new font.
		/// </param>
		/// <param name="characterSet">
		/// A <c>Byte</c> that specifies a GDI character set to use for
		/// the new font.
		/// </param>
		/// <param name="isVerticalFont">
		/// <c>true</c> if the new <see cref="Font"/> object is derived from
		/// a GDI+ vertical font, <c>false</c> otherwise.
		/// </param>
		/// <returns>
		/// An instance of the <see cref="Font"/> class with specified
		/// <paramref name="familyName"/>, <paramref name="emSize"/>,
		/// <paramref name="style"/>, <paramref name="unit"/>,
		/// <paramref name="characterSet"/> and <paramref name="isVerticalFont"/>.
		/// </returns>
		public Font CreateFont(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte characterSet, bool isVerticalFont)
		{
			if(null == familyName)
				throw new ArgumentNullException("familyName");

			Key key = familyName.GetHashCode();
			key <<= BitShift;
			key ^= emSize.GetHashCode();
			key <<= BitShift;
			key ^= style.GetHashCode();
			key <<= BitShift;
			key ^= unit.GetHashCode();
			key <<= BitShift;
			key ^= characterSet.GetHashCode();
			key <<= BitShift;
			key ^= isVerticalFont.GetHashCode();
			Font result = (Font)fonts[key];
			if(null == result)
			{
				result = new Font(familyName, emSize, style, unit, characterSet, isVerticalFont);
				fonts.Add(key, result);
				disposer.Add(result);
			}
			return result;
		}

		/// <summary>
		/// Creates a <see cref="LinearGradientBrush"/> object with
		/// <paramref name="rect"/>, <paramref name="color1"/>,
		/// <paramref name="color2"/> and <paramref name="linearGradientMode"/>.
		/// </summary>
		/// <param name="rect">
		/// A <see cref="Rectangle"/> structure that specifies the bounds of
		/// the linear gradient.
		/// </param>
		/// <param name="color1">
		/// A <see cref="Color"/> structure that represents the starting color
		/// for the gradient.
		/// </param>
		/// <param name="color2">
		/// A <see cref="Color"/> structure that represents the ending color
		/// for the gradient.
		/// </param>
		/// <param name="linearGradientMode">
		/// A <see cref="LinearGradientMode"/> enumeration element that
		/// specifies the orientation of the gradient.
		/// </param>
		/// <returns>
		/// An instance of <see cref="LinearGradientBrush"/> with specified
		/// <paramref name="rect"/>, <paramref name="color1"/>,
		/// <paramref name="color2"/> and <paramref name="linearGradientMode"/>.
		/// </returns>
		public LinearGradientBrush CreateLinearGradientBrush(Rectangle rect, Color color1, Color color2, LinearGradientMode linearGradientMode)
		{
			Key key = rect.GetHashCode();
			key <<= BitShift;
			key ^= color1.GetHashCode();
			key <<= BitShift;
			key ^= color2.GetHashCode();
			key <<= BitShift;
			key ^= linearGradientMode.GetHashCode();
			LinearGradientBrush result = (LinearGradientBrush)linearGradientBrushes[key];
			if(null == result)
			{
				result = new LinearGradientBrush(rect, color1, color2, linearGradientMode);
				linearGradientBrushes.Add(key, result);
				disposer.Add(result);
			}
			return result;
		}

		/// <summary>
		/// Creates a <see cref="HatchBrush"/> object with
		/// <paramref name="hatchStyle"/>, <paramref name="foreColor"/> and
		/// <paramref name="backColor"/>.
		/// </summary>
		/// <param name="hatchStyle">
		/// The <see cref="HatchStyle"/> enumeration that represents the pattern
		/// drawn by the new <see cref="HatchBrush"/> object.
		/// </param>
		/// <param name="foreColor">
		/// The <see cref="Color"/> structure that represents the color of lines
		/// drawn by the new <see cref="HatchBrush"/> object.
		/// </param>
		/// <param name="backColor">
		/// The <see cref="Color"/> structure that represents the color of
		/// spaces between the lines drawn by new <see cref="HatchBrush"/>
		/// object.
		/// </param>
		/// <returns>
		/// An instance of the <see cref="HatchBrush"/> with specified
		/// <paramref name="hatchStyle"/>, <paramref name="foreColor"/> and
		/// <paramref name="backColor"/>.
		/// </returns>
		public HatchBrush CreateHatchBrush(HatchStyle hatchStyle, Color foreColor, Color backColor)
		{
			Key key = hatchStyle.GetHashCode();
			key <<= BitShift;
			key ^= foreColor.GetHashCode();
			key <<= BitShift;
			key ^= backColor.GetHashCode();
			HatchBrush result = (HatchBrush)hatchBrushes[key];
			if(null == result)
			{
				result = new HatchBrush(hatchStyle, foreColor, backColor);
				hatchBrushes.Add(key, result);
				disposer.Add(result);
			}
			return result;
		}

		/// <summary>
		/// Creates a <see cref="PathGradientBrush"/> object with
		/// <paramref name="path"/>.
		/// </summary>
		/// <param name="path">
		/// The <see cref="GraphicsPath"/> object that defines the area filled
		/// by the new <see cref="PathGradientBrush"/> object
		/// </param>
		/// <returns>
		/// An instance of the <see cref="PathGradientBrush"/> with specified
		/// <paramref name="path"/>.
		/// </returns>
		public PathGradientBrush CreatePathGradientBrush(GraphicsPath path)
		{
			if(null == path)
				throw new ArgumentNullException("path");

			Key key = path.GetHashCode();
			PathGradientBrush result = (PathGradientBrush)pathGradientBrushes1[key];
			if(null == result)
			{
				result = new PathGradientBrush(path);
				pathGradientBrushes1.Add(key, result);
				disposer.Add(result);
			}
			return result;
		}

		/// <summary>
		/// Creates a <see cref="PathGradientBrush"/> object with
		/// <paramref name="points"/> and <paramref name="wrapMode"/>.
		/// </summary>
		/// <param name="points">
		/// An array of <see cref="Point"/> structures that represents
		/// the points that make up the vertices of the path.
		/// </param>
		/// <param name="wrapMode">
		/// A <see cref="WrapMode"/> enumeration that specifies how fills drawn
		/// with the new <see cref="PathGradientBrush"/> object are tiled.
		/// </param>
		/// <returns>
		/// An instance of the <see cref="PathGradientBrush"/> with specified
		/// <paramref name="points"/> and <paramref name="wrapMode"/>.
		/// </returns>
		public PathGradientBrush CreatePathGradientBrush(Point[] points, WrapMode wrapMode)
		{
			Key key = 0;
			for(int i = 0; i < points.Length; i++)
			{
				key <<= BitShift;
				key ^= points[i].GetHashCode();
			}
			key <<= BitShift;
			key ^= wrapMode.GetHashCode();
			PathGradientBrush result = (PathGradientBrush)pathGradientBrushes2[key];
			if(null == result)
			{
				result = new PathGradientBrush(points, wrapMode);
				pathGradientBrushes2.Add(key, result);
				disposer.Add(result);
			}
			return result;
		}

		/// <summary>
		/// Creates a <see cref="PathGradientBrush"/> object with
		/// <paramref name="points"/>.
		/// </summary>
		/// <param name="points">
		/// An array of <see cref="Point"/> structures that represents
		/// the points that make up the vertices of the path.
		/// </param>
		/// <returns>
		/// An instance of the <see cref="PathGradientBrush"/> with specified
		/// <paramref name="points"/> and <paramref name="wrapMode"/>.
		/// </returns>
		public PathGradientBrush CreatePathGradientBrush(Point[] points)
		{
			return CreatePathGradientBrush(points, WrapMode.Clamp);
		}

		/// <summary>
		/// Creates a <see cref="PathGradientBrush"/> object with
		/// <paramref name="points"/> and <paramref name="wrapMode"/>.
		/// </summary>
		/// <param name="points">
		/// An array of <see cref="PointF"/> structures that represents
		/// the points that make up the vertices of the path.
		/// </param>
		/// <param name="wrapMode">
		/// A <see cref="WrapMode"/> enumeration that specifies how fills drawn
		/// with the new <see cref="PathGradientBrush"/> object are tiled.
		/// </param>
		/// <returns>
		/// An instance of the <see cref="PathGradientBrush"/> with specified
		/// <paramref name="points"/> and <paramref name="wrapMode"/>.
		/// </returns>
		public PathGradientBrush CreatePathGradientBrush(PointF[] points, WrapMode wrapMode)
		{
			Key key = 0;
			for(int i = 0; i < points.Length; i++)
			{
				key <<= BitShift;
				key ^= points[i].GetHashCode();
			}
			key <<= BitShift;
			key ^= wrapMode.GetHashCode();
			PathGradientBrush result = (PathGradientBrush)pathGradientBrushes3[key];
			if(null == result)
			{
				result = new PathGradientBrush(points, wrapMode);
				pathGradientBrushes3.Add(key, result);
				disposer.Add(result);
			}
			return result;
		}

		/// <summary>
		/// Creates a <see cref="PathGradientBrush"/> object with
		/// <paramref name="points"/>.
		/// </summary>
		/// <param name="points">
		/// An array of <see cref="PointF"/> structures that represents
		/// the points that make up the vertices of the path.
		/// </param>
		/// <returns>
		/// An instance of the <see cref="PathGradientBrush"/> with specified
		/// <paramref name="points"/> and <paramref name="wrapMode"/>.
		/// </returns>
		public PathGradientBrush CreatePathGradientBrush(PointF[] points)
		{
			return CreatePathGradientBrush(points, WrapMode.Clamp);
		}

		private static void Alignment(StringFormat format, ContentAlignment alignment)
		{
			switch(alignment)
			{
				case ContentAlignment.BottomCenter:
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Far;
					break;
				case ContentAlignment.BottomLeft:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Far;
					break;
				case ContentAlignment.BottomRight:
					format.Alignment = StringAlignment.Far;
					format.LineAlignment = StringAlignment.Far;
					break;
				case ContentAlignment.MiddleCenter:
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Center;
					break;
				case ContentAlignment.MiddleLeft:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Center;
					break;
				case ContentAlignment.MiddleRight:
					format.Alignment = StringAlignment.Far;
					format.LineAlignment = StringAlignment.Center;
					break;
				case ContentAlignment.TopCenter:
                    format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Near;
					break;
				case ContentAlignment.TopLeft:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Near;
					break;
				case ContentAlignment.TopRight:
					format.Alignment = StringAlignment.Far;
                    format.LineAlignment = StringAlignment.Near;
					break;
			}
		}

		/// <summary>
		/// Creates a clone of <see cref="StringFormat.GenericDefault"/> object
		/// with predefined <paramref name="alignment"/>.
		/// </summary>
		/// <param name="align">Content alignment</param>
		/// <returns>
		/// </returns>
		public StringFormat CreateDefaultStringFormat(ContentAlignment alignment)
		{
			Key key = alignment.GetHashCode();
			StringFormat result = (StringFormat)stringFormat1[key];
			if(null == result)
			{
				result = (StringFormat)StringFormat.GenericDefault.Clone();
				Alignment(result, alignment);
				stringFormat1.Add(key, result);
				disposer.Add(result);
			}
			return result;
		}

		/// <summary>
		/// Creates a clone of <see cref="StringFormat.GenericTypographic"/>
		/// with predefined <paramref name="alignment"/>.
		/// </summary>
		/// <param name="align">Content alignment</param>
		/// <returns>
		/// An exact copy of <see cref="StringFormat.GenericTypographic"/> that
		/// can be adjusted by the caller.
		/// </returns>
		public StringFormat CreateTypographicStringFormat(ContentAlignment alignment)
		{
			Key key = alignment.GetHashCode();
			StringFormat result = (StringFormat)stringFormat2[key];
			if(null == result)
			{
				result = (StringFormat)StringFormat.GenericTypographic.Clone();
				Alignment(result, alignment);
				stringFormat2.Add(key, result);
				disposer.Add(result);
			}
			return result;
		}
	}
}
