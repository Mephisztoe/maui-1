using System.Linq;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Compatibility.ControlGallery
{
	[Preserve(AllMembers = true)]
	public class EntryCellTest
	{
		public string Label { get; set; }
		public string Placeholder { get; set; }
		public Color LabelColor { get; set; }
		public Color PlaceholderColor { get; set; }

	}

	public class EntryCellListPage : ContentPage
	{
		public EntryCellListPage()
		{
			Title = "EntryCell List Gallery - Legacy";

			if (DeviceInfo.Platform == DevicePlatform.iOS && DeviceInfo.Idiom == DeviceIdiom.Tablet)
				Padding = new Thickness(0, 0, 0, 60);

			var dataTemplate = new DataTemplate(typeof(EntryCell));
			dataTemplate.SetBinding(EntryCell.LabelProperty, new Binding("Label"));
			dataTemplate.SetBinding(EntryCell.LabelColorProperty, new Binding("LabelColor"));
			dataTemplate.SetBinding(EntryCell.PlaceholderProperty, new Binding("Placeholder"));

			var label = new Label
			{
				Text = "I have not been selected"
			};

			var listView = new ListView
			{
				AutomationId = CellTypeList.CellTestContainerId,
				ItemsSource = Enumerable.Range(0, 100).Select(i => new EntryCellTest
				{
					Label = "Label " + i,
					LabelColor = i % 2 == 0 ? Colors.Red : Colors.Blue,
					Placeholder = "Placeholder " + i,
					PlaceholderColor = i % 2 == 0 ? Colors.Red : Colors.Blue,
				}),
				ItemTemplate = dataTemplate
			};

			listView.ItemSelected += (sender, args) =>
			{
				label.Text = "I have been selected";
			};



			Content = new StackLayout { Children = { label, listView } };
		}
	}
}