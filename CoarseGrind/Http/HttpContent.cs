// Copyright (c) 2016 William Arthur Hood
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
using System;
using System.Text;

namespace Rockabilly.Common.Http
{
	public class HttpContent
	{
		public const string HEADER_KEY = "Content-Type";
		public const string TYPE_SUBTYPE_DELIMITER = "/";
		public string type = default(string);
		public string subtype = default(string);
		private const string BOUNDARY_IDENTIFIER = "boundary=";
		private const string BOUNDARY_MISSING = "Content Type header does not designate a multipart boundary.";

		public HttpContent() { }

		public HttpContent(string contentType, string contentSubtype)
		{
			type = contentType;
			subtype = contentSubtype;
		}

		public bool IsSet
		{
			get
			{
				return type != default(string);
			}
		}

		public string MultipartBoundary
		{
			get
			{
				if (!type.Equals(multipart)) throw new HttpMessageParseException(BOUNDARY_MISSING);
				int index = (subtype.LastIndexOf(BOUNDARY_IDENTIFIER));
				if (index == -1) throw new HttpMessageParseException(BOUNDARY_MISSING);
				index += BOUNDARY_IDENTIFIER.Length;
				return subtype.Substring(index);
			}
		}

		public override string ToString()
		{
			// Allow omission of Content-Type header by blanking out the type.
			if (type.IsBlank()) return string.Empty;

			StringBuilder result = new StringBuilder(type);
			if (!subtype.IsBlank())
			{
				result.Append(TYPE_SUBTYPE_DELIMITER);
				result.Append(subtype);
			}

			return result.ToString();
		}

		public HttpHeader ToHttpHeader
		{
			get
			{
				return new HttpHeader(HEADER_KEY, ToString());
			}
		}

		public static HttpContent FromString(string candidate)
		{
			HttpContent result = new HttpContent();
			string[] tmp = candidate.Split(TYPE_SUBTYPE_DELIMITER.ToCharArray());

			try
			{
				result.type = tmp[0];
				result.subtype = tmp[1];
			}
			catch (Exception dontCare)
			{
				// Deliberate NO-OP
				// Assuming this to be a missing subtype
			}

			return result;
		}

		public static HttpContent FromHttpHeader(HttpHeader candidate)
		{
			if (!candidate.Key.Equals(HEADER_KEY)) return null;
			return FromString(candidate.Value);
		}

		public bool IsMultipart
		{
			get
			{
				return type.Equals(multipart);
			}
		}

		public bool IsText
		{
			get
			{
				if (IsMultipart) return false;

				if (type.Equals(text)) return true;
				if (subtype.Equals(text)) return true;
				if (subtype.Equals(json)) return true;
				if (subtype.Equals(html)) return true;
				if (subtype.Equals(java)) return true;
				if (subtype.Equals(css)) return true;
				if (subtype.Equals(base64)) return true;
				if (subtype.Equals(pascal)) return true;
				if (subtype.Equals(x_lisp)) return true;
				if (subtype.Equals(x_script_lisp)) return true;
				if (subtype.Equals(richtext)) return true;
				if (subtype.Equals(rtf)) return true;
				if (subtype.Equals(x_rtf)) return true;
				if (subtype.Equals(x_json)) return true;
				if (subtype.Equals(tab_separated_values)) return true;
				if (subtype.Equals(csv)) return true;
				if (subtype.Equals(comma_separated_values)) return true;
				if (subtype.Equals(x_script_perl)) return true;
				if (subtype.Equals(javascript)) return true;
				if (subtype.Equals(x_javascript)) return true;
				if (subtype.Equals(ecmascript)) return true;
				if (subtype.Equals(plain)) return true;
				if (subtype.Equals(vrml)) return true;
				if (subtype.Equals(x_vrml)) return true;
				/*
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				if (subtype.Equals(xxxxxxxxxx)) return true;
				*/

				return false;
			}
		}

		public bool IsBinary
		{
			get
			{
				if (IsMultipart) return false;
				return !IsText;
			}
		}

