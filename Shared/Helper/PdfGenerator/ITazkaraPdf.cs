using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Helper.PdfGenerator
{
	public interface ITazkaraPdf
	{
		public byte[] GeneratePdf(MatchTazkaraDto matchTazkaraDto);
	}
}
