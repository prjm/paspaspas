using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Api;
using PasPasPas.Typings.Common;
using System.Windows.Media;
using System;
using PasPasPas.Typings.Structured;
using PasPasPas.Global.Runtime;
using PasPasPas.Global.Constants;

namespace P3SyntaxTreeViewer {

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
            tv.Items.Clear();
            AddNodes(tv, null, env, cst, typeNames);
        }

        private void AddNodes(TreeView tv, TreeViewItem parent, ITypedEnvironment env, ISyntaxPart cst, Dictionary<int, string> typeNames) {
            var treeViewItem = new TreeViewItem();

            if (cst is Terminal terminal) {
                treeViewItem.Header = "'" + terminal.Token.Value + "'";
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

                if (typeInfo.TypeInfo.IsConstant)
                    treeViewItem.Header += "*";

                if (typeInfo.TypeInfo is IValue value) {
                    treeViewItem.Header += " = " + value.ToString();
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

            foreach (var child in cst.Parts) {
                AddNodes(tv, treeViewItem, env, child, typeNames);
            }

            treeViewItem.IsExpanded = true;
        }

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