		public const string application = "application";
		public const string image = "image";
		public const string video = "video";
		public const string audio = "audio";
		public const string text = "text";
		public const string model = "model";
		public const string chemical = "chemical";
		public const string x_conference = "x-conference";
		public const string multipart = "multipart";
		public const string x_www_form_urlencoded = "x-www-form-urlencoded";
		public const string form_data = "form-data";
		public const string mixed = "mixed";
		public const string x_world = "x-world";
		public const string html = "html";
		public const string postscript = "postscript";
		public const string mime = "mime";
		public const string octet_stream = "octet-stream";
		public const string avi = "avi";
		public const string macbinary = "macbinary";
		public const string mac_binary = "mac-binary";
		public const string bmp = "bmp";
		public const string book = "book";
		public const string plain = "plain";
		public const string java = "java";
		public const string java_byte_code = "java-byte-code";
		public const string x_java_class = "x-java-class";
		public const string css = "css";
		public const string msword = "msword";
		public const string x_dvi = "x-dvi";
		public const string x_fortran = "x-fortran";
		public const string gif = "gif";
		public const string jpeg = "jpeg";
		public const string javascript = "javascript";
		public const string x_javascript = "x-javascript";
		public const string ecmascript = "ecmascript";
		public const string midi = "midi";
		public const string x_karaoke = "x-karaoke";
		public const string x_lisp = "x-lisp";
		public const string x_script_lisp = "x-script.lisp";
		public const string mpeg = "mpeg";
		public const string x_mid = "x-mid";
		public const string x_midi = "x-midi";
		public const string crescendo = "crescendo";
		public const string x_motion_jpeg = "x-motion-jpeg";
		public const string base64 = "base64";
		public const string quicktime = "quicktime";
		public const string x_mpeg = "x-mpeg";
		public const string x_mpeq2a = "x-mpeq2a";
		public const string mpeg3 = "mpeg3";
		public const string x_mpeg_3 = "x-mpeg-3";
		public const string vnd_ms_project = "vnd.ms-project";
		public const string pascal = "pascal";
		public const string pdf = "pdf";
		public const string x_script_perl = "x-script.perl";
		public const string png = "png";
		public const string mspowerpoint = "mspowerpoint";
		public const string ms_powerpoint = "ms-powerpoint";
		public const string powerpoint = "powerpoint";
		public const string x_quicktime = "x-quicktime";
		public const string x_pn_realaudio = "x-pn-realaudio";
		public const string x_pn_realaudio_plugin = "x-pn-realaudio-plugin";
		public const string x_realaudio = "x-realaudio";
		public const string richtext = "richtext";
		public const string rn_realtext = "rn-realtext";
		public const string rtf = "rtf";
		public const string x_rtf = "x-rtf";
		public const string tiff = "tiff";
		public const string x_tiff = "x-tiff";
		public const string tab_separated_values = "tab-separated-values";
		public const string comma_separated_values = "comma-separated-values";
		public const string csv = "csv";
		public const string x_visio = "x-visio";
		public const string wordperfect6_0 = "wordperfect6.0";
		public const string wordperfect6_1 = "wordperfect6.1";
		public const string wav = "wav";
		public const string x_wav = "x-wav";
		public const string wordperfect = "wordperfect";
		public const string mswrite = "mswrite";
		public const string vrml = "vrml";
		public const string x_vrml = "x-vrml";
		public const string xbm = "xbm";
		public const string x_xbm = "x-xbm";
		public const string x_xbitmap = "x-xbitmap";
		public const string excel = "excel";
		public const string x_excel = "x-excel";
		public const string x_msexcel = "x-msexcel";
		public const string vnd_ms_excel = "vnd.ms-excel";
		public const string xml = "xml";
		public const string movie = "movie";
		public const string x_xpixmap = "x-xpixmap";
		public const string xpm = "xpm";
		public const string x_compress = "x-compress";
		public const string x_compressed = "x-compressed";
		public const string x_zip_compressed = "x-zip-compressed";
		public const string x_zip = "x-zip";
		public const string zip = "zip";
		public const string json = "json";
		public const string x_json = "x-json";
		public const string x_bzip = "x-bzip";
		public const string x_bzip2 = "x-bzip2";
		public const string x_chat = "x-chat";
		public const string x_gzip = "x-gzip";

		/*
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		public const string xxxxxxx = "xxxxxxx";
		*/
	}
}
