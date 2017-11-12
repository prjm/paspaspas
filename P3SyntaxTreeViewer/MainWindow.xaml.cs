﻿using PasPasPas.Infrastructure.Log;
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

                (var bst, var ast) = Parse(env, code);

                Dispatcher.Invoke(() => {
                    DisplayTree(StandardTreeView, bst);
                    DisplayTree(AbstractTreeView, ast);
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

        private void DisplayTree(TreeView tv, ISyntaxPart cst) {
            tv.Items.Clear();
            AddNodes(tv, null, cst);
        }

        private void AddNodes(TreeView tv, TreeViewItem parent, ISyntaxPart cst) {
            var treeViewItem = new TreeViewItem();
            var terminal = cst as Terminal;
            var symbol = cst as ISymbolTableEntry;
            var typeInfo = cst as PasPasPas.Parsing.SyntaxTree.Types.ITypedSyntaxNode;

            if (terminal != null) {
                treeViewItem.Header = "'" + terminal.Token.Value + "'";
            }
            else {
                treeViewItem.Header = cst.GetType().Name;
            }

            if (symbol != null)
                treeViewItem.Header += ": " + symbol.SymbolName;

            if (typeInfo != null && typeInfo.TypeInfo != null) {
                if (typeInfo.TypeInfo.TypeName != null)
                    treeViewItem.Header += " [" + typeInfo.TypeInfo.TypeName + "]";
                else
                    treeViewItem.Header += " [" + typeInfo.TypeInfo.TypeId.ToString() + "]";
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

        /// <summary>
        ///     parse the source
        /// </summary>
        /// <param name="env"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private (ISyntaxPart bst, ProjectRoot ast) Parse(ITypedEnvironment env, string code) {
            var parserApi = new ParserApi(env);
            using (var parser = parserApi.CreateParserForString("z.x.pas", code)) {
                var bst = parser.Parse();
                var ast = parserApi.CreateAbstractSyntraxTree(bst);
                parserApi.AnnotateWithTypes(ast);
                return (bst, ast);
            }
        }


        private ITypedEnvironment CreateEnvironment()
            => new DefaultEnvironment();

        private void Code_TextChanged(object sender, TextChangedEventArgs e)
            => UpdateTrees();
    }

}