using PasPasPas.DesktopPlatform;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Parsing.Tokenizer;
using System;
using System.Windows;
using System.Windows.Controls;

namespace P3SyntaxTreeViewer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }

        /// <summary>
        ///     parse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e) {
            UpdateTrees();
        }

        private void UpdateTrees() {
            var code = Code.Text;
            var env = CreateEnvironment();
            var cst = Parse(env, code);
            var visitor = new TreeTransformer();
            var options = new TreeTransformerOptions() { LogManager = (LogManager)env.Log };
            cst.Accept(visitor, options);
            DisplayTree(StandardTreeView, cst);
            DisplayTree(AbstractTreeView, options.Project);
        }

        private void DisplayTree(TreeView tv, ISyntaxPart cst) {
            tv.Items.Clear();
            AddNodes(tv, null, cst);
        }

        private void AddNodes(TreeView tv, TreeViewItem parent, ISyntaxPart cst) {
            var treeViewItem = new TreeViewItem();
            var terminal = cst as Terminal;

            if (terminal != null) {
                treeViewItem.Header = "'" + terminal.Token.Value + "'";
            }
            else {
                treeViewItem.Header = cst.GetType().Name;
            }

            if (parent != null) {
                parent.Items.Add(treeViewItem);
            }
            else {
                tv.Items.Add(treeViewItem);
            }

            foreach (var child in cst.Parts) {
                AddNodes(tv, treeViewItem, child);
            }

            treeViewItem.IsExpanded = true;
        }

        private ISyntaxPart Parse(ParserServices environment, string code) {
            StandardParser parser = new StandardParser(environment);
            using (var inputFile = new StringInput(code, new FileReference("z.x.pas")))
            using (var reader = new StackedFileReader()) {
                reader.AddFile(inputFile);
                parser.BaseTokenizer = new StandardTokenizer(environment, reader);
                return parser.Parse();
            }

        }

        private ParserServices CreateEnvironment() {
            var mgr = new LogManager();
            var options = new OptionSet(new StandardFileAccess());
            var environment = new ParserServices(mgr);
            environment.Options = options;
            return environment;
        }

        private void Code_TextChanged(object sender, TextChangedEventArgs e) {
            UpdateTrees();
        }
    }


}
