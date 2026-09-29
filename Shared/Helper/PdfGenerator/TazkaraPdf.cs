using System;
using System.Collections.Generic;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Shared.Helper.PdfGenerator
{
	public class TazkaraPdf : ITazkaraPdf
	{
		public byte[] GeneratePdf(MatchTazkaraDto matchTazkaraDto)
		{
			QuestPDF.Settings.License = LicenseType.Community;
			QuestPDF.Settings.UseSystemFonts = true;
			QuestPDF.Settings.ThrowOnMissingFontFamilies = false;

			var primaryRed = Color.FromHex("#C91F37");
			var darkNavy = Color.FromHex("#0B132B");
			var matchCardNavy = Color.FromHex("#111C38");
			var goldAccent = Color.FromHex("#F59E0B");
			var cardBorder = Color.FromHex("#CBD5E1");
			var textDark = Color.FromHex("#0F172A");
			var textMuted = Color.FromHex("#64748B");
			var successGreen = Color.FromHex("#16A34A");
			var dangerRed = Color.FromHex("#DC2626");

			var isValid = matchTazkaraDto.IsActive ?? true;
			var bookingRef = $"TAZ-{Math.Abs((matchTazkaraDto.FanId ?? "0000").GetHashCode() % 900000 + 100000)}";

			var document = Document.Create(container =>
			{
				container.Page(page =>
				{
					page.Size(PageSizes.A4);
					page.Margin(24);
					page.PageColor(Color.FromHex("#F1F5F9"));
					page.DefaultTextStyle(x => x.FontFamily("Lato").FontSize(10).FontColor(textDark));

					page.Content().Column(col =>
					{
						// Top Official Tazkarti Header
						col.Item().PaddingBottom(12).Row(headerRow =>
						{
							headerRow.RelativeItem().Row(brandRow =>
							{
								brandRow.ConstantItem(95).Background(primaryRed).CornerRadius(6).PaddingVertical(6).PaddingHorizontal(8).Column(c =>
								{
									c.Item().AlignCenter().Text("TAZKARTI").FontSize(12).ExtraBold().FontColor(Colors.White);
									c.Item().AlignCenter().Text("تـذكـرتـي").FontSize(8).Bold().FontColor(Colors.White);
								});

								brandRow.RelativeItem().PaddingLeft(12).Column(c =>
								{
									c.Item().Text("OFFICIAL MATCH E-TICKET").FontSize(12).Bold().FontColor(darkNavy);
									c.Item().Text("Egyptian Stadiums & Entertainment Ticketing System").FontSize(8).FontColor(textMuted);
									c.Item().Text("التذكرة الإلكترونية الرسمية لدخول المباريات").FontSize(8).FontColor(textMuted);
								});
							});

							headerRow.ConstantItem(180).Column(rightCol =>
							{
								rightCol.Item().AlignRight().Text($"REF: {bookingRef}").FontSize(9).Bold().FontColor(darkNavy);
								rightCol.Item().AlignRight().PaddingTop(3).Row(badge =>
								{
									if (isValid)
									{
										badge.ConstantItem(110).Background(Color.FromHex("#DCFCE7")).CornerRadius(10).PaddingVertical(3).PaddingHorizontal(8)
											.AlignCenter().Text("✓ VALID & ACTIVE").FontSize(8).Bold().FontColor(successGreen);
									}
									else
									{
										badge.ConstantItem(110).Background(Color.FromHex("#FEE2E2")).CornerRadius(10).PaddingVertical(3).PaddingHorizontal(8)
											.AlignCenter().Text("⚠ INACTIVE").FontSize(8).Bold().FontColor(dangerRed);
									}
								});
							});
						});

						// Main Tazkara Card (Stadium Match Ticket)
						col.Item().Border(1.5f).BorderColor(cardBorder).CornerRadius(12).Background(Colors.White).Column(ticketCol =>
						{
							// Competition & Round Bar
							ticketCol.Item().Background(darkNavy).PaddingVertical(10).PaddingHorizontal(16).Row(matchHeader =>
							{
								matchHeader.RelativeItem().Column(c =>
								{
									c.Item().Text((matchTazkaraDto.CompetitionName ?? "EGYPTIAN PREMIER LEAGUE").ToUpper()).FontSize(11).Bold().FontColor(goldAccent);
									c.Item().Text($"{matchTazkaraDto.Round ?? "Official Matchday"} • {matchTazkaraDto.Title ?? "Match Ticket"}").FontSize(8.5f).FontColor(Colors.White);
								});

								matchHeader.ConstantItem(110).AlignRight().Background(primaryRed).CornerRadius(6).PaddingVertical(4).PaddingHorizontal(8)
									.AlignCenter().Text($"GATE: {matchTazkaraDto.Gate ?? "GENERAL"}").FontSize(9).Bold().FontColor(Colors.White);
							});

							// Teams Match Arena (Home vs Away)
							ticketCol.Item().Background(matchCardNavy).PaddingVertical(16).PaddingHorizontal(16).Row(teamsRow =>
							{
								// Home Team
								teamsRow.RelativeItem().AlignLeft().Column(c =>
								{
									c.Item().Text("HOME TEAM").FontSize(7.5f).Bold().FontColor(Color.FromHex("#94A3B8"));
									c.Item().Text(matchTazkaraDto.HomeTeamName ?? "Home Club").FontSize(15).Bold().FontColor(Colors.White);
									c.Item().PaddingTop(2).Text("مستضيف المباراة").FontSize(7.5f).FontColor(Color.FromHex("#94A3B8"));
								});

								// VS Badge
								teamsRow.ConstantItem(60).AlignCenter().Column(c =>
								{
									c.Item().AlignCenter().Background(primaryRed).CornerRadius(15).Width(32).Height(32).AlignCenter().AlignMiddle()
										.Text("VS").FontSize(11).ExtraBold().FontColor(Colors.White);
									c.Item().PaddingTop(2).AlignCenter().Text("ضــد").FontSize(7.5f).FontColor(Color.FromHex("#CBD5E1"));
								});

								// Away Team
								teamsRow.RelativeItem().AlignRight().Column(c =>
								{
									c.Item().AlignRight().Text("AWAY TEAM").FontSize(7.5f).Bold().FontColor(Color.FromHex("#94A3B8"));
									c.Item().AlignRight().Text(matchTazkaraDto.AwayTeamName ?? "Away Club").FontSize(15).Bold().FontColor(Colors.White);
									c.Item().AlignRight().PaddingTop(2).Text("الفريق الضيف").FontSize(7.5f).FontColor(Color.FromHex("#94A3B8"));
								});
							});

							// Ticket Body: Details & Stub
							ticketCol.Item().Row(bodyRow =>
							{
								// Left: Details Grid (70% width)
								bodyRow.RelativeItem(7).Padding(14).Column(detailsCol =>
								{
									// Row 1: Fan ID & Holder Name
									detailsCol.Item().Row(r =>
									{
										BuildInfoCard(r.RelativeItem(), "FAN ID / رقم بطاقة المشجع", matchTazkaraDto.FanId ?? "TAZ-000000", "#0B132B");
										r.ConstantItem(8);
										BuildInfoCard(r.RelativeItem(), "TICKET HOLDER / اسم المشجع", matchTazkaraDto.HolderName ?? "Fan Name", "#0B132B");
									});

									// Row 2: Gate & Price
									detailsCol.Item().PaddingTop(8).Row(r =>
									{
										BuildInfoCard(r.RelativeItem(), "STADIUM GATE / البوابة", matchTazkaraDto.Gate ?? "Main Gate", "#C91F37");
										r.ConstantItem(8);
										BuildInfoCard(r.RelativeItem(), "TICKET PRICE / السعر", $"{matchTazkaraDto.Price:F2} EGP", "#0B132B");
									});

									// Row 3: Stand Category & Round
									detailsCol.Item().PaddingTop(8).Row(r =>
									{
										BuildInfoCard(r.RelativeItem(), "CATEGORY / الفئة والدرجة", matchTazkaraDto.Title ?? "Standard Stand", "#0F172A");
										r.ConstantItem(8);
										BuildInfoCard(r.RelativeItem(), "ROUND / الجولة", matchTazkaraDto.Round ?? "Matchday Fixture", "#0F172A");
									});
								});

								// Perforation Divider
								bodyRow.ConstantItem(1).Background(Color.FromHex("#E2E8F0"));

								// Right: Tear-Off Stub (30% width)
								bodyRow.RelativeItem(3).Background(Color.FromHex("#FAFAFA")).Padding(12).Column(stubCol =>
								{
									stubCol.Item().AlignCenter().Text("ENTRY STUB").FontSize(7.5f).Bold().FontColor(textMuted);
									stubCol.Item().AlignCenter().Text("قسيمة الدخول").FontSize(7f).FontColor(textMuted);

									stubCol.Item().PaddingVertical(4).AlignCenter().Text(matchTazkaraDto.Gate ?? "GATE").FontSize(11).Bold().FontColor(primaryRed);

									// Barcode simulation
									stubCol.Item().PaddingVertical(6).AlignCenter().Width(110).Height(40).Background(Colors.White).Border(1).BorderColor(cardBorder).Padding(4).Row(bRow =>
									{
										int[] barWidths = [2, 1, 3, 1, 2, 4, 1, 2, 1, 3, 2, 1, 4, 1, 2, 3, 1, 2, 1, 3];
										for (int i = 0; i < barWidths.Length; i++)
										{
											bRow.ConstantItem(barWidths[i]).Background(Colors.Black);
											bRow.ConstantItem(2).Background(Colors.White);
										}
									});

									stubCol.Item().AlignCenter().Text($"*{matchTazkaraDto.FanId}*").FontSize(7.5f).FontColor(darkNavy);
									stubCol.Item().PaddingTop(4).AlignCenter().Text("SCAN AT TURNSTILE").FontSize(6.5f).Bold().FontColor(textMuted);
									stubCol.Item().AlignCenter().Text("المسح عند البوابة الإلكترونية").FontSize(6f).FontColor(textMuted);
								});
							});

							// Security Footer Bar
							ticketCol.Item().Background(Color.FromHex("#F1F5F9")).BorderTop(1).BorderColor(cardBorder).PaddingVertical(6).PaddingHorizontal(14).Row(secRow =>
							{
								secRow.RelativeItem().Text("🛡️ TAZKARTI VERIFIED ELECTRONIC ENTRY PASS • DIGITAL ENCRYPTED").FontSize(7.5f).Bold().FontColor(textDark);
								secRow.ConstantItem(180).AlignRight().Text($"HASH: {Math.Abs((matchTazkaraDto.FanId + matchTazkaraDto.HolderName + matchTazkaraDto.Price).GetHashCode()):X10}").FontSize(7f).FontColor(textMuted);
							});
						});

						// Stadium Rules & Instructions Card
						col.Item().PaddingTop(16).Background(Colors.White).Border(1).BorderColor(cardBorder).CornerRadius(10).Padding(14).Column(rulesCol =>
						{
							rulesCol.Item().Row(rTitle =>
							{
								rTitle.RelativeItem().Text("STADIUM ADMISSION INSTRUCTIONS | إرشادات وتعليمات دخول الاستاد").FontSize(10).Bold().FontColor(primaryRed);
								rTitle.ConstantItem(120).AlignRight().Text("TAZKARTI RULES").FontSize(8).Bold().FontColor(textMuted);
							});

							rulesCol.Item().PaddingTop(8).Column(rc =>
							{
								rc.Item().Row(row =>
								{
									BuildRuleItem(row.RelativeItem(), "1.", "Original Tazkarti Fan ID card is mandatory with this ticket at turnstiles.\nإحضار بطاقة المشجع (Fan ID) الأصلية شرط أساسي للدخول مع التذكرة.", primaryRed, textDark);
									row.ConstantItem(12);
									BuildRuleItem(row.RelativeItem(), "2.", "Stadium gates open 4 hours before kickoff and close 60 minutes prior.\nتفتح البوابات قبل 4 ساعات من المباراة وتغلق قبل ساعة كاملة من البداية.", primaryRed, textDark);
								});

								rc.Item().PaddingTop(8).Row(row =>
								{
									BuildRuleItem(row.RelativeItem(), "3.", "Strictly prohibited: Flares, fireworks, glass bottles, and laser pointers.\nممنوع دخول الألعاب النارية والشماريخ والزجاجات وشواحن الهواتف والليزر.", primaryRed, textDark);
									row.ConstantItem(12);
									BuildRuleItem(row.RelativeItem(), "4.", "Tickets are non-transferable and linked to the registered Fan ID holder.\nالتذكرة شخصية ومربوطة بحساب المشجع وغير قابلة للتداول أو إعادة البيع.", primaryRed, textDark);
								});
							});
						});

						// Support Info
						col.Item().PaddingTop(12).Row(supportRow =>
						{
							supportRow.RelativeItem().Text("Customer Care Hotline: 15355 | Website: www.tazkarti.com | Email: support@tazkarti.com").FontSize(7.5f).FontColor(textMuted);
							supportRow.ConstantItem(140).AlignRight().Text("© Tazkarti - All Rights Reserved").FontSize(7.5f).FontColor(textMuted);
						});
					});

					page.Footer().AlignCenter().Text(t =>
					{
						t.Span("Page ").FontSize(8).FontColor(textMuted);
						t.CurrentPageNumber().FontSize(8).FontColor(textMuted);
						t.Span(" of ").FontSize(8).FontColor(textMuted);
						t.TotalPages().FontSize(8).FontColor(textMuted);
					});
				});
			});

			return document.GeneratePdf();
		}

		private static void BuildInfoCard(IContainer container, string label, string value, string valueColorHex)
		{
			container.Background(Color.FromHex("#F8FAFC"))
				.Border(1)
				.BorderColor(Color.FromHex("#CBD5E1"))
				.CornerRadius(8)
				.Padding(8)
				.Column(c =>
				{
					c.Item().Text(label).FontSize(7.5f).Bold().FontColor(Color.FromHex("#64748B"));
					c.Item().PaddingTop(2).Text(value).FontSize(11.5f).Bold().FontColor(Color.FromHex(valueColorHex));
				});
		}

		private static void BuildRuleItem(IContainer container, string index, string text, Color accentColor, Color textColor)
		{
			container.Row(row =>
			{
				row.ConstantItem(16).Text(index).FontSize(8.5f).Bold().FontColor(accentColor);
				row.RelativeItem().Text(text).FontSize(7.5f).FontColor(textColor);
			});
		}
	}
}
