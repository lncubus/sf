#region License
/*  Pvax.UI.Views
    Copyright (c) 2005, 2006, Alexey A. Popov
    All rights reserved.

    Redistribution and use in source and binary forms, with or without modification, are
    permitted provided that the following conditions are met:

    - Redistributions of source code must retain the above copyright notice, this list
      of conditions and the following disclaimer.

    - Redistributions in binary form must reproduce the above copyright notice, this list
      of conditions and the following disclaimer in the documentation and/or other materials
      provided with the distribution.

    - Neither the name of the Alexey A. Popov nor the names of its contributors may be used to
      endorse or promote products derived from this software without specific prior written
      permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS *AS IS* AND ANY EXPRESS
    OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
    AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
    CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
    DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
    DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER
    IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT
    OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System;
using System.Drawing;

namespace Pvax.UI.Views
{
	/// <summary>
	/// Specifies how an image is positioned within a <see cref="ImageView"/>.
	/// </summary>
	[Serializable]
	public enum ImageViewSizeMode
	{
		/// <summary>
		/// The <see cref="ImageView"/> is sized equal to the size of the image
		/// that it contains.
		/// </summary>
		AutoSize,

		/// <summary>
		/// The image is displayed in the center if the <see cref="ImageView"/>.
		/// </summary>
		CenterImage,

		/// <summary>
		/// The image is placed in the upper-left corner of
		/// the <see cref="ImageView"/>.
		/// </summary>
		Normal,

		/// <summary>
		/// The image within the <see cref="ImageView"/> is stretched or shrunk
		/// to fit the size of the <c>PictureBox</c>.
		/// </summary>
		StretchImage
	}

	/// <summary>
	/// Represents a light-weight view that displays an image.
	/// </summary>
	[Serializable]
	public class ImageView: View
	{
		private Image image;

		private ImageViewSizeMode sizeMode;

		/// <summary>
		/// Initializes a new instance of <see cref="ImageView"/>.
		/// </summary>
		public ImageView():
			this(null, 0, 0, 0, 0)
		{
			sizeMode = ImageViewSizeMode.Normal;
		}

		/// <summary>
		/// Initializes a new instance of <see cref="ImageView"/> with
		/// the <paramref name="image"/>.
		/// </summary>
		/// <param name="image">
		/// The <see cref="Image"/> to display in the view.
		/// </param>
		public ImageView(Image image):
			this(image, 0, 0, 0, 0)
		{}

		/// <summary>
		/// Initializes a new instance of <see cref="ImageView"/> with
		/// the <paramref name="image"/> and the view's bounds.
		/// </summary>
		/// <param name="image">
		/// The <see cref="Image"/> to display in the view.
		/// </param>
		/// <param name="x">The horizontal coordinate of the view relative to
		/// it's parent object.</param>
		/// <param name="y">The vertical coordinate of the view relative to
		/// it's parent object.</param>
		/// <param name="width">The width of the view.</param>
		/// <param name="height">The height of the view.</param>
		public ImageView(Image image, int x, int y, int width, int height):
			base(x, y, width, height)
		{
			sizeMode = ImageViewSizeMode.Normal;
			this.image = image;
		}

		private void FitToImage()
		{
			Width = image.Width;
			Height = image.Height;
		}

		/// <summary>
		/// Gets or sets the image that the view displays.
		/// </summary>
		/// <value>
		/// The <see cref="Image"/> to display in the view.
		/// </value>
		public Image Image
		{
			get
			{
				return image;
			}

			set
			{
				if(image != value)
				{
					image = value;
					if((null != value) && (ImageViewSizeMode.AutoSize == sizeMode))
						FitToImage();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets or sets the image sizing mode for the view.
		/// </summary>
		/// <value>
		/// The <see cref="ImageViewSizeMode"/> value that spedifies how
		/// the image is going toi be displayed in the view.
		/// </value>
		public ImageViewSizeMode SizeMode
		{
			get
			{
				return sizeMode;
			}

			set
			{
				if(sizeMode != value)
				{
					sizeMode = value;
					if(ImageViewSizeMode.AutoSize == sizeMode)
						FitToImage();
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Paints the <see cref="ImageView.Image"/>.
		/// </summary>
		/// <param name="graphics">An instance of the <see cref="Graphics"/>
		/// object to paint on.</param>
		protected override void Draw(Graphics graphics)
		{
			if(null == image)
				return;
			switch(this.sizeMode)
			{
				case ImageViewSizeMode.AutoSize:
				case ImageViewSizeMode.Normal:
					graphics.DrawImage(image, 0, 0);
					break;
				case ImageViewSizeMode.CenterImage:
					float cx = (Width - image.Width) / 2.0f;
					float cy = (Height - image.Height) / 2.0f;
					graphics.DrawImage(image, cx, cy);
					break;
				case ImageViewSizeMode.StretchImage:
					graphics.DrawImage(image, 0, 0, Width, Height);
					break;
			}
		}
	}
}
