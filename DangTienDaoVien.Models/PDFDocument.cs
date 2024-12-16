using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;

namespace DangTienDaoVien.Models
{
	public class PdfDocument : IDocument
	{
		public List<Chapter> Chapters { get; set; }
		public string Name { get; set; }

		public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

		public void Compose(IDocumentContainer container)
		{
			container
				.Page(page =>
				{
					page.Size(PageSizes.A4);
					page.Margin(2, Unit.Centimetre);
					page.PageColor(Colors.White);
					page.DefaultTextStyle(x => x.FontSize(20));

					page.Content().Column(column =>
					{
						column.Item().Text(Name).SemiBold().FontSize(48).FontColor(Colors.BlueGrey.Darken4).FontFamily("TitleFont");
						foreach (var chapter in Chapters)
						{
							column.Item().PageBreak();
							column.Item().Text(chapter.Title).SemiBold().FontSize(36).FontColor(Colors.Yellow.Accent4).FontFamily("TitleFont");
							column.Item().Text(chapter.Content).FontFamily("ContentFont").FontSize(20);
						}
					});

					page.Footer()
						.AlignCenter()
						.Text(x =>
						{
							x.Span("Page ");
							x.CurrentPageNumber();
						});
				});
		}

		public PdfDocument()
		{
			FontManager.RegisterFontWithCustomName("ContentFont", File.OpenRead("wwwroot/fonts/palatinolinotype_roman.ttf"));
			FontManager.RegisterFontWithCustomName("TitleFont", File.OpenRead("wwwroot/fonts/ZTNeueRalewe-MediumItalic.ttf"));
		}
	}
	
	public class Chapter
	{
		public string Title { get; set; }
		public string Content { get; set; }
	}
}
