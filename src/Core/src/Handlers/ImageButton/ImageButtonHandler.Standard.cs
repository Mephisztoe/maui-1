using System;

namespace Microsoft.Maui.Handlers
{
	public sealed partial class ImageButtonHandler : ViewHandler<IImageButton, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();

		public static void MapStrokeColor(IImageButtonHandler handler, IButtonStroke buttonStroke) { }
		public static void MapStrokeThickness(IImageButtonHandler handler, IButtonStroke buttonStroke) { }
		public static void MapCornerRadius(IImageButtonHandler handler, IButtonStroke buttonStroke) { }
		public static void MapPadding(IImageButtonHandler handler, IImageButton imageButton) { }

		void OnSetImageSource(object? obj)
		{
			throw new NotImplementedException();
		}
	}
}