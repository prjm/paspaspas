using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PasPasPas.Api;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Common;

namespace P3SyntaxTreeViewer {

    internal class NodeVisitor : IStartEndVisitor {

        private Stack<TreeViewItem> items = new Stack<TreeViewItem>();
        private ITypedEnvironment env;
        private TreeView tv;

        public NodeVisitor(TreeView tv, ITypedEnvironment env) {
            this.tv = tv;
            this.env = env;
        }

        public void StartVisit<VisitorType>(VisitorType cst) {
            var parent = items.Count != 0 ? items.Peek() : null;

            var treeViewItem = new TreeViewItem();

            if (cst is Terminal terminal) {
                treeViewItem.Header = terminal.Token.Value;
                treeViewItem.Background = MainWindow.Black;
                treeViewItem.Foreground = MainWindow.Green;
            }
            else {
                treeViewItem.Header = cst.GetType().Name;
            }

            if (cst is ISymbolTableEntry symbol)
                treeViewItem.Header += ": " + symbol.SymbolName;

            if (cst is PasPasPas.Parsing.SyntaxTree.Types.ITypedSyntaxNode typeInfo && typeInfo.TypeInfo != null) {

                var t = env.TypeRegistry.GetTypeByIdOrUndefinedType(typeInfo.TypeInfo.TypeId);

                if (t.TypeId == KnownTypeIds.ErrorType) {
                    treeViewItem.Header += " [Type Error]";
                }
                else {
                    treeViewItem.Header += " [" + t.ToString() + "]";
                }

                if (typeInfo.TypeInfo.IsConstant) {
                    treeViewItem.Header += "* " + typeInfo.TypeInfo.ToString();
                }

                if (typeInfo.TypeInfo != null && typeInfo.TypeInfo.TypeId == KnownTypeIds.ErrorType)
                    treeViewItem.Foreground = new SolidColorBrush(Colors.Red);

            }

            if (cst is SymbolReferencePart srp) {
                treeViewItem.Header += " " + srp.Kind.ToString();
            }

            if (parent != null) {
                parent.Items.Add(treeViewItem);
            }
            else {
                tv.Items.Add(treeViewItem);
            }

            treeViewItem.IsExpanded = true;
            items.Push(treeViewItem);
        }

        public void EndVisit<VisitorType>(VisitorType element) => items.Pop();

    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();

            foreach (var font in Fonts.SystemFontFamilies) {
                if (string.Equals("hack", font.Source, StringComparison.OrdinalIgnoreCase)) {
                    Code.FontFamily = font;
                    Code.FontSize = 14;
                    break;
                }
            }

            WindowState = WindowState.Maximized;

            if (File.Exists(@"c:\temp\editor.pas"))
                Code.Text = File.ReadAllText(@"c:\temp\editor.pas", Encoding.UTF8);
        }

        protected override void OnClosing(CancelEventArgs e) {
            File.WriteAllText(@"c:\temp\editor.pas", Code.Text, Encoding.UTF8);
            base.OnClosing(e);
        }

        /// <summary>
        ///     parse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
            => UpdateTrees();

        private void UpdateTrees() {
            var code = Code.Text;
            var task = new Task(() => {
                var env = CreateEnvironment();
                var listLog = new ListLogTarget();
                env.Log.RegisterTarget(listLog);

                if (string.IsNullOrWhiteSpace(code)) {
                    Dispatcher.Invoke(() => {
                        StandardTreeView.Items.Clear();
                        AbstractTreeView.Items.Clear();
                        Messages.Items.Clear();
                    });
                    return;
                }

                (var bst, var ast, var typeNames) = Parse(env, code);

                Dispatcher.Invoke(() => {
                    DisplayTree(StandardTreeView, env, bst, null);
                    DisplayTree(AbstractTreeView, env, ast, typeNames);
                    DisplayLog(listLog.Messages);
                });
            });
            task.Start();
        }

        private void DisplayLog(IList<ILogMessage> messages) {
            Messages.Items.Clear();

            foreach (var logentry in messages) {
                var block = new TextBlock();
                var key = "m_" + logentry.MessageID.ToString("n");
                var m = key;
                var r = P3SyntaxTreeViewer.Messages.ResourceManager.GetString(key);
                if (r != null)
                    block.Text = r.ToString();
                else
                    block.Text = key;
                var item = new ListBoxItem() {
                    Content = block
                };
                Messages.Items.Add(item);
            }
        }

        private void DisplayTree(TreeView tv, ITypedEnvironment env, ISyntaxPart cst, Dictionary<int, string> typeNames) {
            tv.FontFamily = Code.FontFamily;
            tv.FontSize = Code.FontSize;
            tv.Items.Clear();
            cst.Accept(new NodeVisitor(tv, env));
        }

        internal static Brush Red = new SolidColorBrush(Colors.Red);
        internal static Brush Black = new SolidColorBrush(Colors.Black);
        internal static Brush Green = new SolidColorBrush(Colors.LightGreen);

        /// <summary>
        ///     parse the source
        /// </summary>
        /// <param name="env"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private (ISyntaxPart bst, ProjectRoot ast, Dictionary<int, string> typeNames) Parse(ITypedEnvironment env, string code) {
            var parserApi = new ParserApi(env);
            using (var parser = parserApi.CreateParserForString("z.x.pas", code)) {
                var bst = parser.Parse();
                var ast = parserApi.CreateAbstractSyntraxTree(bst);
                parserApi.AnnotateWithTypes(ast);

                var tn = new Dictionary<int, string>();
                return (bst, ast, tn);
            }
        }

        private ITypedEnvironment CreateEnvironment()
            => new DefaultEnvironment();

        private void Code_TextChanged(object sender, TextChangedEventArgs e)
            => UpdateTrees();
    }

}