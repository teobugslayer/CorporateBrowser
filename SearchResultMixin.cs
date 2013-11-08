using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CorporateBrowser
{
	public static class SearchResultMixin
	{
		public static string SafeReadString(this SearchResult item, string property)
		{
			var props = item.Properties[property];
			return props.Count > 0 ? props[0] as string : "";
		}

		public static ImageSource SafeReadPicture(this SearchResult item)
		{
			var jpegPhotoPropertyValue = item.Properties["jpegPhoto"];
			if (jpegPhotoPropertyValue.Count > 0)
			{
				try
				{
					var realBytes = jpegPhotoPropertyValue[0] as byte[];
					var memStream = new MemoryStream(realBytes, writable: false);
					var jpeg = new JpegBitmapDecoder(memStream, BitmapCreateOptions.None, BitmapCacheOption.Default);
					var result = jpeg.Frames[0];
					return result;
				}
				catch (FileFormatException ex)
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}
	}
}
