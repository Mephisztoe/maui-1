﻿namespace Microsoft.Maui.Controls
{
	public partial class Layout
	{
		public static void MapInputTransparent(LayoutHandler handler, Layout layout)
		{
			handler.PlatformView?.UpdateInputTransparent(layout);
			layout.UpdateDescendantInputTransparent();
		}
	}
}
