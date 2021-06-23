using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.IO;

namespace WorkerService1.Util
{
    class WindowsToast
    {
		public void GenerateToast(string appid, string imageFullPath, string h1, string h2, string p1)
		{

			var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

			var textNodes = template.GetElementsByTagName("text");

			textNodes[0].AppendChild(template.CreateTextNode(h1));
			textNodes[1].AppendChild(template.CreateTextNode(h2));
			textNodes[2].AppendChild(template.CreateTextNode(p1));

			if (File.Exists(imageFullPath))
			{
				XmlNodeList toastImageElements = template.GetElementsByTagName("image");
				((XmlElement)toastImageElements[0]).SetAttribute("src", imageFullPath);
			}
			IXmlNode toastNode = template.SelectSingleNode("/toast");
			((XmlElement)toastNode).SetAttribute("duration", "long");

			var notifier = ToastNotificationManager.CreateToastNotifier(appid);
			var notification = new ToastNotification(template);

			notifier.Show(notification);
		}
	}
}
