using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string filename = @"C:\Users\Administrator\Desktop\新建文件夹\uidump.xml";
            string str = System.IO.File.ReadAllText(filename);
            var temp = DeserializeFromXml(str);
            var t = temp.TextContains("淘").FirstOrDefault();
            Debug.WriteLine(t.Rect.CenterY);
        }
        static Hierarchy DeserializeFromXml(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Hierarchy));
            using (StringReader reader = new StringReader(xml))
            {
                return (Hierarchy)serializer.Deserialize(reader);
            }
        }

    }
    public static class HierarchyHelper
    {
        /// <summary>
        /// 根据Text查找节点
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public static IEnumerable<Node> Text(this Hierarchy hierarchy, string searchText)
        {
            IEnumerable<Node> nodes = hierarchy.RootNode.Children;
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithText(node.Children ?? new List<Node>(), searchText)))
                .Where(node => node.Text == searchText);
        }
        /// <summary>
        /// 根据Text模糊查找节点
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public static IEnumerable<Node> TextContains(this Hierarchy hierarchy, string searchText)
        {
            IEnumerable<Node> nodes = hierarchy.RootNode.Children;
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(ContainsText(node.Children ?? new List<Node>(), searchText)))
                .Where(node => node.Text.Contains(searchText));
        }
        /// <summary>
        /// 根据Package查找节点
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="searchPackage"></param>
        /// <returns></returns>
        public static IEnumerable<Node> Package(this Hierarchy hierarchy, string searchPackage)
        {
            IEnumerable<Node> nodes = hierarchy.RootNode.Children;
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithPackage(node.Children ?? new List<Node>(), searchPackage)))
                .Where(node => node.Package == searchPackage);
        }
        /// <summary>
        /// 根据Class查找节点
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="searchClass"></param>
        /// <returns></returns>
        public static IEnumerable<Node> Class(this Hierarchy hierarchy, string searchClass)
        {
            IEnumerable<Node> nodes = hierarchy.RootNode.Children;
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithClass(node.Children ?? new List<Node>(), searchClass)))
                .Where(node => node.Class == searchClass);
        }
        /// <summary>
        /// 根据包含的Package查找节点
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="searchPackage"></param>
        /// <returns></returns>
        public static IEnumerable<Node> PackageContains(this Hierarchy hierarchy, string searchPackage)
        {
            IEnumerable<Node> nodes = hierarchy.RootNode.Children;
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithPackageContains(node.Children ?? new List<Node>(), searchPackage)))
                .Where(node => node.Package.Contains(searchPackage));
        }
        /// <summary>
        /// 根据包含的Class查找节点
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="searchClass"></param>
        /// <returns></returns>
        public static IEnumerable<Node> ClassContains(this Hierarchy hierarchy, string searchClass)
        {
            IEnumerable<Node> nodes = hierarchy.RootNode.Children;
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithClassContains(node.Children ?? new List<Node>(), searchClass)))
                .Where(node => node.Class.Contains(searchClass));
        }
        /// <summary>
        /// 根据ResourceId查找节点
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="searchResourceId"></param>
        /// <returns></returns>
        public static IEnumerable<Node> ResourceId(this Hierarchy hierarchy, string searchResourceId)
        {
            IEnumerable<Node> nodes = hierarchy.RootNode.Children;
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithResourceId(node.Children ?? new List<Node>(), searchResourceId)))
                .Where(node => node.ResourceId == searchResourceId);
        }
        /// <summary>
        /// 根据包含的ResourceId查找节点
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="searchResourceId"></param>
        /// <returns></returns>
        public static IEnumerable<Node> ResourceIdContains(this Hierarchy hierarchy, string searchResourceId)
        {
            IEnumerable<Node> nodes = hierarchy.RootNode.Children;
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithResourceIdContains(node.Children ?? new List<Node>(), searchResourceId)))
                .Where(node => node.ResourceId.Contains(searchResourceId));
        }

        #region 私有方法
        private static IEnumerable<Node> FindNodesWithPackage(IEnumerable<Node> nodes, string searchPackage)
        {
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithPackage(node.Children ?? new List<Node>(), searchPackage)))
                .Where(node => node.Package == searchPackage);
        }
        private static IEnumerable<Node> FindNodesWithClass(IEnumerable<Node> nodes, string searchClass)
        {
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithClass(node.Children ?? new List<Node>(), searchClass)))
                .Where(node => node.Class == searchClass);
        }
        private static IEnumerable<Node> FindNodesWithPackageContains(IEnumerable<Node> nodes, string searchPackage)
        {
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithPackageContains(node.Children ?? new List<Node>(), searchPackage)))
                .Where(node => node.Package.Contains(searchPackage));
        }
        private static IEnumerable<Node> FindNodesWithClassContains(IEnumerable<Node> nodes, string searchClass)
        {
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithClassContains(node.Children ?? new List<Node>(), searchClass)))
                .Where(node => node.Class.Contains(searchClass));
        }
        private static IEnumerable<Node> ContainsText(IEnumerable<Node> nodes, string searchText)
        {
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(ContainsText(node.Children ?? new List<Node>(), searchText)))
                .Where(node => node.Text.Contains(searchText));
        }
        private static IEnumerable<Node> FindNodesWithText(IEnumerable<Node> nodes, string searchText)
        {
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithText(node.Children ?? new List<Node>(), searchText)))
                .Where(node => node.Text == searchText);
        }
        private static IEnumerable<Node> FindNodesWithResourceId(IEnumerable<Node> nodes, string searchResourceId)
        {
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithResourceId(node.Children ?? new List<Node>(), searchResourceId)))
                .Where(node => node.ResourceId == searchResourceId);
        }
        private static IEnumerable<Node> FindNodesWithResourceIdContains(IEnumerable<Node> nodes, string searchResourceId)
        {
            // 使用 SelectMany 来扁平化节点列表，并递归搜索每个子节点
            return nodes
                .SelectMany(node =>
                    new[] { node }.Concat(FindNodesWithResourceIdContains(node.Children ?? new List<Node>(), searchResourceId)))
                .Where(node => node.ResourceId.Contains(searchResourceId));
        }
        #endregion
    }

    [XmlRoot(ElementName = "hierarchy")]
    public class Hierarchy
    {
        [XmlAttribute(AttributeName = "rotation")]
        public int Rotation { get; set; }

        [XmlElement(ElementName = "node")]
        public Node RootNode { get; set; }
    }

    public class Node
    {
        [XmlAttribute(AttributeName = "index")]
        public int Index { get; set; }

        [XmlAttribute(AttributeName = "text")]
        public string Text { get; set; }

        [XmlAttribute(AttributeName = "class")]
        public string Class { get; set; }

        [XmlAttribute(AttributeName = "resource-id")]
        public string ResourceId { get; set; }

        [XmlAttribute(AttributeName = "package")]
        public string Package { get; set; }

        [XmlAttribute(AttributeName = "content-desc")]
        public string ContentDesc { get; set; }

        [XmlAttribute(AttributeName = "checkable")]
        public bool Checkable { get; set; }

        [XmlAttribute(AttributeName = "checked")]
        public bool Checked { get; set; }

        [XmlAttribute(AttributeName = "clickable")]
        public bool Clickable { get; set; }

        [XmlAttribute(AttributeName = "enabled")]
        public bool Enabled { get; set; }

        [XmlAttribute(AttributeName = "focusable")]
        public bool Focusable { get; set; }

        [XmlAttribute(AttributeName = "focused")]
        public bool Focused { get; set; }

        [XmlAttribute(AttributeName = "scrollable")]
        public bool Scrollable { get; set; }

        [XmlAttribute(AttributeName = "long-clickable")]
        public bool LongClickable { get; set; }

        [XmlAttribute(AttributeName = "password")]
        public bool Password { get; set; }

        [XmlAttribute(AttributeName = "selected")]
        public bool Selected { get; set; }

        [XmlAttribute(AttributeName = "bounds")]
        public string Bounds { get; set; }

        [XmlElement(ElementName = "node")]
        public List<Node> Children { get; set; }
        /// <summary>
        /// 矩形
        /// </summary>
        public Rect Rect
        {
            get
            {
                return ParseBounds(this.Bounds);
            }
        }

        private Rect ParseBounds(string boundsValue)
        {
            // 使用正则表达式提取坐标值
            MatchCollection matches = Regex.Matches(boundsValue, @"(\d+)");

            if (matches.Count == 4)
            {
                int x1 = int.Parse(matches[0].Value);
                int y1 = int.Parse(matches[1].Value);
                int x2 = int.Parse(matches[2].Value);
                int y2 = int.Parse(matches[3].Value);

                int x = Math.Min(x1, x2);
                int y = Math.Min(y1, y2);
                int width = Math.Abs(x2 - x1);
                int height = Math.Abs(y2 - y1);

                return new Rect(x, y, width, height);
            }
            else
            {
                throw new ArgumentException("Invalid Bounds format");
            }
        }
    }
    public struct Rect
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

        public Rect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public int CenterX
        {
            get { return X + Width / 2; }
        }

        public int CenterY
        {
            get { return Y + Height / 2; }
        }
    }
}
