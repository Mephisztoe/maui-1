using System;
using Microsoft.Maui.Essentials;

namespace Microsoft.Maui.Controls
{
	public static class LayoutDirectionExtensions
	{
		public static FlowDirection ToFlowDirection(this LayoutDirection layoutDirection) =>
			layoutDirection == LayoutDirection.RightToLeft
				? FlowDirection.RightToLeft
				: FlowDirection.LeftToRight;
	}
}